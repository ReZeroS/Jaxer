using ReZeros.Jaxer.PlayerBase;
using UnityEngine;

public class AreaSound : MonoBehaviour
{

    [SerializeField] private string areaSoundName;

    private string lastTrackName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<MainPlayer>())
        {
            lastTrackName = MusicManager.Instance.currentTrackName;
            MusicManager.Instance.PlayMusic(areaSoundName);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<MainPlayer>())
        {
            MusicManager.Instance.PlayMusic(lastTrackName);
        }
    }
    
    
}
