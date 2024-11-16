using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

	public Image characterIcon;
	public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private RectTransform dialogBox;
    
    [SerializeField] private float targetYPosition = -320f;
    
    private Queue<DialogueLine> lines;
    
    private bool isTyping = false;
    
	public bool isDialogueActive = false;

	public float typingSpeed = 0.1f;

	private List<string> currentLines = new List<string>();

	private string currentTypingLine = "";
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        dialogBox = GetComponent<RectTransform>();
    }


    private void Start()
    {		
	    lines = new Queue<DialogueLine>();
	    // 将对话框初始位置设置在 Canvas 下方
	    dialogBox.anchoredPosition = new Vector2(dialogBox.anchoredPosition.x, -Screen.height * 2);
    }

    public void StartDialogue(SegmentDialogue segmentDialogue)
	{
		isDialogueActive = true;
		InputManager.instance.SwitchActionMap(InputManager.InputMapType.UI);
		currentLines.Clear(); // 清空之前的行
		// 从下方弹出到指定位置
		dialogBox.DOAnchorPosY(targetYPosition, 0.5f).SetEase(Ease.OutBounce);
		
		lines.Clear();

		foreach (DialogueLine dialogueLine in segmentDialogue.dialogueLines)
		{
			lines.Enqueue(dialogueLine);
		}

		DisplayNextDialogueLine();
		
	}

	public void DisplayNextDialogueLine()
	{
		if (lines.Count == 0 && !isTyping)
		{
			EndDialogue();
			return;
		}

		if (isTyping)
		{
			StopAllCoroutines(); // 停止当前的协程
			dialogueArea.text = currentTypingLine; // 直接显示完整文本
			isTyping = false; // 设置为不正在打字
		}
		else
		{
			DialogueLine currentLine = lines.Dequeue();

			characterIcon.sprite = currentLine.character.icon;
			characterName.text = currentLine.character.name;

			StopAllCoroutines();
			StartCoroutine(TypeSentence(currentLine));
		}
		
	}
	

	IEnumerator TypeSentence(DialogueLine dialogueLine)
	{
		isTyping = true;
		currentTypingLine = dialogueLine.line;
		dialogueArea.text = "";
		foreach (char letter in dialogueLine.line)
		{
			dialogueArea.text += letter;
			yield return new WaitForSeconds(typingSpeed);
		}
		isTyping = false; // 打字完成
	}



	private void Update()
	{
		bool confirmJustPressed = InputManager.instance.confirm.justPressed;
		if (isDialogueActive && confirmJustPressed) // 可以替换为其他按键
		{
			DisplayNextDialogueLine();
		}
	}

	void EndDialogue()
	{
		Debug.Log("end dialog ");
		isDialogueActive = false;
		dialogBox.DOAnchorPosY(-Screen.height * 2, 0.5f).SetEase(Ease.InCubic)
			.OnComplete(() => 
			{
				// 动画完成后启用输入
				InputManager.instance.SwitchActionMap(InputManager.InputMapType.GamePlay);
			});
	}
}
