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

    private DialogueNode currentNode;        // 今表示中のノード
    private string fullText;                 // タイプライター用の全文保持

    // 会話終了時に発火するイベント
    public System.Action onEndTalk;

    public float typingSpeed = 0.05f;

    bool isTyping = false;
    bool isTalking = false;
    public bool IsTalking
    {
        get { return isTalking; }
    }

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
        // E・スペース・クリックのどれかを判定
        bool isAdvanceInput =
            Keyboard.current.eKey.wasPressedThisFrame
            || Keyboard.current.spaceKey.wasPressedThisFrame
            || Mouse.current.leftButton.wasPressedThisFrame;

        if (isTalking && isAdvanceInput)
        {
            if (isTyping)
            {
                // タイピング中なら全文スキップ表示
                StopAllCoroutines();
                messageText.text = fullText;  // currentTalk→fullTextに変更
                isTyping = false;
                return;
            }

            NextLine();
        }
    }

    public void StartTalk(DialogueNode startNode)
    {
        currentNode = startNode;   // 最初のノードをセット
        isTalking = true;
        dialoguePanel.SetActive(true);
        ShowLine();
    }

    void ShowLine()
    {
        nameText.text = currentNode.speaker;  // 話者名をセット
        fullText = currentNode.text;          // 全文を保持しておく

        StopAllCoroutines();
        StartCoroutine(TypeSentence(fullText)); // タイプライター開始
    }

    void NextLine()
    {
        // nextNodeIdがnullか"null"なら会話終了
        if (string.IsNullOrEmpty(currentNode.nextNodeId) 
            || currentNode.nextNodeId == "null")
        {
            EndTalk();
            return;
        }

        // CSVLoaderから次のノードを取得して進む
        currentNode = CSVLoader.GetNode(currentNode.nextNodeId);
        Debug.Log("次のノード: " + currentNode?.nodeId); // 追加、nullなら"null"と表示される
        ShowLine();
    }

    void EndTalk()
    {
        isTalking = false;

        dialoguePanel.SetActive(false);

        onEndTalk?.Invoke();  // 登録されてる処理があれば呼ぶ（なければ何もしない）

        Debug.Log("会話終了！ isTalking = " + isTalking);
    }
}