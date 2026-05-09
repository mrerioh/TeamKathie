using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleHook : MonoBehaviour
{

    [Header("References")]
    public  PlayerController           Pc;
    public  Camera                     Camera;
    public  Transform                  GrappleTip;
    [SerializeField] private LayerMask GrappleLayer;
    public  LineRenderer               lr;

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

    private void Awake()
    {
        PlayerMap = InputActions.FindActionMap( "Player" );
        Camera    = Camera.main;
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
        Attack   = PlayerMap.FindAction( "Attack" );
    }

    private void StartGrapple()
    {
        if( GrappleCooldownTimer > 0)
            return;

        IsGrappling          = true;
        Pc.IsPreppingGrapple = true;

        RaycastHit Hit;
        Vector2    MousePosition = Mouse.current.position.ReadValue();
        Ray        Ray = Camera.ScreenPointToRay( MousePosition );

        if( Physics.Raycast( Ray, out Hit, MaxGrappleLen, GrappleLayer ) )
        {
            GrapplePoint   = Hit.point;
            GrapplePoint.z = transform.position.z;

            Invoke( nameof( ExecuteGrapple ), GrappleDelayTime );
        }
        else
        {
            GrapplePoint   = Ray.origin + Ray.direction * MaxGrappleLen;
            GrapplePoint.z = transform.position.z;
            Invoke( nameof( StopGrapple ), GrappleDelayTime );
        }

        lr.enabled = true;
        lr.SetPosition( 1, GrapplePoint );
    }

    private void ExecuteGrapple()
    {
        Vector3 Direction    = ( GrapplePoint - transform.position ).normalized;
        Pc.IsPreppingGrapple = false;

    }

    private void StopGrapple()
    {
        IsGrappling          = false;
        Pc.IsPreppingGrapple = false;
        GrappleCooldownTimer = GrappleCooldown;
        lr.enabled           = false;

    }

    // Update is called once per frame
    void Update()
    {
        if( Attack.WasPressedThisFrame() )
            StartGrapple();

        if( GrappleCooldownTimer > 0)
            GrappleCooldownTimer -= Time.deltaTime;

        if( IsGrappling )
            lr.SetPosition( 0, GrappleTip.position );
    }
}
