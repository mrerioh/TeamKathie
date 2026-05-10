using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PullHandler : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private int _maxPull = 0;
    [SerializeField] private int _currentPull = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (_currentPull != _maxPull)
        {
            OnPulled();
        }

        if (_currentPull - 1 != -1)
        {
            OnPunched();
        }
    }

    private void OnPulled()
    {
        Debug.Log("Drawer Pulling");
        _gameObject
            .transform.DOMoveX(_gameObject.transform.position.x - 1f, 1f)
            .OnPlay(() => disableItem())
            .OnComplete(() =>
            {
                _currentPull += 1;
                enableItem();
            });
    }

    private void OnPunched()
    {
        Debug.Log("Drawer Pushed");
        _gameObject
            .transform.DOMoveX(_gameObject.transform.position.x - 1f, 1f)
            .OnPlay(() => disableItem())
            .OnComplete(() =>
            {
                _currentPull -= 1;
                enableItem();
            });
    }

    private void disableItem()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        
    }

    private void enableItem()
    {
        this.GetComponent<BoxCollider>().enabled = true;
        
    }
}
