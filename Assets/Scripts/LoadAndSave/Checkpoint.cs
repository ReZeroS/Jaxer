using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
   
    private Animator animator;
    public string checkpointId;
    public bool isActivated;
    private static readonly int Active = Animator.StringToHash("active");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    [ContextMenu("Generate Id")]
    private void GenerateId()
    {
        checkpointId = Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            ActivatedCheckpoint();
        }
    }

    public void ActivatedCheckpoint()
    {
        if (!isActivated)
        {
            AudioManager.instance.PlaySFX(5, transform);
        }
        isActivated = true;
        animator?.SetBool(Active, true);
    }
}
