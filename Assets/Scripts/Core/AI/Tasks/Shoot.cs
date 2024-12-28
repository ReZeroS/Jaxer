using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using ReZeros.Jaxer.Core.Combat;
using ReZeros.Jaxer.Manager;
using UnityEngine;

namespace ReZeros.Jaxer.Core.AI.Tasks
{
    public class Shoot : EnemyAction
    {
        public List<Weapon> weapons;
        public bool shakeCamera;

        public override TaskStatus OnUpdate()
        {
            foreach (var weapon in weapons)
            {
                var projectile = Object.Instantiate(weapon.projectilePrefab, weapon.weaponTransform.position,
                    Quaternion.identity);
                projectile.Shooter = gameObject;

                var force = new Vector2(weapon.horizontalForce * transform.localScale.x, weapon.verticalForce);
                Debug.Log("force for " + force);
                projectile.SetForce(force);

                if (shakeCamera)
                    CameraManager.Instance.ShakeCamera(0.5f);
            }

            return TaskStatus.Success;
        }
    }
}