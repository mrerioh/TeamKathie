using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    public  LineCollider               lc;

    private Vector3                    GrapplePoint;
    private Vector3                    AnchorPoint;
    public  InputActionAsset           InputActions;
    private InputActionMap             PlayerMap;
    private InputAction                Attack;
    private InputAction                Stomp;
    private InputAction                Pull;

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
    public bool                        IsReadyToUse;
    bool                               IsPreppingGrapple;
    bool                               IsReadyToStomp;
    bool                               IsStomped;

    private bool                       EnableBoxCollider  = false;
    private bool                       EnableLineRenderer = false;

    [Header("Pull state parameters")]
    private GameObject                 HitObject;
    public  float                      PullCooldown;
    private float                      PullCooldownTimer;
    public float                       PullDelayTime;


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
        Attack                  = PlayerMap.FindAction( "Attack" );
        Pull                    = PlayerMap.FindAction( "Interact" );
        Stomp                   = PlayerMap.FindAction( "Stomp" );
        lc.boxCollider.enabled  = false;
        lc.lineRenderer.enabled = false;
        GrappleTip.position     = Pc.GetPlayerCenter();
    }

    private void StartGrapple()
    {
        if( GrappleCooldownTimer > 0 )
            return;

        Debug.Log("StartGrapple");
        IsPreppingGrapple = true;

        RaycastHit Hit;
        Vector2    MousePosition = Mouse.current.position.ReadValue();
        Ray        Ray = Camera.ScreenPointToRay( MousePosition );

        if( Physics.Raycast( Ray, out Hit, MaxGrappleLen, GrappleLayer ) )
        {
            GrapplePoint   = Hit.point;
            GrapplePoint.z = transform.position.z;
            IsReadyToStomp = true;
            HitObject      = Hit.collider.gameObject;
            Invoke( nameof( ExecuteGrapple ), GrappleDelayTime );
        }
        else
        {
            // If user misses the grapple, still need to StopGrapple();
            GrapplePoint   = Ray.origin + Ray.direction * MaxGrappleLen;
            GrapplePoint.z = transform.position.z;
            Invoke( nameof( StopGrapple ), GrappleDelayTime );
        }

        EnableLineRenderer = true;
        lc.lineRenderer.SetPosition( 1, GrapplePoint );
    }

    private void StartStomp()
    {
        if( StompCooldownTimer > 0 )
            return;

        Debug.Log("StartStomp");
        IsPreppingGrapple = false;
        // if standing on grapple layer, lock in stomp and set IsStomped = true;
        RaycastHit Hit;
        if( Physics.Raycast( transform.position, Vector3.down, out Hit, Pc.PlayerHeight * 0.5f + 0.2f, GrappleLayer ) )
        {
            AnchorPoint         = Hit.point;
            // Player "stomps" their arm down to leg level
            AnchorPoint.z       = transform.position.z;
            Invoke( nameof( ExecuteStomp ), StompDelayTime );
        }
        else
        {
            // Stop Stomp if not standing on grapple layer
            if( IsStomped == false )
                Invoke( nameof( StopStomp ), StompDelayTime );
        }
    }
    private void ExecuteStomp()
    {
        // Enable box collider here
        Debug.Log("ExecuteStomp");
        IsReadyToStomp          = false;
        IsStomped               = true;
        EnableLineRenderer      = true;
        EnableBoxCollider       = true;
    }
    private void ExecuteGrapple()
    {
        EnableLineRenderer      = true;
        Debug.Log("ExecuteGrapple");
    }

    private void StopStomp()
    {
        // disable box collider here,  drop grapple hook at point of grapple layer. user will have to pick this up
        Debug.Log("StopStomp");
        EnableLineRenderer       = false;
        EnableBoxCollider        = false;
        IsStomped                = false;
        StompCooldownTimer       = StompCooldown;
    }

    private void StopGrapple()
    {
        Debug.Log("StopGrapple");
        IsPreppingGrapple       = false;
        // Pc.IsPreppingGrapple = false;
        GrappleCooldownTimer    = GrappleCooldown;
        EnableLineRenderer      = false;
        EnableBoxCollider       = false;
    }

    private void FsmHandler()
    {
        if( IsReadyToUse )
        {
            // Follow Player
            transform.position = Pc.transform.position;

            if( IsPreppingGrapple )
            {
                GrappleState   = GrappleState.GRAPPLE_PREP;
            }
            else if( IsReadyToStomp )
            {
                GrappleState        = GrappleState.GRAPPLE_READY_TO_STOMP;
                GrappleTip.position = Pc.GetPlayerCenter();
            }
            else if( IsStomped )
            {
                GrappleState        = GrappleState.GRAPPLE_STOMP;
                GrappleTip.position = AnchorPoint;
            }
        }
        else
            GrappleState = GrappleState.GRAPPLE_ON_GROUND;
            // if( PickUp.WasPressedThisFrame() )
            //     Pickup();
            // else
            //     RenderOnGround();
        // Debug.Log(GrappleState);
    }

    private void StartPull()
    {
        if( PullCooldownTimer > 0 )
            return;

        Debug.Log("StartPull");
        
        if( HitObject.GetComponent<PullHandler>() != null )
            HitObject.GetComponent<PullHandler>().OnPulled();

        Invoke( nameof( StopPull ), PullDelayTime );
    }

    private void StopPull()
    {
        Debug.Log("StopPull");
        PullCooldownTimer    = PullCooldown;
        EnableLineRenderer   = false;
        EnableBoxCollider    = false;
        IsStomped            = false;

    }

    // Update is called once per frame
    void Update()
    {
        if( Attack.WasPressedThisFrame() )
        {
            StartGrapple();
        }

        if( IsReadyToStomp == true )
        {
            if( Stomp.WasPressedThisFrame() )
            {
                StartStomp();
                return;
            }

            else if( Pull.WasPressedThisFrame() )
            {
                StartPull();
                return;
            }
        }

        if( Stomp.WasPressedThisFrame() && ( IsStomped == true ) )
        {
            StopStomp();
        }

        FsmHandler();

        if( GrappleCooldownTimer > 0 )
            GrappleCooldownTimer -= Time.deltaTime;

        if( StompCooldownTimer > 0 )
            StompCooldownTimer -= Time.deltaTime;

        if( PullCooldownTimer > 0 )
            PullCooldownTimer -= Time.deltaTime;

        // GrappleTip.position updated as part of FSM handler
        lc.lineRenderer.SetPosition( 0, GrappleTip.position );
        lc.boxCollider.enabled  = EnableBoxCollider;
        lc.lineRenderer.enabled = EnableLineRenderer;
    }
}
