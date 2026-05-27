// MonoBehaviourを継承しない静的管理クラス
// どのシーンからでもアクセスできてシーンをまたいでも消えない
public static class GameData
{
    public static string currentCharId;  // 今対応中のお客さんのID
    public static bool shouldFadeIn = false;  // フェードインが必要かのフラグ
    public static int money = 0;           // シーンをまたいで保持するお金

}