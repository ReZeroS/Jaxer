using ReZeros.Jaxer.Manager;
using ReZeros.Jaxer.PlayerBase;
using UnityEngine;

[CreateAssetMenu(fileName = "ice and fire effect", menuName = "Data/Item effect/IceAndFire")]
public class IceAndFireEffects : ItemEffects
{
    [SerializeField] private GameObject iceFirePrefab;
    [SerializeField] private float moveVelocity;

    public override void ExecuteEffect(Transform respawnPosition)
    {
        MainPlayer instanceMainPlayer = PlayerManager.instance.Player;
        Transform playerTransform = instanceMainPlayer.transform;
        if (instanceMainPlayer.primaryAttackState.comboCounter == 2)
        {
            GameObject iceAndFireCreated = Instantiate(iceFirePrefab, respawnPosition.position, playerTransform.rotation);
            iceAndFireCreated.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(moveVelocity * instanceMainPlayer.facingDir, 0);
            Destroy(iceAndFireCreated, 10f);
        }
        
    }
}
