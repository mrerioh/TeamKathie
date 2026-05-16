using DG.Tweening;
using UnityEngine;

public class DoorAnimHandler : MonoBehaviour
{
    public void OpenDoor()
    {
        this.GetComponentInParent<Transform>().DORotate(new Vector3(0, 90, 0), 1);
    }

    public void CloseDoor()
    {
        this.GetComponentInParent<Transform>().DORotate(new Vector3(0, 0, 0), 1);
    }
}
