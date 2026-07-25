using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{    
    public static GameManager instance;

    //お客さんが来る
    public Transform counterPoint;
    //お客さんが来たときにtrueにしてお客さん打ち止め
    public bool customerExists = false;
    
    //資金管理
    public TextMeshProUGUI money_txt;

    //ダイアログ管理
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;

    
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
    }
    

    void Start()
    {
        // フェードイン完了後の処理を登録
        SceneTransition.instance.onFadeInComplete += OnFadeInComplete;
        UpdateUI();
    }

    void OnFadeInComplete()
    {
        if (GameData.isReturningFromMaking)
        {
            StartThankYouDialogue();                  // お礼ダイアログ開始
        }
    }

    void StartThankYouDialogue()
    {
        DialogueNode node = CSVLoader.GetNode("node_thankyou");
        DialogueManager.instance.StartTalk(node);

        // ダイアログ終了後にお客さんを退場させる
        DialogueManager.instance.onEndTalk += OnThankYouEnd;
    }

    void OnThankYouEnd()
    {
        DialogueManager.instance.onEndTalk -= OnThankYouEnd;  // 登録解除
        ComeCustomer customer = FindAnyObjectByType<ComeCustomer>();
        if (customer != null)
        {
            StartCoroutine(customer.Leave());  // 退場開始
        }
    }

    void OnDisable()
    {
        SceneTransition.instance.onFadeInComplete -= OnFadeInComplete;
    }

    void UpdateUI()
    {
        money_txt.text = "売上 " + GameData.money + "円";
    }

}