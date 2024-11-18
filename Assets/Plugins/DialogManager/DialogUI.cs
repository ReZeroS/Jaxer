using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour {
    public Text dialogueText; // 对话显示文本
    public Transform choiceContainer; // 选项容器
    public GameObject choiceButtonPrefab; // 选项按钮预制件

    private DialogueSystem dialogueSystem;

    void Start() {
        dialogueSystem = FindFirstObjectByType<DialogueSystem>();
        dialogueSystem.OnDialogueUpdated += UpdateUI;

        UpdateUI(dialogueSystem.CurrentDialogue);
    }

    void UpdateUI(Dialogue currentDialogue) {
        if (currentDialogue == null) return;

        // 更新对话文本
        dialogueText.text = currentDialogue.text;

        // 清空现有选项按钮
        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject);

        // 动态生成选项按钮
        foreach (var choice in currentDialogue.choices) {
            var button = Instantiate(choiceButtonPrefab, choiceContainer).GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = choice.text;

            button.onClick.AddListener(() => {
                dialogueSystem.ChooseOption(choice);
            });
        }

        // 自动导航第一个按钮（新输入系统支持手柄输入）
        if (choiceContainer.childCount > 0) {
            var firstButton = choiceContainer.GetChild(0).GetComponent<Button>();
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        }
    }
}