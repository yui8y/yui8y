using UnityEngine;

public class TalkButtonCollider : MonoBehaviour
{
    public TalkButton talkButton;  // TalkButtonコンポーネントを参照

    void OnMouseDown()
    {
        Debug.Log("TalkButtonCollider OnMouseDown!");
        if (!MakingManager.instance.isTooltipOpen) return;  // TooltipPanel非表示中は無効
        talkButton.OnTalkButtonClicked();
    }
}