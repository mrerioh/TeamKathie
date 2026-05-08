using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public InputActionAsset InputActions;

    private InputActionMap    playerMap;
    private InputAction       moveAction;
    private InputAction       jumpAction;

    private Vector2           moveAmt;
    private Rigidbody         rb;

    public float              walkSpeed = 5;
    public float              jumpSpeed = 5;

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
        rb         = GetComponent<Rigidbody>();
    }

    void JumpPlayer()
    {
        rb.AddForce( Vector3.up * jumpSpeed, ForceMode.Impulse );
    }

    void Update()
    {
        moveAmt = moveAction.ReadValue<Vector2>();

        rb.linearVelocity = new Vector3( moveAmt.x * walkSpeed,
                                         rb.linearVelocity.y,
                                         0 );

        if( jumpAction.WasPressedThisFrame() )
            JumpPlayer();
    }
}
