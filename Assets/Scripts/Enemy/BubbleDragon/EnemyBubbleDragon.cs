using System.Collections;
using UnityEngine;

public class EnemyBubbleDragon : Enemy
{
    public GameObject bubblePrefab;
    public float bubbleCheckRadius = 2f;
    
 
    #region States
    public BubbleDragonIdleState idleState;

    #endregion
    
    
    protected override void Awake()
    {
        base.Awake();
        idleState = new BubbleDragonIdleState(stateMachine,this, "Idle");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }





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
