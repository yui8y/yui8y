using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class CoffeeMakingManager : MonoBehaviour
{
    public static CoffeeMakingManager instance;  // シングルトン

    public CoffeeSelection currentSelection = new CoffeeSelection();  // 今の選択状態


    public GameObject tooltipPanel;       // 共通のツールチップパネル
    public TextMeshProUGUI tooltipText;   // 共通のツールチップテキスト
    public Transform tooltipTransform;

    public Canvas parentCanvas;           // 親のCanvas

    bool isHovering = false;              // ホバー中フラグ

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

            tooltipTransform.position = mouseWorldPos + new Vector3(0.3f, 0.3f, 0f);

            // マウススクリーン座標をワールド座標に変換
            // Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
            //     new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f)  // Zは画面からの距離
            // );

            // tooltipTransform.position = mouseWorldPos + new Vector3(0.3f, 0.3f, 0); // 少しオフセット
        }
    }

    // ボタン側から呼ぶ：ツールチップを表示
    public void ShowTooltip(string text)
    {
        tooltipText.text = text;       // テキストをセット
        tooltipPanel.SetActive(true);  // 表示
        isHovering = true;             // 追従開始
    }

    // ボタン側から呼ぶ：ツールチップを隠す
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false); // 非表示
        isHovering = false;            // 追従停止
    }

    // 味が選ばれたときに呼ぶ
    public void SelectTaste(string tasteId)
    {
        currentSelection.selectedTaste = tasteId;   // 選択した味を記録
        Debug.Log("味選択: " + tasteId);
    }

    // 香りが選ばれたときに呼ぶ
    public void SelectAroma(string aromaId)
    {
        currentSelection.selectedAroma = aromaId;   // 選択した香りを記録
        Debug.Log("香り選択: " + aromaId);
    }

    // 温度が選ばれたときに呼ぶ
    public void SelectTemp(bool isHot)
    {
        currentSelection.isHot = isHot;             // ホット/アイスを記録
        Debug.Log("温度選択: " + (isHot ? "ホット" : "アイス"));
    }
}