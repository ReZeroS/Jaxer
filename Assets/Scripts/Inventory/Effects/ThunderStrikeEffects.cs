using UnityEngine;

[CreateAssetMenu(fileName = "thunder strike effect", menuName = "Data/Item effect/ThunderStrike")]
public class ThunderStrikeEffects : ItemEffects
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform respawnPosition)
    {
        GameObject thunder = Instantiate(thunderStrikePrefab, respawnPosition.position, Quaternion.identity);
        Destroy(thunder, .5f);
    }
}
