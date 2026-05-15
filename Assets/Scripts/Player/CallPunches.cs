using UnityEngine;

public class CallPunches : MonoBehaviour
{
    private RocketPunch rocketPunch;
    private void Awake()
    {
        rocketPunch = GetComponentInParent<RocketPunch>();
    }
    public void EnableQuickPunchHitbox()
    {
        if (rocketPunch != null)
            rocketPunch.EnableQuickPunchHitbox();
    }
    public void EnableHeavyPunchHitbox()
    {
        if (rocketPunch != null)
            rocketPunch.EnableHeavyPunchHitbox();
    }
    public void DisablePunchHitbox()
    {
        if (rocketPunch != null)
            rocketPunch.DisablePunchHitbox();
    }
    public void FinishPunch()
    {
        if (rocketPunch != null)
            rocketPunch.FinishPunch();
    }
}
