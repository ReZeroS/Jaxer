using UnityEngine;

public class AreaSound : MonoBehaviour
{

    [SerializeField] private string areaSoundName;

    private string lastTrackName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            lastTrackName = MusicManager.Instance.currentTrackName;
            MusicManager.Instance.PlayMusic(areaSoundName);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            MusicManager.Instance.PlayMusic(lastTrackName);
        }
    }
    
    
}
