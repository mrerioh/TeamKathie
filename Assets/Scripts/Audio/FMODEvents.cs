using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;


public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance;
    [field: Header("Player SFX")]
    [SerializeField] public EventReference PlayerFootsteps;

    [field: Header("Ambience SFX")]
    [SerializeField] public EventReference Ambience;

    private void Awake()
    {
        if( instance == null )
            instance = this;
        else
            Debug.LogError("Found more than one FMODEvent instance in the scene");
    }
}
