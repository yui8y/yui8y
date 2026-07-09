using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;  // DOTweenを使用

public class AnnotationPart : MonoBehaviour
{

    public SpriteRenderer dotSprite;   // ○のSpriteRenderer
    public SpriteRenderer lineSprite;  // 線のSpriteRenderer
    public TextMeshPro labelText;      // 「目」などのラベル

    public string labelID;

    Vector3 lineOriginalScale;         // 線の元のスケールを記憶
    Vector3 tooltipOriginalScale;  // TooltipPanelの元のスケールを記憶


    public GameObject tooltipPanel;        // TooltipPanel
    public TextMeshProUGUI tooltipTitle;   // TooltipTitleのTextMeshPro
    public TextMeshProUGUI tooltipText;    // TooltipTextのTextMeshPro
    public GameObject talkButton;          // TalkButton
    public string partId;                  // この部位のID
    public string charId;                  // キャラID
    bool isTooltipOpen = false;            // TooltipPanel表示中かどうか
    bool isAnimating = false;              // TooltipPanelがアニメーション中かどうか


    void Start()
    {
        // 元のスケールを保存
        lineOriginalScale = lineSprite.transform.localScale;
        tooltipOriginalScale = tooltipPanel.transform.localScale;

        dotSprite.transform.localScale = Vector3.zero;
        lineSprite.transform.localScale = new Vector3(0, lineOriginalScale.y, lineOriginalScale.z);
        labelText.text = "";
        
        gameObject.SetActive(false);
        tooltipPanel.SetActive(false);
    }

    void OnEnable()
    {
        // SetActive(true)されたときにアニメーション開始
        if (lineOriginalScale == Vector3.zero) return;  // Start前に呼ばれたら無視
        
        // スケールをリセットしてからアニメーション開始
        dotSprite.transform.localScale = Vector3.zero;
        lineSprite.transform.localScale = new Vector3(0, lineOriginalScale.y, lineOriginalScale.z);
        labelText.text = "";
        
        PlayAppearAnimation();
    }

    void PlayAppearAnimation()
    {
        Sequence seq = DOTween.Sequence();  // 順番に実行するシーケンスを作成

        // ○が拡大
        seq.Append(
            dotSprite.transform
                .DOScale(Vector3.one, 0.1f) // 0→1に0.1秒で拡大
                .SetEase(Ease.OutBack) // バネっぽいイージング
        );

        // 線が伸びる
        seq.Append(
            lineSprite.transform
                .DOScaleX(lineOriginalScale.x, 0.15f) // X方向だけ伸ばす
                .SetEase(Ease.OutCubic)               // 機械的なイージング
        );

        // ③ シーケンス終了後にタイプライターを呼ぶ
        seq.OnComplete(() => 
        MakingManager.instance.StartCoroutine(TypeLabel(labelID)));  // MakingManagerから呼ぶ！
    }

    // 1文字ずつ取り出す
    IEnumerator TypeLabel(string text)
    {
        labelText.text = "";

        foreach (char c in text)
        {
            labelText.text += c;
            yield return new WaitForSeconds(0.08f);
        }
    }

    // LabelTextやCircleがクリックされたときに呼ぶ
    public void OnPartClicked()
    {
        Debug.Log("OnPartClicked呼ばれた！ isAnimating: " + isAnimating + " isTooltipOpen: " + isTooltipOpen);
        if (isAnimating) return;  // アニメーション中は無視
        MakingManager.instance.currentTooltipPanel = tooltipPanel;  // 今開いたパネルを記憶
        MakingManager.instance.currentAnnotationPart = this;  // 自分自身を登録

        AnalysisPartData data = AnalysisCSVLoader.GetPartData(
            GameData.currentCharId, partId
        );

        if (data == null) return;

        tooltipTitle.text = data.partName;
        tooltipText.text = data.hoverText;
        talkButton.GetComponent<TalkButton>()?.SetNodeId(data.startNodeId);

        isTooltipOpen = !isTooltipOpen;

        if (isTooltipOpen)
        {
            isAnimating = true;
            MakingManager.instance.isTooltipOpen = true;
            tooltipPanel.SetActive(true);
            tooltipPanel.transform.localScale = Vector3.zero;

            tooltipPanel.transform
                .DOScale(tooltipOriginalScale, 0.2f)
                .SetEase(Ease.OutBack)
                .OnComplete(() => isAnimating = false);
        }
        else
        {
            isAnimating = true;
            MakingManager.instance.isTooltipOpen = false;
            tooltipPanel.transform
                .DOScale(Vector3.zero, 0.15f)
                .SetEase(Ease.InBack)
                .OnComplete(() => {
                tooltipPanel.SetActive(false);
                isAnimating = false;
            });
        }
    }

    public void CloseTooltip()
    {
        Debug.Log("CloseTooltip呼ばれた！ isAnimating: " + isAnimating);
        if (isAnimating) return;
        isTooltipOpen = false;
        MakingManager.instance.isTooltipOpen = false;

        tooltipPanel.transform
            .DOScale(Vector3.zero, 0.15f)
            .SetEase(Ease.InBack)
            .OnComplete(() => tooltipPanel.SetActive(false));
    }
}
