using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

    public static AudioManager  instance;
    private List<EventInstance> eventInstances;

    private EventInstance       ambienceEventInstance;

    private void Awake()
    {
        if( instance == null )
            instance = this;
        else
            Debug.LogError("Found more than one Audio manager in the scene");
        
        eventInstances = new List<EventInstance>();
    }

    private void InitializeAmbience( EventReference ambienceEventRef )
    {
        ambienceEventInstance = CreateEventInstance( ambienceEventRef );
        ambienceEventInstance.start();
    }

    private void Start()
    {
        //InitializeAmbience(FMODEvents.instance.Ambience);
    }

    public EventInstance CreateEventInstance( EventReference eventReference )
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance( eventReference );
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach ( EventInstance eventInstance in eventInstances )
        {
            eventInstance.stop( FMOD.Studio.STOP_MODE.IMMEDIATE );
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }

}
