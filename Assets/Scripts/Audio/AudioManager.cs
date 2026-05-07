using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    private void Awake()
    {
        if( instance == null )
            instance = this;
        else
            Debug.LogError("Found more than one Audio manager in the scene");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
