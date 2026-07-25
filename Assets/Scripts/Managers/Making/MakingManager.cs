using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using TMPro;



public class MakingManager : MonoBehaviour
{
    public static MakingManager instance;
    public SpriteRenderer overlayImage;  // InspectorでOverlayのImageをセット
    public GameObject[] annotationParts;
    public bool isTooltipOpen = false;  // TooltipPanel表示中かどうか
    public GameObject currentTooltipPanel;  // 今開いてるTooltipPanelを記憶

    public AnnotationPart currentAnnotationPart;  // 今開いてるAnnotationPartを記憶

    public bool firstcontact = true;    // 初めての会話の場合、イベント発火用

    public string startNodeId;  // 会話開始ノードID

    public static bool DebugMode = false;

    public GameObject fadeRight;        // Fade_right
    public GameObject tooltipPanel;     // TooltipPanel
    public GameObject annotationLayer;  // Annotationの親オブジェクト


    // フェーズの種類
    public enum Phase
    {
        Analysis,   // 分析フェーズ
        Dialogue    // 会話フェーズ
    }

    public Phase currentPhase;  // 現在のフェーズ

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //EnterAnalysisPhase();  // シーン開始時に分析フェーズから始める
        overlayImage.color = new Color(0.04f, 0.08f, 0.2f, 0f);  // 最初は透明
        SceneTransition.instance.onFadeInComplete += OnFadeInComplete;
        if(DebugMode == true){
            OnFadeInComplete(); //デバッグテスト用。直接呼出し
        }
    }

    void Update()
    {
        // TooltipPanel表示中にTooltipPanel以外をクリックしたら閉じる
        if (isTooltipOpen && 
            currentAnnotationPart != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            Debug.Log("クリック検知！ hit: " + (hit.collider != null ? hit.collider.name : "null"));

            // TooltipPanel内のオブジェクトじゃなければ閉じる
            if (hit.collider == null ||
                !hit.collider.transform.IsChildOf(currentTooltipPanel.transform))
            {
                currentAnnotationPart.CloseTooltip();  // 直接呼ぶ！
            }
        }
    }

    // 分析フェーズに入る
    public void EnterAnalysisPhase()
    {
        currentPhase = Phase.Analysis;          // フェーズをAnalysisに設定
    }

    // 部位クリック時に呼ぶ。startNodeIdを渡して会話へ
    public void EnterDialoguePhase(string startNodeId)
    {
        currentPhase = Phase.Dialogue;

        // フェードアウトしてから非表示にする
        FadeOutAndHide(tooltipPanel.GetComponent<SpriteRenderer>());

        // Annotationの子オブジェクトのSpriteRendererをまとめてフェード
        foreach (SpriteRenderer sr in annotationLayer.GetComponentsInChildren<SpriteRenderer>())
        {
            FadeOutAndHide(sr);
        }

        // TextMeshProも別途フェードアウト
        foreach (TextMeshPro tmp in annotationLayer.GetComponentsInChildren<TextMeshPro>())
        {
            tmp.DOFade(0f, 0.4f)                    // 0.4秒でアルファを0に
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                    tmp.gameObject.SetActive(false) // フェード完了後に非表示
                );
        }

        // TooltipPanelは縮小アニメーションで消す
        if (tooltipPanel.activeSelf)  // 表示中のときだけ実行
        {
            tooltipPanel.transform
                .DOScale(Vector3.zero, 0.15f)        // 縮小
                .SetEase(Ease.InBack)               // 逆バネで閉じる
                .OnComplete(() =>
                    tooltipPanel.SetActive(false)); // 完了後に非表示
        }

        // Fade_rightのフェードアウト完了後にStartTalkを呼ぶ
        fadeRight.GetComponent<SpriteRenderer>()
        .DOFade(0f, 0.3f)
        .SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            fadeRight.SetActive(false);
            DialogueNode node = CSVLoader.GetNode(startNodeId);
            DialogueManager.instance.StartTalk(node);
        });
    }

    // SpriteRendererをフェードアウトして非表示にするメソッド
    void FadeOutAndHide(SpriteRenderer sr)
    {
        if (sr == null) return;                  // nullチェック

        sr.DOFade(0f, 0.4f)                     // 0.4秒でアルファを0に
            .SetEase(Ease.OutCubic)             // 機械的なイージング
            .OnComplete(() =>
                sr.gameObject.SetActive(false)  // フェード完了後に非表示
            );
    }


    void OnFadeInComplete()
    {
        if (firstcontact)
        {
            currentPhase = Phase.Dialogue;
            DialogueNode node = CSVLoader.GetNode(startNodeId);
            DialogueManager.instance.onEndTalk += OnIntroDialogueEnd;
            DialogueManager.instance.StartTalk(node);
        }
    }

    void OnIntroDialogueEnd()
    {
        // 二重発火防止のため解除
        DialogueManager.instance.onEndTalk -= OnIntroDialogueEnd;

        // overlayのフェードインを実行
        for (int i = 0; i < annotationParts.Length; i++)
        {
            int index = i;
            DOVirtual.DelayedCall(
                i * 0.1f,
                () => annotationParts[index].SetActive(true)
            );
        }

        overlayImage.DOFade(0.7f, 0.5f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => EnterAnalysisPhase());
    }

    void OnDisable()
    {
        SceneTransition.instance.onFadeInComplete -= OnFadeInComplete;
    }
}
