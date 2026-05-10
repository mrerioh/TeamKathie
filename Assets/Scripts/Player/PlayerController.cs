using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

public enum MovementState
{
    PREPPING_GRAPPLE,
    GRAPPLING,
    SWINGING,
    WALKING,
    SPRINTING,
    SWITCHING,
    MID_AIR
}

public class PlayerController : MonoBehaviour
{
    public InputActionAsset           InputActions;
    private InputActionMap            PlayerMap;
    [SerializeField] private Animator animator;

    private EventInstance             PlayerFootsteps;
    public Rigidbody                  rb;
    public SpriteRenderer             sr;

    public MovementState              PlayerMovementState;

    [Header("Grounding Info")]
    public float                      PlayerHeight;
    public LayerMask                  GroundLayer;
    bool                              IsGrounded;

    [Header("Walk Info")]
    private InputAction               Move;
    private Vector2                   MoveDir;
    public float                      Speed       = 5;
    private int                       facingRight  = 1;

    [Header("Switch Info")]
    private InputAction               Switch;
    public float                      zFore        = 4.92f;
    public float                      zBack        = 14.07f;
    public bool                       isBackground = false;
    bool                              IsSwitching;

    [Header("Jump Info")]
    private InputAction               Jump;
    bool                              IsReadyToJump;
    [SerializeField] private float    JumpCooldown;
    public float                      JumpSpeed   = 5;
    public float                      JumpForce   = 5f;

    [Header("Grapple Info")]
    public bool                       IsPreppingGrapple;
    bool                              IsGrappling;
    bool                              IsSwinging;

    private void Awake()
    {
        PlayerMap = InputActions.FindActionMap( "Player" );
    }

    private void OnEnable()
    {
        PlayerMap.Enable();
    }

    private void OnDisable()
    {
        PlayerMap.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb     = gameObject.GetComponent<Rigidbody>();
        Move   = PlayerMap.FindAction( "Move" );
        Jump   = PlayerMap.FindAction( "Jump" );
        Switch = PlayerMap.FindAction( "SwitchLayer" );
        //PlayerFootsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.PlayerFootsteps);
        IsReadyToJump = true;
    }

    private void MovePlayer()
    {
        if ( IsGrappling ) return;
        if ( IsSwinging )  return;

        MoveDir = Move.ReadValue<Vector2>();
        rb.linearVelocity = new Vector3( MoveDir.x * Speed,
                                         rb.linearVelocity.y,
                                         0 );
    }

    private void JumpPlayer()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        IsReadyToJump = true;
    }

    void SwitchPlayer()
    {
        Vector3 currentPos = rb.position;

        float newZ;
        if (isBackground==false)
        {
            newZ = zBack;
            isBackground=true;
        }
        else
        {
           newZ = zFore;
           isBackground=false; 
        }

        rb.position = new Vector3( currentPos.x, currentPos.y, newZ );
    }

    private void FsmHandler()
    {
        if ( IsPreppingGrapple )
        {
            PlayerMovementState = MovementState.PREPPING_GRAPPLE;
            Speed               = 0;
            rb.linearVelocity   = Vector3.zero;
        }
        else if ( IsGrappling )
        {
            PlayerMovementState = MovementState.GRAPPLING;
            Speed               = 5;
            rb.useGravity       = false;
        }
        else if ( IsSwinging )
        {
            PlayerMovementState = MovementState.SWINGING;
            Speed               = 5;
        }
        else if ( IsGrounded )
        {
            PlayerMovementState = MovementState.WALKING;
            Speed               = 5;
        }
        else if( IsSwitching )
        {
            PlayerMovementState = MovementState.SWITCHING;
            Speed               = 0;
            rb.linearVelocity   = Vector3.zero;
        }
        else
        {
            PlayerMovementState = MovementState.MID_AIR;
        }
        Debug.Log(PlayerMovementState);
    }

    private void UpdateSound()
    {
        if( rb.linearVelocity.x != 0 )
        {
            PLAYBACK_STATE PlaybackState;
            PlayerFootsteps.getPlaybackState( out PlaybackState );
            if( PlaybackState.Equals( PLAYBACK_STATE.STOPPED ) )
                PlayerFootsteps.start();

        }
        else
        {
            PlayerFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void ResetRestrictions()
    {
        IsGrappling = false;
    }

    public void JumpToPosition( Vector3 TargetPoint )
    {
        IsGrappling = true;


        Invoke( nameof(ResetRestrictions), 3f );
    }

    // Update is called once per frame
    void Update()
    {

        IsGrounded = Physics.Raycast( transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, GroundLayer );

        MovePlayer();

        if( Jump.WasPressedThisFrame() && IsGrounded && IsReadyToJump )
        {
            IsReadyToJump = false;
            JumpPlayer();
            Invoke( nameof( ResetJump ), JumpCooldown );
        }

        if(Switch.WasPressedThisFrame())
            SwitchPlayer();

        FsmHandler();

        // Animation switching
        if( rb.linearVelocity.x != 0 )
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // Character facing right/left
        if( rb.linearVelocity.x > 0 )
        {
            facingRight = 1;
            transform.localScale = new Vector3(facingRight, 1, 1);
        }
        else if ( rb.linearVelocity.x < 0 )
        {
            facingRight = -1;
            transform.localScale = new Vector3(facingRight, 1, 1);
        }

        // Update sound
        UpdateSound();

    }
}
