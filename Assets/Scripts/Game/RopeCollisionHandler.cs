using System;
using UnityEngine;

public class RopeCollisionHandler : MonoBehaviour
{
    [SerializeField] private bool ExceedsWeight;
    [SerializeField] private int NumOnRope;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NumOnRope = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == 10)
        {
            Debug.Log("Hit");
            NumOnRope += 1;
            CheckWeight();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exited");
        NumOnRope -= 1;
    }
    private void CheckWeight()
    {
        if(NumOnRope == 3)
            this.gameObject.SetActive(false);
    }
}
