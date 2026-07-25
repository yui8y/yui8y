using UnityEngine;
using UnityEngine.InputSystem;

public class CounterInteraction : MonoBehaviour
{
    public string startNodeId;

    public string nextSceneName;

    private bool playerNear = false;
    public static bool hasStartedTalk = false;

    void Update()
    {
        if (!playerNear) return;

        if (hasStartedTalk) return;

        if (DialogueManager.instance.IsTalking) return;

        if (Keyboard.current.eKey.wasPressedThisFrame && !GameData.isReturningFromMaking)
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
        Debug.Log("TriggerEnter: " + other.tag);
        if (other.CompareTag("Customer"))
        {
            playerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Customer"))
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
        if(!hasStartedTalk){
            return;
        }

        if (GameData.isReturningFromMaking){
            return;
        }

        // お客さんの位置を保存
        GameObject customer = GameObject.FindWithTag("Customer");
        if (customer != null)
        {
            ComeCustomer.SavePosition(customer.transform.position);  // 位置を保存
        }

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneTransition.instance.FadeToScene(nextSceneName);
        }
    }
}