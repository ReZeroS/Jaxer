using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private float respawnDelay = 3f;
    [SerializeField] private Rigidbody2D rb;

    private Vector3 startPosition;
    private bool falling = false;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (falling)
            return;

        if (collision.transform.CompareTag("Player"))
        {
            falling = true;
            Invoke(nameof(StartFall), fallDelay);
            Invoke(nameof(DisablePlatform), fallDelay + destroyDelay);
            Invoke(nameof(ResetPlatform), fallDelay + destroyDelay + respawnDelay);
        }
    }

    private void StartFall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void DisablePlatform()
    {
        gameObject.SetActive(false);
    }

    private void ResetPlatform()
    {
        transform.position = startPosition;
        falling = false;
        gameObject.SetActive(true);
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
    }
}
