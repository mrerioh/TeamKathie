using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public float speed = 3f;
    public float sightRange = 10f;
    public bool onBackLayer;
    public float stunDuratrion=0.35f;
    private float stunTimer;

    Rigidbody rb;
    PlayerController player;

    public void stun(float duration)
    {
        stunTimer = Mathf.Max(stunTimer,duration);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        GameObject p = GameObject.Find("Player");
        if (p) player = p.GetComponent<PlayerController>();
    }

    void Update()
    {

        if(stunTimer>0f)
        {
            stunTimer-=Time.deltaTime;

            return;
        }
        if (!player || player.isBackground != onBackLayer)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        float dx = player.transform.position.x - transform.position.x;

        if (Mathf.Abs(dx) > sightRange)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        float dir = Mathf.Sign(dx);
        rb.linearVelocity = new Vector3(dir * speed, rb.linearVelocity.y, 0);
        transform.localScale = new Vector3(dir, 1, 1);
    }
}
