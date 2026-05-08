using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float          Speed;
    public float          GroundDist;

    public LayerMask      TerrainLayer;
    public Rigidbody      rb;
    public SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit Hit;
        Vector3    CastPos = transform.position;
        CastPos.y++;

        if( Physics.Raycast( CastPos, -transform.up, out Hit, Mathf.Infinity, TerrainLayer ) )
        {
            if( Hit.collider != null )
            {
                Vector3 MovePos = transform.position;
                MovePos.y = Hit.point.y + GroundDist;
                transform.position = MovePos;
            }
        }

        // float x = Input.GetAxis("Horizontal");
        // float y = Input.GetAxis("Vertical");
        // Vector3 MoveDir = new Vector3(x, 0, y);
        // rb.linearVelocity = MoveDir * Speed;

        // if( ( x != 0 ) && ( x < 0 ) )
        //     sr.flipX = true;
        // else if ( ( x != 0 ) && ( x > 0 ) )
        //     sr.flipX = false;
    }
}
