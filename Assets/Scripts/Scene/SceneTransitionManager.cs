using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; } // 单例模式

    // 初始关卡
    [SerializeField] private string startSceneName; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (IsSceneLoaded(startSceneName))
        {
            Debug.Log("Start scene has loaded " + startSceneName);
            return;
        }
        StartCoroutine(loadSceneAtActive(startSceneName));
    }

    
    private void OnEnable()
    {
        EventHandleManager.TransitionEvent += OnTransitionEvent;
    }

    private void OnDisable()
    {
        EventHandleManager.TransitionEvent -= OnTransitionEvent; 
    }

    private void OnTransitionEvent(string sceneToGo, Vector3 teleportPos)
    {
        StartCoroutine(Transition(sceneToGo, teleportPos));
    }

    private IEnumerator loadSceneAtActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        EventHandleManager.CallAfterSceneLoadedEvent();
    }


    private IEnumerator Transition(string sceneName, Vector3 position)
    {
        EventHandleManager.CallBeforeSceneUnLoadEvent();
            
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        yield return loadSceneAtActive(sceneName); 
            
        EventHandleManager.CallMovePosition(position);

        EventHandleManager.CallAfterSceneLoadedEvent();
 
    }
   
    
    private bool IsSceneLoaded(string name)
    {
        Scene scene = SceneManager.GetSceneByName(name);
        return scene.isLoaded;
    }
    
    
    
}
