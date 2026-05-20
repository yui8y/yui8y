using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialoguePanel;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;

    public TalkDataSO currentTalk;


    public float typingSpeed = 0.05f;

    bool isTyping = false;


    int currentLine = 0;
    bool isTalking = false;

    //文字テキスト
    System.Collections.IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        messageText.text = "";

        foreach(char c in sentence)
        {
            messageText.text += c;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(isTalking && Keyboard.current.eKey.wasPressedThisFrame){
            if(isTyping == true)
            {
                StopAllCoroutines();

                messageText.text = currentTalk.lines[currentLine].message;

                isTyping = false;
                return;
            }

            NextLine();
        }
    }

    public void StartTalk(TalkDataSO talkData)
    {
        currentTalk = talkData;

        currentLine = 0;
        isTalking = true;

        dialoguePanel.SetActive(true);

        ShowLine();
    }

    void ShowLine()
    {
        TalkLine line = currentTalk.lines[currentLine];

        nameText.text = line.speaker;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(line.message));
    }

    void NextLine()
    {
        currentLine++;

        if(currentLine >= currentTalk.lines.Count)
        {
            EndTalk();
            return;
        }

        ShowLine();
    }

    void EndTalk()
    {
        isTalking = false;

        dialoguePanel.SetActive(false);

        Debug.Log("会話終了！");
    }
}