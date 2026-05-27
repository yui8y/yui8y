using UnityEngine;
using UnityEngine.InputSystem;



public class CounterInteraction : MonoBehaviour
{

    public string startNodeId;
    bool playerNear = false;
    bool hasStartedTalk = false;  // 一度会話を始めたかのフラグ
    public string nextSceneName;  // Inspectorで遷移先シーン名を入力する  

    void Update()
    {
        if(playerNear &&
            !hasStartedTalk &&                              // まだ会話してないとき
            !DialogueManager.instance.IsTalking &&          // 会話中じゃないとき
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            hasStartedTalk = true;                          // 会話開始フラグを立てる
            DialogueNode node = CSVLoader.GetNode(startNodeId);
            DialogueManager.instance.StartTalk(node);
        }
    }
    System.Collections.IEnumerator StartTalkNextFrame()
    {
        yield return null;  // 1フレーム待つ（EndTalkが完全に終わるのを待つ）

        // 待った後もまだ会話中じゃなければ開始
        if (!DialogueManager.instance.IsTalking)
        {
            DialogueNode node = CSVLoader.GetNode(startNodeId);
            DialogueManager.instance.StartTalk(node);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }


    void OnEnable()
    {
        // 会話終了イベントに処理を登録する
        DialogueManager.instance.onEndTalk += OnDialogueEnd;
    }

    void OnDisable()
    {
        // 登録を解除する（メモリリーク防止）
        DialogueManager.instance.onEndTalk -= OnDialogueEnd;
    }  
    void OnDialogueEnd()
    {
        if (!hasStartedTalk) return;  // この会話台じゃない場合はスキップ
        SceneTransition.instance.FadeToScene(nextSceneName);  // フェード開始
    }
}