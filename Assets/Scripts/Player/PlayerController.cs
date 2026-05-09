using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public InputActionAsset     InputActions;
    private InputActionMap      PlayerMap;
    private InputAction         Move;
    private InputAction         Jump;
    private InputAction         Switch;
    private Vector2             MoveDir;
    public float                Speed       = 5;
    public float                JumpSpeed   = 5;
    public float                GroundDist;

    public LayerMask            TerrainLayer;
    public Rigidbody            rb;
    public SpriteRenderer       sr;
    public float                zFore        = 4.92f;

    public float                zBack        = 14.07f;
    public bool                 isBackground = false;

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

    // Update is called once per frame
    void Update()
    {
        // RaycastHit Hit;
        // Vector3    CastPos = transform.position;
        // CastPos.y++;

        // if( Physics.Raycast( CastPos, -transform.up, out Hit, Mathf.Infinity, TerrainLayer ) )
        // {
        //     if( Hit.collider != null )
        //     {
        //         Vector3 MovePos = transform.position;
        //         MovePos.y = Hit.point.y + GroundDist;
        //         transform.position = MovePos;
        //     }
        // }

        MoveDir = Move.ReadValue<Vector2>();

        rb.linearVelocity = new Vector3( MoveDir.x * Speed,
                                         rb.linearVelocity.y,
                                         0 );

        if( Jump.WasPressedThisFrame() )
            JumpPlayer();

        if(Switch.WasPressedThisFrame())
            SwitchPlayer();

        // if( ( x != 0 ) && ( x < 0 ) )
        //     sr.flipX = true;
        // else if ( ( x != 0 ) && ( x > 0 ) )
        //     sr.flipX = false;
    }
}
