using System.Collections.Generic;  // Listを使うために必要

// 1つの会話ノードを表すクラス
[System.Serializable]
public class DialogueNode
{
    public string nodeId;           // ノードの識別子（例: node_001）
    public string charId;           // キャラID（例: npc_a）
    public string speaker;          // 話者名（表示用）
    public string text;             // 会話テキスト
    public string nextNodeId;       // 次のノードID（nullなら会話終了）
    public List<KeywordData> keywords = new List<KeywordData>(); // キーワードリスト
}

// 1つのキーワードを表すクラス
[System.Serializable]
public class KeywordData
{
    public string word;             // キーワードの単語
    public KeywordType type;        // キーワードの種類
    public int affinityChange;      // 好感度の変動値
    public string nextNodeId;       // この単語選択時の次ノード
}

// キーワードの種類
public enum KeywordType
{
    Normal,     // 通常
    Like,       // 好き
    Critical,   // クリティカル
    Dislike,    // 苦手
    Landmine    // 地雷
}