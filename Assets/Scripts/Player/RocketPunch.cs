
using UnityEngine;
using UnityEngine.InputSystem;

public class RocketPunch : MonoBehaviour
{

    private PlayerController Pc;
    private Rigidbody rb;
    public InputActionAsset InputActions;
    private InputActionMap PlayerMap;
    private InputAction Punch;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator fistAnimator;
    

    [Header("Charge Mechanics ")]
    //public float MinChargeTime=0.15f;
    public float MaxChargeTime=1.0f;
    public float TapThreshold=0.15f;
    private int StoredChargeCoef=0;

    [Header("Punch")]

    public float MaxPunchSpeed=10f;
    public float MaxPunchLength=0.5f;

    public float punchCooldown=1;

    public float ChargeHoldMax=5f;
    public float ChargeHoldCur=0f;

    [Header("Hit stuff")]

    public PunchHitBox Hitbox;
    public Vector3 QuickHitboxScale=new Vector3(1f, 1f, 1f);
    public Vector3 HeavyHitboxScale=new Vector3(2.5f,1.5f,1f);
    public float QuickActiveTime=0.1f;
    public float HeavyActiveTime=0.2f;
    public float QuickKnockback=3f;
    public float HeavyKnockback=50f;
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

        if (ChargeHoldCur > 0f)
        {
            ChargeHoldCur-=Time.deltaTime;
            if(ChargeHoldCur<0f) {
                ChargeHoldCur=0f;
            }
        }


        if(Punch.IsPressed() && !Pc.IsPunching && CooldownTimer<=0)
        {
            Pc.IsChargingPunch=true;
            fistAnimator.SetBool("isCharging", true);
            playerAnimator.SetBool("isCharging", true);
            ChargeTimer+=Time.deltaTime;
        }
        if(Pc.IsChargingPunch && Punch.WasReleasedThisFrame())
        {
            if(ChargeTimer <TapThreshold)
            {
                //ChargeCoef=MaxChargeTime;

                if (ChargeHoldCur > 0f)
                {
                    LaunchPunch();
                }
                else
                {
                    QuickPunch();
                }
                
            }
            else if (ChargeTimer>=MaxChargeTime) 
            {
                ChargeHoldCur= ChargeHoldMax;
                Pc.IsChargingPunch = false;
                ChargeTimer= 0f;
                fistAnimator.SetBool("isCharging", false);
                playerAnimator.SetBool("isCharging", false);

                //animator.SetTrigger("ChargeStored");
            }
            else
            {
                Pc.IsChargingPunch = false;
                ChargeTimer = 0f;
                playerAnimator.SetBool("isCharging", false);
                fistAnimator.SetBool("isCharging", false);
            }

            
        }
        if(Pc.IsPunching) PunchFrames();
    }

    private void QuickPunch()
    {
        Pc.IsChargingPunch=false;
        Pc.IsPunching=true;
        PunchTimer=0f;
        
        ChargeTimer=0f;
        ChargeCoef=0.3f;

        fistAnimator.SetBool("isCharging", false);
        fistAnimator.SetTrigger("QuickPunch");
        playerAnimator.SetBool("isCharging", false);
        playerAnimator.SetTrigger("QuickPunch");

        //EnableQuickPunchHitbox();

        
    }

    private void LaunchPunch()
    {
        ChargeHoldCur=0f;
        Pc.IsChargingPunch=false;
        Pc.IsPunching=true;
        ChargeCoef=1f;
        ChargeTimer=0f;
        //rb.linearVelocity=dir * (MaxPunchSpeed*ChargeCoef);
        ChargeTimer=0f;

        fistAnimator.SetBool("isCharging", false);
        playerAnimator.SetTrigger("HeavyPunch");
        playerAnimator.SetBool("isCharging", false);
        fistAnimator.SetTrigger("HeavyPunch");

        //EnableHeavyPunchHitbox();
    }

    private void PunchFrames()
    {
        PunchTimer+=Time.deltaTime;
       // rb.linearVelocity=dir*(MaxPunchSpeed * ChargeCoef);
        //if(PunchTimer>=MaxPunchLength*ChargeCoef) StopPunch();
    if(PunchTimer>=MaxPunchLength*ChargeCoef) StopPunch();
    }

    private void StopPunch()
    {
        Pc.IsPunching=false;
        CooldownTimer=punchCooldown;

        Hitbox.Deactivate();
    }

    public void EnableQuickPunchHitbox()
    {
        Vector3 s = QuickHitboxScale;
        Hitbox.transform.localScale=s;
       Hitbox.Activate(QuickKnockback);
    }

    public void EnableHeavyPunchHitbox()
    {
        Vector3 s = HeavyHitboxScale;
        Hitbox.transform.localScale=s;
        Hitbox.Activate(HeavyKnockback);
    }

    public void DisablePunchHitbox()
    {
        Hitbox.Deactivate();
    }

    public void FinishPunch()
    {
        Pc.IsPunching=false;
        CooldownTimer=punchCooldown;
        Hitbox.Deactivate();
    }
}
