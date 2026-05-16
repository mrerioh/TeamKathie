using FMOD.Studio;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public static bool HasLeftLimb;
    public static bool HasRightLimb;
    public static bool IsBalanced;

    public static event Action OnLeftLimbAdded;
    public static event Action OnRightLimbAdded;
    public static event Action OnLeftLimbRemoved;
    public static event Action OnRightLimbRemoved;

    public static event Action OnLeftLimbSelected;
    public static event Action OnRightLimbSelected;

    private static GameObject LeftArm;
    private static GameObject RightArm;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("Found more than one Limb manager in the scene");
    }

    private void Start()
    {

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

    public static void DropLeftLimb()
    {
        if (!HasLeftLimb)
            return;
        OnLeftLimbRemoved?.Invoke();
        UpdateLeftLimb(false);
    }

    public static void DropRightLimb()
    {
        if (!HasRightLimb)
            return;
        OnRightLimbRemoved?.Invoke();
        UpdateRightLimb(false);
    }
}
