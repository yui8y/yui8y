using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TasteButton : MonoBehaviour,
    IPointerClickHandler
{
    public string optionId;          // ID
    public string detailText;        // ホバー時の詳細説明

    public SpriteRenderer iconImage;          // 豆のアイコンImage
    public SpriteRenderer selectedFrame;      // 選択中を示す枠

    public bool isTaste = true;      // true=味ボタン, false=香りボタン

    void Start()
    {
        selectedFrame.gameObject.SetActive(false);  // 最初は未選択状態
        //Debug.Log("起動したよ: " + optionId);
    }

    void OnMouseEnter()
    {
        //Debug.Log("入った！");
        selectedFrame.gameObject.SetActive(true);
        CoffeeMakingManager.instance.ShowTooltip(detailText, optionId);  // 共通ツールチップに表示依頼
    }

    void OnMouseExit()
    {
        //Debug.Log("出た！");
        selectedFrame.gameObject.SetActive(false);
        CoffeeMakingManager.instance.HideTooltip();  // 共通ツールチップを隠す
    }

    

    public void OnPointerClick(PointerEventData eventData)
    {
        CoffeeMakingManager.instance.MakeCoffee();
        Debug.Log("選択済み");
        if (isTaste)
        {
            CoffeeMakingManager.instance.SelectTaste(optionId);  // 味として登録
        }
        else
        {
            CoffeeMakingManager.instance.SelectAroma(optionId);  // 香りとして登録
        }

        // 同じパネル内の他のボタンの選択枠を消す
        transform.parent.GetComponent<TastePanel>()?.SelectButton(this);
    }

    public void SetSelected(bool selected)
    {
        selectedFrame.gameObject.SetActive(selected);  // 選択枠の表示切り替え
    }
}