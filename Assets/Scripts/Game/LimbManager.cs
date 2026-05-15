using FMOD.Studio;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LimbManager : MonoBehaviour
{
    public static LimbManager instance;

    public static bool HasLeftLimb;
    public static bool HasRightLimb;
    public static bool IsBalanced;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("Found more than one Limb manager in the scene");
    }
    
    public static void UpdateLeftLimb(bool newState)
    {
        HasLeftLimb = newState;
        UpdateBalancedState();
    }

    public static void UpdateRightLimb(bool newState)
    {
        HasRightLimb = newState;
        UpdateBalancedState();
    }

    public static void UpdateBalancedState()
    {
        IsBalanced = HasLeftLimb && HasRightLimb;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
