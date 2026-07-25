using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;

    public Camera uiCamera;

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
        if(instance == null)
{
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        dialoguePanel.SetActive(false);
        Debug.Log("Awake: DialoguePanel非表示にした");  // 追加
    }

    void Update()
    {
        if(!isTalking)
        {
            return;
        }

        bool KeywordClicked = Keyboard.current.enterKey.wasPressedThisFrame;

        if(KeywordClicked)
        {
            // キーワードがあって、nextNodeId_eventがあれば分岐
            if (currentNode.keywords.Count > 0 &&
                !string.IsNullOrEmpty(currentNode.keywords[0].word) &&
                currentNode.keywords[0].word != "null" &&
                !string.IsNullOrEmpty(currentNode.nextNodeId_event) &&
                currentNode.nextNodeId_event != "null")
            {
                Debug.Log("キーワード分岐！ → " + currentNode.nextNodeId_event);
                DialogueNode eventNode = CSVLoader.GetNode(currentNode.nextNodeId_event);                
                
                // 好感度変動処理
                if (currentNode.keywords.Count > 0 && 
                    currentNode.keywords[0].affinityChange != 0)
                {
                    GameData.npc_001_like += currentNode.keywords[0].affinityChange;
                    Debug.Log("好感度変動: " + currentNode.keywords[0].affinityChange + 
                            " 現在の好感度: " + GameData.npc_001_like);
                }

                currentNode = eventNode;
                ShowLine();
                return;
            }
        }

        // テキスト送り処理
        bool advanceInput =
            Keyboard.current.eKey.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame;
        if(!advanceInput)
        {
            return;
        }


        // タイプ中なら全文表示
        if(isTyping)
        {
            StopAllCoroutines();
            messageText.maxVisibleCharacters = int.MaxValue;  // 全文表示
            isTyping = false;
            return;
        }

        NextLine();
    }

    // 会話開始
    public void StartTalk(DialogueNode startNode)
    {
        if(startNode == null)
        {
            Debug.LogError("開始ノードがありません！");
            return;
        }
        //Debug.Log("StartTalk呼ばれた！");
        currentNode = startNode;
        isTalking = true;

        dialoguePanel.SetActive(true);

        ShowLine();
    }

    // 現在のノード表示
    void ShowLine()
    {
        if(currentNode == null)
        {
            EndTalk();
            return;
        }

        nameText.text = currentNode.speaker;

        // キーワードをオレンジ色にする
        string coloredText = ColorKeyword(currentNode.text, currentNode.keywords.Count > 0 
            ? currentNode.keywords[0].word 
            : "");
        
        fullText = coloredText;


        

        StopAllCoroutines();
        StartCoroutine(TypeSentence(fullText));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        messageText.text = "";

        // タグを除いた文字だけを1文字ずつ表示する
        messageText.text = sentence;              // 全文セット（タグ込み）
        messageText.ForceMeshUpdate();            // テキスト情報を強制更新
        messageText.maxVisibleCharacters = 0;    // 全部非表示

        int totalChars = messageText.textInfo.characterCount;  // タグを除いた文字数を取得

        for(int i = 0; i <= totalChars; i++)
        {
            messageText.maxVisibleCharacters = i;  // 1文字ずつ表示
            yield return new WaitForSeconds(typingSpeed);
        }

        messageText.maxVisibleCharacters = int.MaxValue;  // 全文表示
        isTyping = false;
    }

    //次のノードへ
    void NextLine()
    {
        if(string.IsNullOrEmpty(currentNode.nextNodeId) ||
            currentNode.nextNodeId == "null")
        {
            EndTalk();
            return;
        }

        DialogueNode nextNode = CSVLoader.GetNode(currentNode.nextNodeId);

        if(nextNode == null)
        {
            Debug.LogWarning($"NodeID [{currentNode.nextNodeId}] が見つかりません。");
            EndTalk();
            return;
        }

        currentNode = nextNode;
        ShowLine();
    }

    // 会話終了
    void EndTalk()
    {
        StopAllCoroutines();

        isTyping = false;
        isTalking = false;

        dialoguePanel.SetActive(false);

        onEndTalk?.Invoke();

        // node_cafe_endで始まるノードが終わったらシーン遷移
        if(currentNode.nodeId.StartsWith("node_cafe_end"))
        {
            GameData.isReturningFromMaking = true;
            GameData.money += 500;
            SceneTransition.instance.FadeToScene("cafe_main");
        }

        Debug.Log("会話終了");
    }

    // keywordWordをオレンジ色で表示する
    string ColorKeyword(string text, string keyword)
    {
        if(string.IsNullOrEmpty(keyword)) return text;  // キーワードなければそのまま
        return text.Replace(keyword, $"<color=#F0A050>{keyword}</color>");  // オレンジに
    }
}