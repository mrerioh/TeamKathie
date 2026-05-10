using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public float speed = 3f;
    public float sightRange = 10f;
    public bool onBackLayer;

    Rigidbody rb;
    PlayerController player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        GameObject p = GameObject.Find("Player");
        if (p) player = p.GetComponent<PlayerController>();
    }

    void Update()
    {
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
