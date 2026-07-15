using UnityEngine;

public class DebugActive : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log("DialoguePanel表示された！");  // どこから呼ばれてるか確認
        Debug.Log(UnityEngine.StackTraceUtility.ExtractStackTrace());  // 呼び出し元を表示
    }
}