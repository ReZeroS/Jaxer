using System.Collections;
using UnityEngine;

public class BubbleDragon : MonoBehaviour
{
    public GameObject bubblePrefab;
    public float bubbleCheckRadius = 2f;

    public void ShootBubble()
    {
        GameObject bubbleObj = Instantiate(bubblePrefab, transform.position, transform.rotation);
        Bubble bubble = bubbleObj.GetComponent<Bubble>();

        StartCoroutine(CheckForEnemies(bubble));
    }

    private IEnumerator CheckForEnemies(Bubble bubble)
    {
        while (bubble != null && !bubble.TryCapturingEnemy(null))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(bubble.transform.position, bubbleCheckRadius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null && bubble.TryCapturingEnemy(enemy))
                    {
                        yield break;
                    }
                }
            }

            yield return null;
        }
    }
}