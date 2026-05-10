using DG.Tweening;
using UnityEngine;

public class CurtainAnimationHandler : MonoBehaviour
{
    [SerializeField] private Transform LCurtain;
    [SerializeField] private Transform LCurtainOpend;
    [SerializeField] private Transform LCurtainClosed;
    [SerializeField] private Transform RCurtain;
    [SerializeField] private Transform RCurtainOpened;
    [SerializeField] private Transform RCurtainClosed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        
    }

    public void CloseCurtain()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(LCurtain.DOLocalMove(LCurtainClosed.position, 1.5f).SetEase(Ease.InQuad))
        .Join(RCurtain.DOLocalMove(RCurtainClosed.position, 1.5f).SetEase(Ease.InQuad))
        .Play();
    }
    public void OpenCurtain()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(LCurtain.DOLocalMove(LCurtainOpend.position, 1.5f).SetEase(Ease.InQuad))
        .Join(RCurtain.DOLocalMove(RCurtainOpened.position, 1.5f).SetEase(Ease.InQuad))
        .Play();
    }
}
