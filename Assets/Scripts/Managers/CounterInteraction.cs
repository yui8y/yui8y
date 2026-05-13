using UnityEngine;
using UnityEngine.InputSystem;

public class CounterInteraction : MonoBehaviour
{
    bool playerNear = false;

    void Update()
    {
        if(playerNear && Keyboard.current.eKey.wasPressedThisFrame)
        {
            GameManager.instance.dialogueUI.SetActive(true);

            GameManager.instance.dialogueText.text = "いらっしゃいませ、ご注文はいかがいたしますか?";
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