using UnityEngine;
using System.Collections.Generic;  // Listを使うために必要

public class TastePanel : MonoBehaviour
{
    public List<TasteButton> buttons;  // このパネル内の全ボタン

    // クリックされたボタン以外の選択枠を消す（ラジオボタン式）
    public void SelectButton(TasteButton selected)
    {
        Debug.Log("SelectButton呼ばれた！ selected: " + selected.optionId);  // 追加
        foreach (var btn in buttons)
        {
            bool isActive = btn == selected;
            Debug.Log(btn.optionId + " → " + isActive);  // 追加
            btn.SetSelected(isActive);
        }
    }
}