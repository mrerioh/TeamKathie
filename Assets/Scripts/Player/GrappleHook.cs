using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GrappleState
{
    GRAPPLE_ON_GROUND,
    GRAPPLE_READY_TO_USE,
    GRAPPLE_PREP,
    GRAPPLE_READY_TO_STOMP,
    GRAPPLE_STOMP
}

public class GrappleHook : MonoBehaviour
{

    [Header("References")]
    public  PlayerController           Pc;
    public  Camera                     Camera;
    public  Transform                  GrappleTip;
    [SerializeField] private LayerMask GrappleLayer;
    public  LineRenderer               lr;

    private Vector3                    GrapplePoint;
    private Vector3                    AnchorPoint;
    private DistanceJoint2D            Joint;
    public  InputActionAsset           InputActions;
    private InputActionMap             PlayerMap;
    private InputAction                Attack;
    private InputAction                Stomp;

    [Header("Grapple state parameters")]
    public float                       MaxGrappleLen;
    public float                       GrappleDelayTime;

    public  float                      GrappleCooldown;
    private float                      GrappleCooldownTimer;

    [Header("Stomp state parameters")]
    public  float                      StompCooldown;
    private float                      StompCooldownTimer;
    public float                       StompDelayTime;

    [Header("Grapple state booleans")]
    public GrappleState                GrappleState;
    bool                               IsReadyToUse;
    bool                               IsPreppingGrapple;
    bool                               IsReadyToStomp;
    bool                               IsStomped;

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
        // Pickup   = PlayerMap.FindAction( "Pickup" );
        Stomp    = PlayerMap.FindAction( "Stomp" );
    }

    private void StartGrapple()
    {
        if( GrappleCooldownTimer > 0 )
            return;

        Debug.Log("StartGrapple");
        IsPreppingGrapple = true;
        // Pc.IsPreppingGrapple = true;

        RaycastHit Hit;
        Vector2    MousePosition = Mouse.current.position.ReadValue();
        Ray        Ray = Camera.ScreenPointToRay( MousePosition );

        if( Physics.Raycast( Ray, out Hit, MaxGrappleLen, GrappleLayer ) )
        {
            GrapplePoint   = Hit.point;
            GrapplePoint.z = transform.position.z;
            IsReadyToStomp = true;
            Invoke( nameof( ExecuteGrapple ), GrappleDelayTime );
        }
        else
        {
            // If user misses the grapple, still need to StopGrapple();
            GrapplePoint   = Ray.origin + Ray.direction * MaxGrappleLen;
            GrapplePoint.z = transform.position.z;
            Invoke( nameof( StopGrapple ), GrappleDelayTime );
        }

        lr.enabled = true;
        lr.SetPosition( 1, GrapplePoint );
    }

    private void StartStomp()
    {
        if( StompCooldownTimer > 0 )
            return;

        Debug.Log("StartStomp");
        // if standing on grapple layer, lock in stomp and set IsStomped = true;
        RaycastHit Hit;
        Vector3    PlayerPosition = transform.position;
        if( Physics.Raycast( PlayerPosition, Vector3.down, out Hit, Mathf.Infinity, GrappleLayer ) )
        {
            Debug.Log("Detected GrappleLayer underneath StartStomp");
            AnchorPoint    = Hit.point;
            AnchorPoint.z  = transform.position.z;
            lr.SetPosition( 0, AnchorPoint );
            Invoke( nameof( ExecuteStomp ), StompDelayTime );
        }
        else
        {
            // Stop Stomp if not standing on grapple layer
            if( IsStomped == false )
                Invoke( nameof( StopStomp ), StompDelayTime );
        }

        return;
    }
    private void ExecuteStomp()
    {
        // Enable box collider here
        Debug.Log("ExecuteStomp");
        IsReadyToStomp = false;
        IsStomped      = true;
        lr.enabled     = true;
        return;
    }
    private void ExecuteGrapple()
    {
        lr.enabled = true;
        Debug.Log("ExecuteGrapple");
        return;
    }

    private void StopStomp()
    {
        // disable box collider here,  drop grapple hook at point of grapple layer. user will have to pick this up
        Debug.Log("StopStomp");
        // if standing on grapple layer, unlock grapple and set IsStomped = false;
        Vector3    PlayerPosition = transform.position;
        if( Physics.Raycast( PlayerPosition, Vector3.down, Mathf.Infinity, GrappleLayer ) )
        {
            Debug.Log("Detected GrappleLayer underneath StopStomp");
            // lr.enabled          = false;
            // IsStomped           = false;
            // StompCooldownTimer  = StompCooldown;
        }
        else
        {
            // Stop Stomp if not standing on grapple layer
            lr.enabled          = false;
            IsStomped           = false;
            IsReadyToStomp      = true;
            StompCooldownTimer  = StompCooldown;
                
        }
        return;
    }

    private void StopGrapple()
    {
        Debug.Log("StopGrapple");
        IsPreppingGrapple    = false;
        // Pc.IsPreppingGrapple = false;
        GrappleCooldownTimer = GrappleCooldown;
        lr.enabled           = false;
        return;
    }

    /*
    if( isreadytouse )
        if( prep grapple )
            render line to hit point
            set isreadytostomp
        if( isreadytostomp and stomp pressed )
            stomp (only stomp if user is standing on grapple layer, enable box collider of line renderer, set line renderer between the two grapple layers)
            set isstomped
        if( isstomped and stomp pressed)
            StopGrapple ( disable box collider, disable line renderer, drop grapple hook at point of grapple layer. user will have to pick this up)
    else
        if( pickup action )
            render grapple arm
            set is readytouse
        else
            render on ground
    */
    private void FsmHandler()
    {
        if( IsReadyToUse )
        {
            if( IsPreppingGrapple )
                GrappleState   = GrappleState.GRAPPLE_PREP;
            else if( IsReadyToStomp )
                GrappleState   = GrappleState.GRAPPLE_READY_TO_STOMP;
            else if( IsStomped )
                GrappleState   = GrappleState.GRAPPLE_STOMP;
        }
        else
            GrappleState = GrappleState.GRAPPLE_ON_GROUND;
            // if( PickUp.WasPressedThisFrame() )
            //     Pickup();
            // else
            //     RenderOnGround();
        // Debug.Log(GrappleState);
    }

    // Update is called once per frame
    void Update()
    {
        if( Attack.WasPressedThisFrame() )
        {
            StartGrapple();
        }

        if( Stomp.WasPressedThisFrame() && ( IsReadyToStomp == true ) )
        {
            StartStomp();
            return;
        }

        // if( Stomp.WasPressedThisFrame() && ( IsStomped == true ) )
        // {
        //     StopStomp();
        // }

        FsmHandler();

        if( GrappleCooldownTimer > 0)
            GrappleCooldownTimer -= Time.deltaTime;

        if( StompCooldownTimer > 0)
            StompCooldownTimer -= Time.deltaTime;

        if( IsReadyToStomp )
            lr.SetPosition( 0, GrappleTip.position );
    }
}
