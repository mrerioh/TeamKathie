using UnityEngine;

public class PunchHitBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float knockback;
    
    public void Activate(float kb)
    {
        knockback=kb;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb=other.attachedRigidbody;
        if(rb==null) {
            return;
        }
        Vector3 dir=(other.transform.position - transform.parent.position).normalized;
        rb.AddForce(dir * knockback,ForceMode.Impulse);
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
