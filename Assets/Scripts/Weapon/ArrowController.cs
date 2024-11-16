using UnityEngine;
using Random = UnityEngine.Random;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    
    [SerializeField] private bool canMove;

    [SerializeField] private bool flipped;


    private CharacterStat characterStat;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!canMove)
        {
            rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
        }
    }


    public void SetupArrow(float speed, CharacterStat charStat)
    {
        xVelocity = speed;
        characterStat = charStat;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            characterStat.DoDamage(other.GetComponent<CharacterStat>());
            StuckIntoCharacter(other);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckIntoCharacter(other);
        }
    }

    private void StuckIntoCharacter(Collider2D other)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<Collider2D>().enabled = false;
        canMove = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = other.transform;
        Destroy(gameObject, Random.Range(2, 5));
    }


    public void Flip()
    {
        // only flips once
        if (flipped)
        {
            return;
        }

        flipped = true;
        xVelocity *= -1;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }
}