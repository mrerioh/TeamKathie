using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public InputActionAsset InputActions;

    private InputActionMap    playerMap;
    private InputAction       moveAction;
    private InputAction       jumpAction;

    private InputAction switchAction;

    private Vector2           moveAmt;
    private Rigidbody         rb;

    public float              walkSpeed = 5;
    public float              jumpSpeed = 5;

    public float zFore = 4.92f;

    public float zBack = 14.07f;

    public bool isBackground=false;

    private void Awake()
    {
        playerMap = InputActions.FindActionMap( "Player" );
    }

    private void OnEnable()
    {
        playerMap.Enable();
    }

    private void OnDisable()
    {
        playerMap.Disable();
    }

    void Start()
    {
        moveAction = playerMap.FindAction( "Move" );
        jumpAction = playerMap.FindAction( "Jump" );
        switchAction = playerMap.FindAction("SwitchLayer");
        rb         = GetComponent<Rigidbody>();
    }

    void JumpPlayer()
    {
        rb.AddForce( Vector3.up * jumpSpeed, ForceMode.Impulse );
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

    void Update()
    {
        moveAmt = moveAction.ReadValue<Vector2>();

        rb.linearVelocity = new Vector3( moveAmt.x * walkSpeed, rb.linearVelocity.y, 0f );

        if( jumpAction.WasPressedThisFrame() )
            JumpPlayer();

        if(switchAction.WasPressedThisFrame())
            SwitchPlayer();
    }
}
