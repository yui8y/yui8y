using UnityEngine;
using TMPro;

public class LabelClickHandler : MonoBehaviour
{
    public AnnotationPart annotationPart;   // 親のAnnotationPartを参照

    public SpriteRenderer dotSprite;        // CircleのSpriteRenderer
    public SpriteRenderer lineSprite;       // LineのSpriteRenderer
    public TextMeshPro labelText;           // LabelTextのTextMeshPro

    public Color normalColor = Color.white;
    public Color hoverColor = new Color(1f, 0.6f, 0.2f, 1f);  // オレンジ

    public void OnHoverEnter()
    {
        dotSprite.color = hoverColor;
        lineSprite.color = hoverColor;
        labelText.color = hoverColor;
    }

    public void OnHoverExit()
    {
        dotSprite.color = normalColor;
        lineSprite.color = normalColor;
        labelText.color = normalColor;
    }

    public void OnClicked()
    {
        annotationPart.OnPartClicked();
        dotSprite.color = normalColor;
        lineSprite.color = normalColor;
        labelText.color = normalColor;
    }
    }