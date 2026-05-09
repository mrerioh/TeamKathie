using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleHook : MonoBehaviour
{
    private PlayerController           Pc;
    public  Transform                  Camera;
    public  Transform                  GrappleTip;
    [SerializeField] private LayerMask GrappleLayer;
    private Vector3                    GrapplePoint;
    private DistanceJoint2D            Joint;
    public  InputActionAsset           InputActions;
    private InputActionMap             PlayerMap;
    private InputAction                Attack;

    public float                       MaxGrappleLen;
    public float                       GrappleDelayTime;

    public  float                      GrappleCooldown;
    private float                      GrappleCooldownTimer;

    private bool                       IsGrappling;

    public  LineRenderer               lr;

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
        Joint    = gameObject.GetComponent<DistanceJoint2D>();
        Joint.enabled = false;

        Attack   = PlayerMap.FindAction( "Attack" );
    }

    private void StartGrapple()
    {
        // if( GrappleCooldownTimer > 0)
        //     return;

        // IsGrappling = true;

        // RaycastHit hit;

        // if( Physics.Raycast( Camera.position, Camera.forward, out hit, MaxGrappleLen, GrappleLayer ) )
        // {
        //     GrapplePoint = hit.point;

        //     Invoke( nameof( ExecuteGrapple ), GrappleDelayTime );
        // }
        // else
        // {
        //     GrapplePoint = Camera.position + Camera.forward * MaxGrappleLen;
        //     Invoke( nameof( StopGrapple ), GrappleDelayTime );
        // }

        // lr.enabled = true;
        // lr.SetPosition( 1, GrapplePoint );
    }

    private void ExecuteGrapple()
    {
    }

    private void StopGrapple()
    {
        IsGrappling = false;
        GrappleCooldownTimer = GrappleCooldown;

        lr.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        // if( Attack.WasPressedThisFrame() )
        //     StartGrapple();

        // if( GrappleCooldownTimer > 0)
        //     GrappleCooldownTimer -= Time.deltaTime;

        // if( IsGrappling )
        //     lr.SetPosition( 0, GrappleTip.position );
    }
}
