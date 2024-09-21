using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float minSize = 5f;
    public float maxSize = 8f;
    public float moveSpeed = 5f;
    public float maxMoveDistance = 20f;
    public float floatSpeed = 2f;
    public float lifetime = 5f;
    public string[] popTriggers = { "Arrow", "Player", "Enemy" }; // 可以打破泡泡的对象标签

    private Enemy containedEnemy;
    private Vector3 initialPosition;
    private bool isFloating = false;
    private bool isPopped = false;
    public float rotationSpeed = 180f; // 旋转速度，度/秒
    public float rotationAmount = 30f; // 最大旋转角度
    private void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(MoveAndFloat());
    }

    private IEnumerator MoveAndFloat()
    {
        // 前进阶段
        while (Vector3.Distance(transform.position, initialPosition) < maxMoveDistance && !containedEnemy && !isPopped)
        {
            transform.Translate(transform.right * moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 开始上浮
        isFloating = true;

        // 上浮阶段
        float elapsedTime = 0f;
        float initialRotation = transform.rotation.eulerAngles.z;

        while (elapsedTime < lifetime && !isPopped)
        {
            elapsedTime += Time.deltaTime;

            // 计算垂直位移
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * 0.1f;
            transform.Translate(Vector3.up * (floatSpeed * Time.deltaTime + yOffset));

            // 计算旋转角度
            float rotationFactor = Mathf.Sin(Time.time * floatSpeed * 2) * 0.5f + 0.5f; // 使旋转与上下浮动同步
            float targetRotation = initialRotation + Mathf.Lerp(-rotationAmount, rotationAmount, rotationFactor);
            
            // 应用旋转
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(0, 0, targetRotation),
                rotationSpeed * Time.deltaTime
            );

            yield return null;
        }

        if (!isPopped)
        {
            Pop();
        }
    }
    public bool TryCapturingEnemy(Enemy enemy)
    {
        float enemySize = enemy.GetComponent<Collider2D>().bounds.size.magnitude;
        if (enemySize >= minSize && enemySize <= maxSize)
        {
            float newSize = Mathf.Max(enemySize, minSize);
            transform.localScale = Vector3.one * newSize;

            containedEnemy = enemy;
            enemy.transform.SetParent(transform);
            enemy.gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    public void Pop()
    {
        if (isPopped) return; // 防止多次调用

        isPopped = true;
        if (containedEnemy)
        {
            containedEnemy.transform.SetParent(null);
            containedEnemy.gameObject.SetActive(true);
            containedEnemy.GetComponent<EnemyStat>().TakeDamage(1);
        }
        // 播放破裂动画和音效
        PlayPopEffect();
        Destroy(gameObject, 0.5f); // 给特效留出播放时间
    }

    private void PlayPopEffect()
    {
        // 在这里实现泡泡破裂的视觉和音频效果
        Debug.Log("泡泡破裂！");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (System.Array.Exists(popTriggers, tag => other.CompareTag(tag)))
        {
            Pop();
        }
    }
}

