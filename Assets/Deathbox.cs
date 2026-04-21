using UnityEngine;

public class Deathbox : MonoBehaviour
{
    public GameObject player;
    public Transform  respawnPoint;

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if( collision.CompareTag( "Player" ) )
            collision.transform.position = respawnPoint.position;
    }
}
