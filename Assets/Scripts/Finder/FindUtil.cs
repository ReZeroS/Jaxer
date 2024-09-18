using UnityEngine;

public class FindUtil 

{
    
    public static Transform FindClosestEnemyWithoutSelf(Transform checkTransform, float radius)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(checkTransform.position, radius);
        float closeDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() && Vector2.Distance(hit.transform.position, checkTransform.position) > 1)
            {
                float curDistance = Vector2.Distance(hit.transform.position, checkTransform.position);
                if (curDistance < closeDistance)
                {
                    closeDistance = curDistance;
                    closestEnemy = hit.transform;
                }
            }
        }
        
        if (closestEnemy == null)
        { 
            closestEnemy = checkTransform;
        }
        
        return closestEnemy;
    }
}
