using UnityEngine;

public class UIFadeScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly int FadeOutTrigger = Animator.StringToHash("fadeOut");
    private static readonly int FadeInTrigger = Animator.StringToHash("fadeIn");


    public void FadeOut()
    {
        animator.SetTrigger(FadeOutTrigger);
    }

    public void FadeIn()
    {
        animator.SetTrigger(FadeInTrigger);
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
