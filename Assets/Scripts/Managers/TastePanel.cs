using UnityEngine;
using System.Collections.Generic;  // Listを使うために必要

public class TastePanel : MonoBehaviour
{
    public List<TasteButton> buttons;  // このパネル内の全ボタン

    // クリックされたボタン以外の選択枠を消す（ラジオボタン式）
    public void SelectButton(TasteButton selected)
    {
        foreach (var btn in buttons)
        {
            btn.SetSelected(btn == selected);  // クリックされたものだけtrueに
        }
    }
}