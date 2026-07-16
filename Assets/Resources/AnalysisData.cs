// 1つの部位データを表すクラス
[System.Serializable]
public class AnalysisPartData
{
    public string charId;           // キャラID
    public string partId;           // 部位ID（eye, hairなど）
    public string partName;         // 部位名（tooltipTitleとして使う）
    public int requiredAffinity;    // 解放に必要な好感度
    public string hoverText;        // 詳細テキスト（tooltipTextとして使う）
    public string startNodeId;      // 会話開始ノードID
}