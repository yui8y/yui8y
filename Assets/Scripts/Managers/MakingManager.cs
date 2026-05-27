using UnityEngine;

public class MakingManager : MonoBehaviour
{
    public static MakingManager instance;

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
        EnterAnalysisPhase();  // シーン開始時に分析フェーズから始める
    }

    // 分析フェーズに入る
    public void EnterAnalysisPhase()
    {
        currentPhase = Phase.Analysis;          // フェーズをAnalysisに設定
        Debug.Log("分析フェーズ開始！charId: " + GameData.currentCharId);
        // テスト用：とりあえずnode_002から会話を直接起動
        EnterDialoguePhase("node_002");
        // ここに分析フェーズのUI表示処理を後で追加
    }

    // 部位クリック時に呼ぶ。startNodeIdを渡して会話フェーズへ
    public void EnterDialoguePhase(string startNodeId)
    {
        currentPhase = Phase.Dialogue;          // フェーズをDialogueに設定

        DialogueNode node = CSVLoader.GetNode(startNodeId);  // ノードを取得
        DialogueManager.instance.StartTalk(node);             // 会話開始
    }
}
