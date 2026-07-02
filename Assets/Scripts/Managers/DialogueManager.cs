using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;

    [Header("Type Writer")]
    public float typingSpeed = 0.05f;

    // 現在表示中のノード
    private DialogueNode currentNode;

    // タイプライター用
    private string fullText;
    private bool isTyping = false;

    // 会話中か
    private bool isTalking = false;
    public bool IsTalking => isTalking;

    // 会話終了イベント
    public System.Action onEndTalk;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (!isTalking) return;

        bool advanceInput =
            Keyboard.current.eKey.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame;

        if (!advanceInput) return;

        // タイプ中なら全文表示
        if (isTyping)
        {
            StopAllCoroutines();
            messageText.text = fullText;
            isTyping = false;
            return;
        }

        NextLine();
    }

    /// <summary>
    /// 会話開始
    /// </summary>
    public void StartTalk(DialogueNode startNode)
    {
        if (startNode == null)
        {
            Debug.LogError("開始ノードがありません！");
            return;
        }

        currentNode = startNode;
        isTalking = true;

        dialoguePanel.SetActive(true);

        ShowLine();
    }

    /// <summary>
    /// 現在のノード表示
    /// </summary>
    void ShowLine()
    {
        if (currentNode == null)
        {
            EndTalk();
            return;
        }

        nameText.text = currentNode.speaker;
        fullText = currentNode.text;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(fullText));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        messageText.text = "";

        foreach (char c in sentence)
        {
            messageText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    /// <summary>
    /// 次のノードへ
    /// </summary>
    void NextLine()
    {
        if (string.IsNullOrEmpty(currentNode.nextNodeId) ||
            currentNode.nextNodeId == "null")
        {
            EndTalk();
            return;
        }

        DialogueNode nextNode = CSVLoader.GetNode(currentNode.nextNodeId);

        if (nextNode == null)
        {
            Debug.LogWarning($"NodeID [{currentNode.nextNodeId}] が見つかりません。");
            EndTalk();
            return;
        }

        currentNode = nextNode;
        ShowLine();
    }

    /// <summary>
    /// 会話終了
    /// </summary>
    void EndTalk()
    {
        StopAllCoroutines();

        isTyping = false;
        isTalking = false;

        dialoguePanel.SetActive(false);

        onEndTalk?.Invoke();

        Debug.Log("会話終了");
    }
}