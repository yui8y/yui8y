using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;  // Listを使うために必要


public class CoffeeMakingManager : MonoBehaviour
{
    public static CoffeeMakingManager instance;  // シングルトン

    public CoffeeSelection currentSelection = new CoffeeSelection();  // 今の選択状態


    public GameObject tooltipPanel;       // 共通のツールチップパネル
    public TextMeshProUGUI tooltipText;   // 共通のツールチップテキスト

    public TextMeshProUGUI Tooltip_title;  // 共通のツールチップタイトル
    public Transform tooltipTransform;

    public Canvas parentCanvas;           // 親のCanvas

    bool isHovering = false;              // ホバー中フラグ

    public List<TasteButton> allButtons;  // 全豆・香りボタンをInspectorで登録

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isHovering)
        {
            // マウススクリーン座標を取得
            Vector2 mousePos = Mouse.current.position.ReadValue();

            // ワールド座標に変換
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(mousePos.x, mousePos.y, 10f)
            );

            tooltipTransform.position = mouseWorldPos + new Vector3(0.6f, -0.7f, 0f);

            
        }
    }

    // Tooltipを表示
    public void ShowTooltip(string text, string Id)
    {
        tooltipText.text = text;
        Tooltip_title.text = Id;
        tooltipPanel.SetActive(true);
        isHovering = true;
    }

    // Tooltipを隠す
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
        isHovering = false;
    }

    // 味が選ばれたときに呼ぶ
    public void SelectTaste(string tasteId)
    {
        currentSelection.selectedTaste = tasteId;   // 選択した味を記録
        
        Debug.Log("味選択: " + tasteId);
        
    }

    // 香りが選ばれたときに呼ぶ
    public void SelectFlavor(string flavorId)
    {
        currentSelection.selectedFlavor = flavorId;   // 選択した香りを記録
        Debug.Log("香り選択: " + flavorId);
    }

    // 温度が選ばれたときに呼ぶ
    public void SelectTemp(bool isHot)
    {
        currentSelection.isHot = isHot;             // ホット/アイスを記録
        Debug.Log("温度選択: " + (isHot ? "ホット" : "アイス"));
    }

    // コーヒーを作る
    public void MakeCoffee()
    {
        // 必須項目が選ばれているか確認
        if (string.IsNullOrEmpty(currentSelection.selectedTaste))
        {
            Debug.Log("味を選んでね！");
            return;
        }

        if (string.IsNullOrEmpty(currentSelection.selectedFlavor))
        {
            Debug.Log("香りを選んでね！");
            return;
        }

        Debug.Log("味：" + currentSelection.selectedTaste);
        Debug.Log("香り：" + currentSelection.selectedFlavor);
        Debug.Log("温度：" + (currentSelection.isHot ? "ホット" : "アイス"));

    }

    public void ResetSelection()
    {
        // 全ボタンの選択状態をリセット
        foreach (TasteButton btn in allButtons)
        {
            btn.SetSelected(false);         // 選択枠を非表示
        }

        currentSelection.selectedTaste = "";   // 味の選択をリセット
        currentSelection.selectedFlavor = "";   // 香りの選択をリセット
        currentSelection.isHot = true;         // 温度をデフォルトに戻す
    }

    // 提供ボタンが押されたときに呼ぶ
    public void OnServeButtonClicked()
    {
        //デバッグ用
        if (MakingManager.DebugMode == true)
        {
            GameData.currentCharId = "npc_01";
        }

        string charId = GameData.currentCharId;

        // お客さんのデータを取得
        CustomerData customerData = CustomerDataLoader.GetCustomerData(charId);
        if (customerData == null) return;

        // 判定実行
        string resultNodeId = JudgeResult(customerData);

        // 結果の会話を開始
        DialogueNode node = CSVLoader.GetNode(resultNodeId);
        DialogueManager.instance.StartTalk(node);
    }

    // 一致判定
    string JudgeResult(CustomerData customerData)
    {
        int matchCount = 0;    // 好みと一致した数
        bool isCritical = false;  // 苦手に当たったかどうか

        // 選択した味のスコアを確認
        if (!string.IsNullOrEmpty(currentSelection.selectedTaste))
        {
            // 選択した味がお客さんのデータにあるか確認
            if (customerData.tasteScores.TryGetValue(currentSelection.selectedTaste, out int tasteScore))
            {
                if (tasteScore >= 60)       matchCount++;   // 好み（高スコア）なら一致
                else if (tasteScore <= 20)  isCritical = true;  // 苦手（低スコア）なら致命的
            }
        }

        // 選択した香りのスコアを確認
        if (!string.IsNullOrEmpty(currentSelection.selectedFlavor))
        {
            if (customerData.flavorScores.TryGetValue(currentSelection.selectedFlavor, out int flavorScore))
            {
                if (flavorScore >= 60)       matchCount++;
                else if (flavorScore <= 20)  isCritical = true;
            }
        }

        Debug.Log($"一致数: {matchCount} 致命的: {isCritical}");

        // 結果判定
        if (isCritical)             return customerData.criticalFailNodeId;  // 致命的失敗
        if (matchCount >= 2)        return customerData.greatSuccessNodeId;  // 決定的成功
        if (matchCount == 1)        return customerData.successNodeId;       // 通常成功
        return customerData.failNodeId;                                      // 通常失敗
    }
}