using UnityEngine;
using UnityEngine.InputSystem;


public class CounterInteraction : MonoBehaviour
{

    public TalkDataSO talkData;
    bool playerNear = false;

    void Update()
    {
        if(playerNear && Keyboard.current.eKey.wasPressedThisFrame)
        {
            DialogueManager.instance.StartTalk(talkData);
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
}