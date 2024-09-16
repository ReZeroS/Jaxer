using UnityEngine;

public class AreaSound : MonoBehaviour
{

    [SerializeField] private int areaSoundIndex;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            AudioManager.instance.FadeInSfxTime(areaSoundIndex, 1.5f);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            AudioManager.instance.FadeOutSfxTime(areaSoundIndex, 2f);
        }
    }
    
    
}
