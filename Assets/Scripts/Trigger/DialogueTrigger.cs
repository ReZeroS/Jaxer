using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    [FormerlySerializedAs("dialogue")] public SegmentDialogue segmentDialogue;

    private void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(segmentDialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }
    
}
