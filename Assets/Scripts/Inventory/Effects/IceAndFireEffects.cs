using UnityEngine;

[CreateAssetMenu(fileName = "ice and fire effect", menuName = "Data/Item effect/IceAndFire")]
public class IceAndFireEffects : ItemEffects
{
    [SerializeField] private GameObject iceFirePrefab;
    [SerializeField] private float moveVelocity;

    public override void ExecuteEffect(Transform respawnPosition)
    {
        Player instancePlayer = PlayerManager.instance.player;
        Transform playerTransform = instancePlayer.transform;
        if (instancePlayer.primaryAttackState.comboCounter == 2)
        {
            GameObject iceAndFireCreated = Instantiate(iceFirePrefab, respawnPosition.position, playerTransform.rotation);
            iceAndFireCreated.GetComponent<Rigidbody2D>().velocity = new Vector2(moveVelocity * instancePlayer.facingDir, 0);
            Destroy(iceAndFireCreated, 10f);
        }
        
    }
}
