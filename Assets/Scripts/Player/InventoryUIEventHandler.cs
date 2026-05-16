using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIEventHandler : MonoBehaviour
{
    [SerializeField] Image StretchyArmUI;
    [SerializeField] Image PunchyArmUI;

    private void OnEnable()
    {
        InventoryManager.OnLeftLimbSelected += SelectLeftLimb;
        InventoryManager.OnRightLimbSelected+= SelectRightLimb;

        InventoryManager.OnLeftLimbAdded += AddLeftLimb;
        InventoryManager.OnRightLimbAdded += AddRightLimb;

        InventoryManager.OnLeftLimbRemoved += RemoveLeftLimb;
        InventoryManager.OnRightLimbRemoved += RemoveRightLimb;
    }

    private void OnDisable()
    {
        InventoryManager.OnLeftLimbSelected -= SelectLeftLimb;
        InventoryManager.OnRightLimbSelected -= SelectRightLimb;

        InventoryManager.OnLeftLimbAdded -= AddLeftLimb;
        InventoryManager.OnRightLimbAdded -= AddRightLimb;

        InventoryManager.OnLeftLimbRemoved -= RemoveLeftLimb;
        InventoryManager.OnRightLimbRemoved -= RemoveRightLimb;
    }

    private void RemoveLeftLimb()
    {
        StretchyArmUI.GetComponent<Image>().DOColor(Color.black, 0.2f);
        SelectRightLimb();
    }
    private void RemoveRightLimb()
    {
        PunchyArmUI.GetComponent<Image>().DOColor(Color.black, 0.2f);
        SelectLeftLimb();
    }
    private void AddLeftLimb()
    {
        StretchyArmUI.GetComponent<Image>().DOColor(Color.white, 0.2f);
        SelectLeftLimb();
    }
    private void AddRightLimb()
    {
        PunchyArmUI.GetComponent<Image>().DOColor(Color.white, 0.2f);
        SelectRightLimb();
    }

    private void SelectLeftLimb()
    {
        StretchyArmUI.GetComponent<RectTransform>().DOScale(4f, 0.2f);
        PunchyArmUI.GetComponent<RectTransform>().DOScale(3.5f, 0.2f);
    }

    private void SelectRightLimb()
    {
        StretchyArmUI.GetComponent<RectTransform>().DOScale(3.5f, 0.2f);
        PunchyArmUI.GetComponent<RectTransform>().DOScale(4f, 0.2f);
    }
}
