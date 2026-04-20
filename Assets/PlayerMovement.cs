using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public InputActionAsset InputActions;

    private InputAction     moveAction;
    private InputAction     jumpAction;

    private Vector2         moveAmt;
    private Rigidbody2D     rb;

    public float            walkSpeed = 5;
    public float            jumpSpeed = 5;

    private void OnEnable()
    {
        InputActions.FindActionMap( "Player" ).Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap( "Player" ).Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction( "Move" );
        jumpAction = InputSystem.actions.FindAction( "Jump" );
        rb         = GetComponent<Rigidbody2D>();
    }

    void JumpPlayer()
    {
        rb.AddForceAtPosition( new Vector2( 0, jumpSpeed ), Vector2.up, ForceMode2D.Impulse );
    }

    // Update is called once per frame
    void Update()
    {
        moveAmt = moveAction.ReadValue<Vector2>();

        rb.linearVelocity = new Vector2 ( moveAmt.x * walkSpeed ,
                                          rb.linearVelocity.y );

        if( jumpAction.WasPressedThisFrame() )
            JumpPlayer();
    }
}
