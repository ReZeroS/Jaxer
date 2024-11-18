using System;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public event Action<Dialogue> OnDialogueUpdated;

    public DialogueLoader loader;
    private Dialogue currentDialogue;

    public Dialogue CurrentDialogue => currentDialogue;

    void Start()
    {
        loader.LoadDialogues();
        StartDialogue("start");
    }

    public void StartDialogue(string id)
    {
        currentDialogue = loader.GetDialogueById(id);
        OnDialogueUpdated?.Invoke(currentDialogue); // 通知UI层更新
    }

    public void ChooseOption(Choice choice)
    {
        if (!string.IsNullOrEmpty(choice.eventId))
        {
            // 触发对应事件
            HandleEvent(choice.eventId);
        }

        StartDialogue(choice.nextId);
    }

    private void HandleEvent(string eventId)
    {
        Debug.Log($"Event triggered: {eventId}");
        // 这里可以实现具体的业务逻辑，比如播放动画或给予奖励
    }
}