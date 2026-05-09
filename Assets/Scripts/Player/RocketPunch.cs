using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class RocketPunch : MonoBehaviour
{

    private PlayerController Pc;
    private Rigidbody rb;
    public InputActionAsset InputActions;
    private InputActionMap PlayerMap;
    private InputAction Punch;

    [Header("Charge Mechanics ")]
    //public float MinChargeTime=0.15f;
    public float MaxChargeTime=1.0f;

    [Header("Punch")]

    public float MaxPunchSpeed=15f;
    public float MaxPunchLength=0.5f;

    public float punchCooldown=1;

    [Header("Hit stuff")]

    public float hitRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float ChargeTimer;
    private float ChargeCoef;
    private float PunchTimer;
    private float CooldownTimer=0;
    private Vector3 dir;

    private void Awake()
    {
        PlayerMap = InputActions.FindActionMap( "Player" );
        Pc = GetComponent<PlayerController>();
        rb=GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        PlayerMap.Enable();
    }

    private void OnDisable()
    {
        PlayerMap.Disable();
    }
    void Start()
    {
        Punch=PlayerMap.FindAction("Punch");
    }



    // Update is called once per frame
    void Update()
    {
        if (CooldownTimer > 0)
        {
            CooldownTimer-=Time.deltaTime;
        }


        if(Punch.IsPressed() && !Pc.IsPunching && CooldownTimer<=0)
        {
            Pc.IsChargingPunch=true;
            ChargeTimer+=Time.deltaTime;
        }
        if(Pc.IsChargingPunch && Punch.WasReleasedThisFrame())
        {
            if(ChargeTimer>MaxChargeTime)
            {
                ChargeCoef=MaxChargeTime;
            }
            else
            {
                ChargeCoef=(ChargeTimer/MaxChargeTime);
            }

            LaunchPunch();
        }
        if(Pc.IsPunching) PunchFrames();
    }

    private void LaunchPunch()
    {
        Pc.IsChargingPunch=false;
        Pc.IsPunching=true;
        PunchTimer=0f;
        dir=new Vector3(Pc.facingRight, 0f, 0f);
        ChargeTimer=0f;
        rb.linearVelocity=dir * (MaxPunchSpeed*ChargeCoef);
        ChargeTimer=0f;
    }

    private void PunchFrames()
    {
        PunchTimer+=Time.deltaTime;
        rb.linearVelocity=dir*(MaxPunchSpeed * ChargeCoef);
        if(PunchTimer>=MaxPunchLength*ChargeCoef) StopPunch();
    }

    private void StopPunch()
    {
        Pc.IsPunching=false;
        CooldownTimer=punchCooldown;
    }
}
