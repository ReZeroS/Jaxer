using UnityEngine;

public class UITooltip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public virtual void AdjustPosition(Vector2 pointerPosition)
    {

        var halfScreenWidth = Screen.width / 2;
        float xOffset, yOffset;
        if (pointerPosition.x > halfScreenWidth)
        {
            xOffset = -150;
        }
        else
        {
            xOffset = 150;
        }

        var halfScreenHeight = Screen.height /2;
        if (pointerPosition.y > halfScreenHeight)
        {
            yOffset = -150;
        }
        else
        {
            yOffset = 150;
        }

        transform.position = new Vector2(pointerPosition.x + xOffset, pointerPosition.y + yOffset);

    }
}
