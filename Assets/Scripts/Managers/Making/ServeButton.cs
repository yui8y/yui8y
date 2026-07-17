using UnityEngine;

public class ServeButton : MonoBehaviour
{
    void OnMouseDown()
    {
        CoffeeMakingManager.instance.OnServeButtonClicked();
    }
}