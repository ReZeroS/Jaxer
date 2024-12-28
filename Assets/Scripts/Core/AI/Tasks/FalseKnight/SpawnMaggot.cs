using BehaviorDesigner.Runtime.Tasks;
using ReZeros.Jaxer.Core.AI;
using ReZeros.Jaxer.Core.Combat;
using UnityEngine;

namespace Core.AI
{
    public class SpawnMaggot : EnemyAction
    {
        public GameObject maggotPrefab;
        public Transform maggotTransform;
        public GameObject hazardCollider;

        private Destructable maggot;

        public override void OnStart()
        {
            Debug.Log("SpawnMaggot on start");
            maggot = Object.Instantiate(maggotPrefab, maggotTransform).GetComponent<Destructable>();
            maggot.transform.localPosition = Vector3.zero;
            destructable.Invincible = true;
            // hazardCollider.SetActive(false);
        }

        public override TaskStatus OnUpdate()
        {
            if (maggot.CurrentHealth > 0) return TaskStatus.Running;

            destructable.Invincible = false;
            // hazardCollider.SetActive(true);
            return TaskStatus.Success;
        }
    }
}