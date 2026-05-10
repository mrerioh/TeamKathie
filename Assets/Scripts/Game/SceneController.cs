using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] private CurtainAnimationHandler curtainAnimationHandler;
    [SerializeField] private CinemachineCamera GameplayCam;
    [SerializeField] private CinemachineCamera StageCamera;

    private void Awake()
    {
        if( instance == null )
        {
            instance = this;
            DontDestroyOnLoad( gameObject );
        }
        else
        {
            Destroy( gameObject );
        }

    }
    public void NextLevel()
    {
        GameplayCam.gameObject.SetActive( false );
        AsyncOperation op;
        curtainAnimationHandler.CloseCurtain();
        DOVirtual.DelayedCall(2f,
            () => {
                op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
                op.completed += OnSceneLoaded;
            });
        
        
    }

    private void OnSceneLoaded(AsyncOperation op)
    {
        curtainAnimationHandler.OpenCurtain();
        DOVirtual.DelayedCall(2f,
            () => {  GameObject.Find("GameplayCamera").SetActive(true); });
    }

    public void LoadScene( string SceneName )
    {
        SceneManager.LoadSceneAsync( SceneName );
    }
}
