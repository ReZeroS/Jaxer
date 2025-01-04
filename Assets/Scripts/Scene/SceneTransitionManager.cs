using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance { get; private set; } // 单例模式

    // 初始关卡
    [SerializeField] private string startSceneName; 

    public Slider progressBar;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    
    private void OnEnable()
    {
        EventHandleManager.TransitionEvent += OnTransitionEvent;
    }
    
    private void OnTransitionEvent(string sceneToGo, Vector3 teleportPos)
    {
        StartCoroutine(Transition(sceneToGo, teleportPos));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (IsSceneLoaded(startSceneName))
        {
            return;
        }
        StartCoroutine(LoadSceneAtActive(startSceneName));
    }

    

    private IEnumerator LoadSceneAtActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        EventHandleManager.CallAfterSceneLoadedEvent(sceneName);
    }


    private IEnumerator Transition(string sceneName, Vector3 position)
    {
        EventHandleManager.CallBeforeSceneUnLoadEvent();
            
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        yield return LoadSceneAtActive(sceneName); 
            
        EventHandleManager.CallMovePosition(position);

        EventHandleManager.CallAfterSceneLoadedEvent(sceneName);
 
    }
   
    
    private bool IsSceneLoaded(string name)
    {
        Scene scene = SceneManager.GetSceneByName(name);
        return scene.isLoaded;
    }
    
    

    private void OnDisable()
    {
        EventHandleManager.TransitionEvent -= OnTransitionEvent; 
    }
    
}
