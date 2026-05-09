using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public InputActionAsset     InputActions;
    private InputActionMap      PlayerMap;
    private InputAction         MoveAction;
    private InputAction         JumpAction;
    private Vector3             MoveAmt;
    public float                Speed       = 5;
    public float                JumpSpeed   = 5;
    public float                GroundDist;

    public LayerMask            TerrainLayer;
    public Rigidbody            rb;
    public SpriteRenderer       sr;

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
        rb = gameObject.GetComponent<Rigidbody>();
        MoveAction = PlayerMap.FindAction( "Move" );
        JumpAction = PlayerMap.FindAction( "Jump" );
    }

    void JumpPlayer()
    {
        rb.AddForce( Vector3.up * JumpSpeed, ForceMode.Impulse );
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit Hit;
        Vector3    CastPos = transform.position;
        CastPos.y++;

        if( Physics.Raycast( CastPos, -transform.up, out Hit, Mathf.Infinity, TerrainLayer ) )
        {
            if( Hit.collider != null )
            {
                Vector3 MovePos = transform.position;
                MovePos.y = Hit.point.y + GroundDist;
                transform.position = MovePos;
            }
        }

        MoveAmt = MoveAction.ReadValue<Vector2>();

        rb.linearVelocity = new Vector3( MoveAmt.x * Speed,
                                         rb.linearVelocity.y,
                                         0 );

        if( JumpAction.WasPressedThisFrame() )
            JumpPlayer();

        // if( ( x != 0 ) && ( x < 0 ) )
        //     sr.flipX = true;
        // else if ( ( x != 0 ) && ( x > 0 ) )
        //     sr.flipX = false;
    }
}
