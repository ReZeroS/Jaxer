using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[Serializable]
public class SegmentDialogue
{
    public List<DialogueLine> dialogueLines = new();
}

public class DialogueTrigger : MonoBehaviour
{
    public SegmentDialogue segmentDialogue;

    private bool hasTriggered;
    
    
    private void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(segmentDialogue);
    }
    
    private void Update()
    {
        if (hasTriggered && InputManager.instance.padUp.justPressed)
        {
            TriggerDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            hasTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            hasTriggered = false;
        }
    }
}
