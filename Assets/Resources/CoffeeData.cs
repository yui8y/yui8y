using UnityEngine;

// 1つの「豆」または「香り」の選択肢を表すクラス
[System.Serializable]
public class CoffeeOption
{
    public string id;          // 識別子
    public string label;       // 表示名
    public Sprite icon;        // 豆のアイコン画像
}

// プレイヤーが選んだコーヒーの内容
public class CoffeeSelection
{
    public string selectedTaste;   // 選択した味のid
    public string selectedAroma;   // 選択した香りのid
    public bool isHot = true;      // true=ホット, false=アイス
}