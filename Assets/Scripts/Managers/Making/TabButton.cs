using UnityEngine;

public class TabButton : MonoBehaviour
{
    public enum TabType { Bean, Flavor }
    public TabType tabType;

    void OnMouseDown()
    {
        switch (tabType)
        {
            case TabType.Bean:
                TabManager.instance.ShowBeanPanel();
                break;
            case TabType.Flavor:
                TabManager.instance.ShowFlavorPanel();
                break;
        }
    }
}