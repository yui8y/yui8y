using UnityEngine;
using TMPro;

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
        UpdateUI();
    }

    void UpdateUI()
    {
        money_txt.text = "総資産 " + GameData.money + "円";
    }

}