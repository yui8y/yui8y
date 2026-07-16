using UnityEngine;

public class TalkButton : MonoBehaviour
{
    private string nodeId;  // 会話開始ノードID

    // AnnotationPartから呼ばれてnodeIdをセット
    public void SetNodeId(string id)
    {
        nodeId = id;
    }

    // ボタンクリック時に呼ぶ
    public void OnTalkButtonClicked()
    {
        Debug.Log("TalkButtonクリック！ nodeId: " + nodeId);  // これが出るか確認
        if (string.IsNullOrEmpty(nodeId)) return;

        //  DialogueNode node = CSVLoader.GetNode(nodeId);  // ノードを取得
        //  DialogueManager.instance.StartTalk(node);        // 会話開始
        MakingManager.instance.EnterDialoguePhase(nodeId); // フェーズ切り替え
    }
}