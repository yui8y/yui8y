using System.Collections.Generic;  // DictionaryとListを使うために必要
using System.IO;                    // ファイル読み込みに必要
using UnityEngine;                  // Application.streamingAssetsPathに必要

public class CSVLoader : MonoBehaviour
{
    // nodeIdをキーにしてDialogueNodeを管理する辞書
    private static Dictionary<string, DialogueNode> nodeDict
        = new Dictionary<string, DialogueNode>();

    void Awake()
    {
        LoadDialogueCSV();  // 起動時にCSVを読み込む
    }

    void LoadDialogueCSV()
    {
        // ResourcesフォルダからCSVをTextAssetとして読み込む
        TextAsset csvFile = Resources.Load<TextAsset>("dialogue");

        // ファイルが見つからなければエラーログを出して終了
        if (csvFile == null)
        {
            Debug.LogError("dialogue.csv がResourcesフォルダに見つかりません");
            return;
        }

        // テキストを改行で分割して全行取得
        string[] lines = csvFile.text.Split('\n');

        // 1行目はヘッダーなので2行目（index=1）からループ
        for (int i = 1; i < lines.Length; i++)
        {
            string[] cols = lines[i].Split(',');  // カンマで列に分割

            if (cols.Length < 9) continue;        // 列数が足りない行はスキップ

            string nodeId  = cols[0].Trim();      // ノードID
            string charId  = cols[1].Trim();      // キャラID
            string speaker = cols[2].Trim();      // 話者名
            string text    = cols[3].Trim();      // 会話テキスト
            string kwWord  = cols[4].Trim();      // キーワード単語
            string kwType  = cols[5].Trim();      // キーワード種類
            int affinity   = int.Parse(cols[6].Trim()); // 好感度変動
            string nextId  = cols[7].Trim();      // 次のノードID
            string eventId = cols[8].Trim();      // 特別な選択肢の場合のノードID

            // このnodeIdのノードがまだ辞書にない場合は新規作成
            if (!nodeDict.ContainsKey(nodeId))
            {
                nodeDict[nodeId] = new DialogueNode
                {
                    nodeId     = nodeId,
                    charId     = charId,
                    speaker    = speaker,
                    text       = text,
                    nextNodeId = nextId,
                    nextNodeId_event = eventId
                };
            }

            // キーワードを作ってノードに追加
            KeywordData kw = new KeywordData
            {
                word          = kwWord,
                type          = ParseKeywordType(kwType), // 文字列→enumに変換
                affinityChange = affinity,
                nextNodeId    = nextId,
                nextNodeId_event = eventId
            };

            nodeDict[nodeId].keywords.Add(kw);  // ノードのキーワードリストに追加
        }

        Debug.Log("CSVロード完了！ノード数: " + nodeDict.Count);
        // 読み込んだノードのIDを全部ログに出す
        foreach (var key in nodeDict.Keys)
        {
            Debug.Log("読み込んだノード: " + key);  // node_001等が表示されるはず
        }
    }

    // 外から nodeId を指定してノードを取得するメソッド
    public static DialogueNode GetNode(string nodeId)
    {
        if (nodeDict.TryGetValue(nodeId, out DialogueNode node))
        {
            return node;   // 見つかったら返す
        }

        Debug.LogError("ノードが見つかりません: " + nodeId);
        return null;       // 見つからなければnullを返す
    }

    // 文字列のキーワード種類をenumに変換するメソッド
    KeywordType ParseKeywordType(string typeStr)
    {
        switch (typeStr)
        {
            case "Like":     return KeywordType.Like;
            case "Critical": return KeywordType.Critical;
            case "Dislike":  return KeywordType.Dislike;
            case "Landmine": return KeywordType.Landmine;
            default:         return KeywordType.Normal;  // それ以外はNormal
        }
    }
}