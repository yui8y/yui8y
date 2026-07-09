using UnityEngine;
using UnityEngine.XR;

public class AnnotationCollider : MonoBehaviour
{
    public LabelClickHandler handler;  // 親のLabelClickHandlerを参照

    void OnMouseEnter()
    {
        if (MakingManager.instance.isTooltipOpen) return;  // TooltipPanel表示中は無効
        handler.OnHoverEnter();
    }

    void OnMouseExit()
    {
        if (MakingManager.instance.isTooltipOpen) return;
        handler.OnHoverExit();
    }

    void OnMouseDown()
    {
        if (MakingManager.instance.isTooltipOpen) return;
        handler.OnClicked();
    }
    }