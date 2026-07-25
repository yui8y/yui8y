using System.Collections.Generic;
using UnityEngine;

public class CustomerData
{
    public string charId;
    public Dictionary<string, int> tasteScores  = new Dictionary<string, int>(); // 味の好み度
    public Dictionary<string, int> flavorScores = new Dictionary<string, int>(); // 香りの好み度
    public string successNodeId;
    public string greatSuccessNodeId;
    public string failNodeId;
    public string criticalFailNodeId;
}

public class CustomerDataLoader : MonoBehaviour
{
    public static CustomerDataLoader instance;
    private static Dictionary<string, CustomerData> dataDict = new Dictionary<string, CustomerData>();

    void Awake()
    {
        instance = this;
        LoadCustomerCSV();
    }

    void LoadCustomerCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("customer");

        if (csvFile == null)
        {
            Debug.LogError("customer.csv が見つかりません");
            return;
        }

        string[] lines = csvFile.text.TrimStart('\uFEFF').Split('\n');
        string[] headers = lines[0].Split(',');  // ヘッダー行を取得

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] cols = lines[i].Split(',');
            CustomerData data = new CustomerData();
            data.charId = cols[0].Trim();

            for (int j = 1; j < headers.Length; j++)
            {
                string header = headers[j].Trim();  // ヘッダー名を取得

                if (header.StartsWith("taste_"))
                {
                    // taste_XXX → 味の好み度として登録
                    string tasteId = header.Replace("taste_", "");  // "taste_"を除いたIDを取得
                    data.tasteScores[tasteId] = int.Parse(cols[j].Trim());
                }
                else if (header.StartsWith("flavor_"))
                {
                    // flavor_XXX → 香りの好み度として登録
                    string flavorId = header.Replace("flavor_", "");  // "flavor_"を除いたIDを取得
                    data.flavorScores[flavorId] = int.Parse(cols[j].Trim());
                }
                else if (header == "successNodeId")       data.successNodeId = cols[j].Trim();
                else if (header == "greatSuccessNodeId")  data.greatSuccessNodeId = cols[j].Trim();
                else if (header == "failNodeId")          data.failNodeId = cols[j].Trim();
                else if (header == "criticalFailNodeId")  data.criticalFailNodeId = cols[j].Trim();
            }

            dataDict[data.charId] = data;
            Debug.Log("読み込み完了: " + data.charId);
        }
    }

    // charIdでデータを取得
    public static CustomerData GetCustomerData(string charId)
    {
        if (dataDict.TryGetValue(charId, out CustomerData data))
        {
            return data;
        }
        Debug.LogError("CustomerDataが見つかりません: " + charId);
        return null;
    }

    // そのターンの「好み」を取得（最も高いスコアのID）
    public static string GetTopTaste(string charId)
    {
        CustomerData data = GetCustomerData(charId);
        if (data == null) return "";

        string topId = "";
        int topScore = -1;

        foreach (var kvp in data.tasteScores)
        {
            if (kvp.Value > topScore)    // より高いスコアがあれば更新
            {
                topScore = kvp.Value;
                topId = kvp.Key;
            }
        }
        return topId;  // 最も好み度が高い味のIDを返す
    }

    // そのターンの「好みの香り」を取得
    public static string GetTopFlavor(string charId)
    {
        CustomerData data = GetCustomerData(charId);
        if (data == null) return "";

        string topId = "";
        int topScore = -1;

        foreach (var kvp in data.flavorScores)
        {
            if (kvp.Value > topScore)
            {
                topScore = kvp.Value;
                topId = kvp.Key;
            }
        }
        return topId;
    }
}