using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class MakingManager : MonoBehaviour
{
    public static MakingManager instance;
    public SpriteRenderer overlayImage;  // InspectorでOverlayのImageをセット
    public GameObject[] annotationParts;


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
    }

    // 分析フェーズに入る
    public void EnterAnalysisPhase()
    {
        currentPhase = Phase.Analysis;          // フェーズをAnalysisに設定
        Debug.Log("分析フェーズ開始！charId: " + GameData.currentCharId);
        // テスト用：とりあえずnode_002から会話を直接起動
        //EnterDialoguePhase("node_002");
        // ここに分析フェーズのUI表示処理を後で追加
    }

    // 部位クリック時に呼ぶ。startNodeIdを渡して会話フェーズへ
    public void EnterDialoguePhase(string startNodeId)
    {
        currentPhase = Phase.Dialogue;          // フェーズをDialogueに設定

        DialogueNode node = CSVLoader.GetNode(startNodeId);  // ノードを取得
        DialogueManager.instance.StartTalk(node);             // 会話開始
    }


    void OnFadeInComplete()
    {

        // 部位ごとに0.1秒ずつタイミングをずらして表示
        for (int i = 0; i < annotationParts.Length; i++)
        {
            int index = i;  // ラムダ式内で正しいiを使うためにキャプチャ
            DOVirtual.DelayedCall(
                i * 0.1f,                                    // i番目は i*0.1秒後に実行
                () => annotationParts[index].SetActive(true) // 該当部位を表示
            );
        }

        overlayImage.DOFade(0.7f, 0.5f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => EnterAnalysisPhase());
    }

    void OnDisable()
    {
        SceneTransition.instance.onFadeInComplete -= OnFadeInComplete;  // 登録解除
    }
}
