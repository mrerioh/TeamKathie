using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private void Awake()
    {
        if( instance == null )
        {
            instance = this;
            DontDestroyOnLoad( gameObject );
        }
        else
            Destroy( gameObject );
    }
}
