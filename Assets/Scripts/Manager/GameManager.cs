using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{

    public static GameManager instance;
    [SerializeField] private Checkpoint[] checkpoints;
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

    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>(true);
    }
    
    
    

    
    
    
    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


    public void LoadData(GameData gameData)
    {
        var gameDataCheckpoint = gameData.checkpoint;
        foreach (var checkpoint in gameDataCheckpoint)
        {
            foreach (var cp in checkpoints)
            {
                if (cp.checkpointId == checkpoint.Key)
                {
                    if (checkpoint.Value)
                    {
                        cp.ActivatedCheckpoint();
                    }
                }
            }
        }
        
        foreach (var checkpoint in checkpoints)
        {
            if (gameData.closestCheckpointId == checkpoint.checkpointId)
            {
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
                break;
            }
        }
        
    }
    //
    // private IEnumerator SetPlayerPosition(Checkpoint checkpoint)
    // {
    //     Debug.Log("Setting Player Position" + checkpoint.checkpointId);
    //     yield return new WaitForSeconds(0.1f);
    // }

    public void SaveData(ref GameData gameData)
    {
        gameData.closestCheckpointId = FindClosestCheckpoint()?.checkpointId;
        gameData.checkpoint.Clear();
        foreach (var checkpoint in checkpoints)
        {
            gameData.checkpoint.Add(checkpoint.checkpointId, checkpoint.isActivated);
        }
    }



    private Checkpoint FindClosestCheckpoint()
    {
        var playerPosition = PlayerManager.instance.player.transform.position;
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distance = Vector2.Distance(playerPosition, checkpoint.transform.position);
            if (distance < closestDistance && checkpoint.isActivated)
            {
                closestDistance = distance;
                closestCheckpoint = checkpoint;
            }
        }
        return closestCheckpoint;
    }
    
    
    
}
