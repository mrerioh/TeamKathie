using UnityEngine;

public class PunchArmEventHandler : MonoBehaviour
{
    private void OnEnable()
    {
        InventoryManager.OnLeftLimbRemoved += DetachMyself;
    }

    private void OnDisable()
    {
        InventoryManager.OnLeftLimbRemoved -= DetachMyself;
    }

    private void DetachMyself()
    {
        transform.parent = null;
    }
}
