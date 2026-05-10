using DG.Tweening;
using UnityEngine;

public class DrawerPullHandler : MonoBehaviour
{
    private void PullOutOneLevel()
    {
        this.GetComponentInParent<GameObject>().transform.DOMoveX(-5, 2f);
    }
}
