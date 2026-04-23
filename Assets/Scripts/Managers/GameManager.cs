using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int money = 0;

    public TextMeshProUGUI money_txt;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
    }



    void UpdateUI()
    {
        money_txt.text = "総資産 " + money + "円";
    }
}