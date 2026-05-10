using System;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEditor;
using UnityEngine;

public class MovementLocker : MonoBehaviour
{
    [SerializeField] private Vector3 SpawnLocation;
    [SerializeField] private float MaxLeft;
    [SerializeField] private float MaxRight;
    [SerializeField] private float Normalized;
    [SerializeField] private GameObject LinkedMovement;
    [SerializeField] private float TransformedValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.SpawnLocation = transform.position;
        Normalized = 0f;
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Normalized = Mathf.InverseLerp(MaxLeft * -1f, MaxRight, this.transform.position.x - this.SpawnLocation.x);
        
        if (this.transform.position.x <= this.SpawnLocation.x - MaxLeft)
            this.transform.position = new Vector3(this.SpawnLocation.x - MaxLeft, this.transform.position.y, this.transform.position.z);
        else if (this.transform.position.x >= this.SpawnLocation.x + MaxRight)
            this.transform.position = new Vector3(this.SpawnLocation.x + MaxRight, this.transform.position.y, this.transform.position.z);
        TransformedValue = Normalized * 90f;
        LinkedMovement.gameObject.transform.eulerAngles = new Vector3(0, TransformedValue, 0);
    }
}
