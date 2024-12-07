using UnityEngine;

namespace ReZeros.Jaxer.Core
{
    public class CoreComponent : MonoBehaviour
    {
        protected Core core;

        protected virtual void Awake()
        {
            core = transform.parent.GetComponent<Core>();

            if(core == null) { Debug.LogError("There is no Core on the parent"); }
            core.AddComponent(this);
        }

        public virtual void LogicUpdate() { }

    }
}