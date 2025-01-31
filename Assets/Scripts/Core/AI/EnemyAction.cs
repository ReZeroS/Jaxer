using BehaviorDesigner.Runtime.Tasks;
using ReZeros.Jaxer.Core.Combat;
using ReZeros.Jaxer.Manager;
using ReZeros.Jaxer.PlayerBase;
using UnityEngine;

namespace ReZeros.Jaxer.Core.AI
{
    public class EnemyAction : Action
    {
        protected Rigidbody2D body;
        protected Animator animator;
        protected Destructable destructable;
        protected MainPlayer player;
        
        public override void OnAwake()
        {
            body = GetComponent<Rigidbody2D>();
            player = PlayerManager.instance.Player;
            destructable = GetComponent<Destructable>();
            animator = gameObject.GetComponentInChildren<Animator>();
        }
    }
}