using System;
using UnityEngine;

public class RopeCollisionHandler : MonoBehaviour
{
    [SerializeField] private bool ExceedsWeight;
    [SerializeField] private int NumOnRope;
    [SerializeField] private int NumToBreak = 0;
    [SerializeField] private GameObject LeftEnd;
    [SerializeField] private GameObject RightEnd;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NumOnRope = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            if(!InventoryManager.IsBalanced)
                CutEnds();

        if (other.gameObject.layer == 10)
        {
            Debug.Log("Hit");
            NumOnRope += 1;
            CheckWeight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited");
        NumOnRope -= 1;
    }


    private void CheckWeight()
    {
        if (NumOnRope == NumToBreak)
            CutEnds();
    }

    void CutEnds()
    {
        if (this.RightEnd != null)
            RightEnd.gameObject.SetActive(false);
        if (this.LeftEnd != null)
            LeftEnd.gameObject.SetActive(false);

    }
}