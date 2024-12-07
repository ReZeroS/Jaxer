using UnityEngine;

namespace ReZeros.Jaxer.Core.Combat.Projectile
{
    public class Projectile : AbstractProjectile {
        public override void SetForce(Vector2 forc)
        {
            force = forc;
            GetComponent<Rigidbody2D>().AddForce(forc, ForceMode2D.Impulse);
        }
    }
}