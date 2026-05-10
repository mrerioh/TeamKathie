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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.SpawnLocation = transform.position;
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Normalized = (float) math.remap(MaxLeft, MaxRight, 0d, 1d, this.transform.position.x);
        if (this.transform.position.x < this.SpawnLocation.x - MaxLeft)
            this.transform.position = this.SpawnLocation - new Vector3(MaxLeft, 0, 0);
        else if (this.transform.position.x > this.SpawnLocation.x + MaxRight)
            this.transform.position = this.SpawnLocation + new Vector3(MaxRight, 0, 0);
    }
}
