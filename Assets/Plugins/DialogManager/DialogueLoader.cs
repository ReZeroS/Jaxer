using System;
using UnityEngine;

[System.Serializable]
public class Dialogue {
    public string id;
    public string text;
    public Choice[] choices;
}

[System.Serializable]
public class Choice {
    public string text;
    public string nextId;
    public string eventId;
}

[System.Serializable]
public class DialogueContainer {
    public Dialogue[] dialogues;
}


public class DialogueLoader : MonoBehaviour {
    public TextAsset dialogueJSON;

    private DialogueContainer dialogues;

    public void LoadDialogues() {
        dialogues = JsonUtility.FromJson<DialogueContainer>(dialogueJSON.text);
    }

    public Dialogue GetDialogueById(string id) {
        return Array.Find(dialogues.dialogues, d => d.id == id);
    }
}