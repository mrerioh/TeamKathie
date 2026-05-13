using DG.Tweening;
using UnityEngine;

public class PunchAnimationScript : MonoBehaviour
{
   public void OnHandleEvent()
    {
      this.gameObject.GetComponentInChildren<Transform>().transform.DOLocalMoveX(0.355f - 0.017f,1f);  
    }
}
