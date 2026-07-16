using System.Collections.Generic;
using UnityEngine;

public class AnalysisCSVLoader : MonoBehaviour
{
    // charIdをキーにして部位データのリストを管理
    private static Dictionary<string, List<AnalysisPartData>> dataDict
        = new Dictionary<string, List<AnalysisPartData>>();

    void Awake()
    {
        LoadAnalysisCSV();  // 起動時にCSVを読み込む
    }

    void LoadAnalysisCSV()
    {
        // ResourcesフォルダからCSVを読み込む
        TextAsset csvFile = Resources.Load<TextAsset>("analysis");

        if (csvFile == null)
        {
            Debug.LogError("analysis.csv が見つかりません");
            return;
        }

        // BOMを除去してから改行で分割
        string[] lines = csvFile.text.TrimStart('\uFEFF').Split('\n');

        // 1行目はヘッダーなのでスキップ
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue; // 空行スキップ

            string[] cols = lines[i].Split(',');  // カンマで分割

            if (cols.Length < 6) continue;        // 列数が足りない行はスキップ

            AnalysisPartData data = new AnalysisPartData
            {
                charId           = cols[0].Trim(), // キャラID
                partId           = cols[1].Trim(), // 部位ID
                partName         = cols[2].Trim(), // 部位名
                requiredAffinity = int.Parse(cols[3].Trim()), // 必要好感度
                hoverText        = cols[4].Trim(), // 詳細テキスト
                startNodeId      = cols[5].Trim()  // 会話開始ノードID
            };

            // charIdのリストがなければ新規作成
            if (!dataDict.ContainsKey(data.charId))
            {
                dataDict[data.charId] = new List<AnalysisPartData>();
            }

            dataDict[data.charId].Add(data);  // リストに追加
        }

        Debug.Log("analysisCSVロード完了！");
    }

    // charIdとpartIdで部位データを取得
    public static AnalysisPartData GetPartData(string charId, string partId)
    {
        if (dataDict.TryGetValue(charId, out List<AnalysisPartData> parts))
        {
            return parts.Find(p => p.partId == partId);  // partIdで検索
        }

        Debug.LogError("部位データが見つかりません: " + charId + " / " + partId);
        return null;
    }

    // charIdで全部位データを取得
    public static List<AnalysisPartData> GetAllParts(string charId)
    {
        if (dataDict.TryGetValue(charId, out List<AnalysisPartData> parts))
        {
            return parts;  // そのキャラの全部位データを返す
        }

        return null;
    }
}