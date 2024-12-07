using System;
using ReZeros.Jaxer.Core.Combat.Projectile;
using UnityEngine;

namespace ReZeros.Jaxer.Core.Combat
{
    [Serializable]
    public class Weapon
    {
        public Transform weaponTransform;
        public AbstractProjectile projectilePrefab;
        public float horizontalForce = 5.0f;
        public float verticalForce = 4.0f;
    }
}
