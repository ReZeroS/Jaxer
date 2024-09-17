using UnityEngine;

public class AfterImageFx : MonoBehaviour
{
    
    private SpriteRenderer sr;
    private float loseColorRate;



    public void SetUpAfterImage(Sprite image, float rate)
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = image;
        loseColorRate = rate;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        float alpha = sr.color.a - loseColorRate * Time.deltaTime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
