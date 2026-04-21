using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed=5f;
    public float jumpSpeed=5f;
    private Rigidbody2D rb;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       float moveInput = Input.GetAxis("Horizontal");
       rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y); 
    }
}
