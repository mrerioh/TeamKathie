using UnityEngine;

public class EndZoneEnterHandler : MonoBehaviour
{
    [SerializeField] private SceneController SC;
    private void Start()
    {
        SC = SC.GetComponent<SceneController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (other.gameObject.tag == "Player")
        {
            SC.NextLevel();
        }
    }
}
