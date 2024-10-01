using System.Collections;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public float minSize = 5f;
    public float maxSize = 8f;
    public float moveSpeed = 5f;
    public float maxMoveDistance = 20f;
    public float floatSpeed = 2f;
    public float lifetime = 5f;
    public string[] canContainerTags = { "Player" }; // 可以容纳实体的标签
    public LayerMask whatIsGround;
    private Entity containedEntity;
    private Vector3 initialPosition;
    private bool isPopped;
    private bool isCaptured;
    public float rotationSpeed = 180f; // 旋转速度，度/秒
    public float rotationAmount = 30f; // 最大旋转角度

    private void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(MoveAndFloat());
    }


    public void SetupBubble(float bubbleSpeed)
    {
        moveSpeed = bubbleSpeed;
    }

    private IEnumerator MoveAndFloat()
    {
        // Ensure transform is not null
        if (transform == null)
        {
            Debug.LogError("Transform is null.");
            yield break;
        }

        // Define constants for better readability
        const float VerticalOffsetMultiplier = 0.05f;
        const float RotationLerpMultiplier = 0.5f;
        const float InitialRotationOffset = 0f;
        float maxFloatHeight = Screen.height / 2;
        // Forward movement phase
        while (Vector3.Distance(transform.position, initialPosition) < maxMoveDistance && !containedEntity && !isPopped)
        {
            transform.Translate(transform.right * moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Floating phase
        float elapsedTime = 0f;
        float initialRotation = transform.rotation.eulerAngles.z;

        while (elapsedTime < lifetime && !isPopped)
        {
            elapsedTime += Time.deltaTime;

            // Calculate vertical displacement
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * VerticalOffsetMultiplier;
            Vector3 newPosition = transform.position + new Vector3(0, floatSpeed * Time.deltaTime + yOffset, 0);

            // Check maximum height
            if (newPosition.y > maxFloatHeight)
            {
                // Adjust direction downward or stop moving upward
                newPosition = new Vector3(newPosition.x, maxFloatHeight, newPosition.z);
            }

            // Check collision with ground layer
            RaycastHit2D hit = Physics2D.Raycast(newPosition, Vector2.down, 0.1f, whatIsGround);
            if (hit.collider != null)
            {
                // Adjust direction away from the ground
                newPosition += new Vector3(0, 0.1f, 0);
            }

            transform.position = newPosition;

            // Calculate rotation angle
            float rotationFactor =
                Mathf.Sin(Time.time * floatSpeed * 2) * RotationLerpMultiplier + InitialRotationOffset;
            float targetRotation = initialRotation + Mathf.Lerp(-rotationAmount, rotationAmount, rotationFactor);

            // Apply rotation
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

    private void TryCapturingEntity(Entity entity)
    {
        if (isCaptured)
        {
            return;
        }

        isCaptured = true;

        float entitySize = entity.GetComponent<Collider2D>().bounds.size.magnitude;
        if (entitySize >= minSize && entitySize <= maxSize)
        {
            float newSize = Mathf.Max(entitySize, minSize);
            transform.localScale = Vector3.one * newSize;
        }

        containedEntity = entity;
        entity.transform.SetParent(transform);
        entity.gameObject.SetActive(false);
    }

    private void Pop()
    {
        if (isPopped) return; // 防止多次调用
        isPopped = true;
        if (containedEntity)
        {
            containedEntity.transform.SetParent(null);
            containedEntity.gameObject.SetActive(true);
            containedEntity.transform.rotation = Quaternion.identity;
            // Debug.Log("bubble popped entity damage");
            containedEntity.GetComponent<CharacterStat>().TakeDamage(1);
        }

        // 播放破裂动画和音效
        PlayPopEffect();
        // 启动协程以销毁气泡
        StartCoroutine(DestroyBubbleAfterPop());
    }

    private IEnumerator DestroyBubbleAfterPop()
    {
        // 等待一帧，确保父子关系已完全解除
        yield return null;

        // 销毁气泡对象
        Destroy(gameObject, 0.5f);
    }

    private void PlayPopEffect()
    {
        // TODO: 播放破裂动画和音效
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string canContainerTag in canContainerTags)
        {
            if (canContainerTag.Equals(other.tag))
            {
                TryCapturingEntity(other.GetComponent<Entity>());
                return;
            }
        }
    }
}