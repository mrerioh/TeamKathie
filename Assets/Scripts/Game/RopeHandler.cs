using Unity.VisualScripting;
using UnityEngine;

public class RopeHandler : MonoBehaviour
{
    [SerializeField] public bool ExceedsWeight;
    [SerializeField] public int NumOnRope;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NumOnRope = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
