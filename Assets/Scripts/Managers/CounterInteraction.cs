using UnityEngine;
using UnityEngine.InputSystem;

public class CounterInteraction : MonoBehaviour
{
    [Header("会話開始ノードID")]
    public string startNodeId;

    [Header("会話終了後に遷移するシーン")]
    public string nextSceneName;

    private bool playerNear = false;
    private bool hasStartedTalk = false;

    void Update()
    {
        if (!playerNear) return;

        if (hasStartedTalk) return;

        if (DialogueManager.instance.IsTalking) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            DialogueNode node = CSVLoader.GetNode(startNodeId);

            if (node == null)
            {
                Debug.LogError($"開始ノード '{startNodeId}' が見つかりません！");
                return;
            }

            hasStartedTalk = true;
            DialogueManager.instance.StartTalk(node);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }

    void OnEnable()
    {
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.onEndTalk += OnDialogueEnd;
        }
    }

    void OnDisable()
    {
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.onEndTalk -= OnDialogueEnd;
        }
    }

    void OnDialogueEnd()
    {
        // このカウンターで開始した会話だけ反応
        if (!hasStartedTalk) return;

        hasStartedTalk = false;

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneTransition.instance.FadeToScene(nextSceneName);
        }
    }
}