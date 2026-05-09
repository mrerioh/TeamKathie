using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;


public class PlayerController : MonoBehaviour
{
    public InputActionAsset           InputActions;
    private InputActionMap            PlayerMap;
    private InputAction               Move;
    private InputAction               Jump;
    private InputAction               Switch;
    private Vector2                   MoveDir;
    public float                      Speed       = 5;
    public float                      JumpSpeed   = 5;
    public float                      GroundDist;
    [SerializeField] private Animator animator;

    private EventInstance             PlayerFootsteps;

    public LayerMask                  TerrainLayer;
    public Rigidbody                  rb;
    public SpriteRenderer             sr;
    public float                      zFore        = 4.92f;
      
    public float                      zBack        = 14.07f;
    public bool                       isBackground = false;
    private int                       facingRight  = 1;

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
        PlayerFootsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.PlayerFootsteps);
    }

    void JumpPlayer()
    {
        rb.AddForceAtPosition( new Vector3(0, 5f, 0), Vector3.up, ForceMode.Impulse );
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

    void WalkPlayer()
    {
        return;
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

    // Update is called once per frame
    void Update()
    {

        MoveDir = Move.ReadValue<Vector2>();

        rb.linearVelocity = new Vector3( MoveDir.x * Speed,
                                         rb.linearVelocity.y,
                                         0 );

        if( Jump.WasPressedThisFrame() )
            JumpPlayer();

        if(Switch.WasPressedThisFrame())
            SwitchPlayer();

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
