using ReZeros.Jaxer.Manager;
using UnityEngine;
using Yarn.Unity;

namespace ReZeros.Jaxer.Trigger.Npc
{
    public class NpcDialogTrigger : MonoBehaviour
    {
        public string dialogueNode = "Start"; // 需要触发的 Yarn 节点
        public DialogueRunner dialogueRunner;
        private bool playerInRange = false;

        void Update()
        {
            // 玩家在范围内且按下E键
            if (playerInRange && InputManager.instance.south.justPressed)
            {
                if (dialogueRunner && !dialogueRunner.IsDialogueRunning)
                {
                    dialogueRunner.StartDialogue(dialogueNode);
                }
                else
                {
                    dialogueRunner.dialogueViews[0].UserRequestedViewAdvancement();
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                Debug.Log("玩家进入对话范围，按 E 开始对话");
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                Debug.Log("玩家离开对话范围");
            }
        }
    }
}