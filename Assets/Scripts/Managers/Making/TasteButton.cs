using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TasteButton : MonoBehaviour
{
    public string optionId;          // 豆の名称ID
    public string detailText;        // ホバー時の詳細説明

    public SpriteRenderer iconImage;          // 豆のアイコンImage
    public SpriteRenderer selectedFrame;      // 選択中を示す枠

    public bool isBeans = true;      // true=豆ボタン, false=香りボタン
    public bool isSelected = false;  //選択状態かどうか

    void Start()
    {
        selectedFrame.gameObject.SetActive(false);  // 最初は未選択状態
        //Debug.Log("起動したよ: " + optionId);
        isSelected = false;  // 初期化
    }

    void OnMouseEnter()
    {
        Debug.Log("ホバー！ isSelected: " + isSelected + " selectedFrame: " + selectedFrame);
        if (!isSelected)
        {
            selectedFrame.gameObject.SetActive(true);
        }
        CoffeeMakingManager.instance.ShowTooltip(detailText, optionId);
    }

    void OnMouseExit()
    {
        if (!isSelected)  // 未選択のときだけホバー枠を消す
        {
            selectedFrame.gameObject.SetActive(false);
        }
        CoffeeMakingManager.instance.HideTooltip();
    }

    void OnMouseDown()
    {
        isSelected = true;                    // 選択状態にする
        selectedFrame.gameObject.SetActive(true);  // 枠を固定表示

        CoffeeMakingManager.instance.MakeCoffee();
        if (isBeans)
        {
            CoffeeMakingManager.instance.SelectTaste(optionId);
        }
        else
        {
            CoffeeMakingManager.instance.SelectFlavor(optionId);
        }

        transform.parent.GetComponent<TastePanel>()?.SelectButton(this);
    }

    public void SetSelected(bool selected)
    {
        Debug.Log(optionId + " SetSelected呼ばれた: " + selected);  // 追加
        isSelected = selected;
        selectedFrame.gameObject.SetActive(selected);
    }
}