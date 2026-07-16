using UnityEngine;

public class TabManager : MonoBehaviour
{
    public GameObject beanPanel;
    public GameObject flavorPanel;

    public SpriteRenderer tabBean;
    public SpriteRenderer tabFlavor;

    public Color activeColor;   // 選択中のタブの色
    public Color inactiveColor; // 非選択のタブの色

    public static TabManager instance;

    void Start()
    {
        // 最初は豆パネルを表示
        ShowBeanPanel();
    }

    public void ShowBeanPanel()
    {
        SetActivePanel(beanPanel);
        SetActiveTab(tabBean);
    }

    public void ShowFlavorPanel()
    {
        SetActivePanel(flavorPanel);
        SetActiveTab(tabFlavor);
    }

    void SetActivePanel(GameObject activePanel)
    {
        beanPanel.SetActive(false);
        flavorPanel.SetActive(false);
        activePanel.SetActive(true);
    }

    void SetActiveTab(SpriteRenderer activeTab)
    {
        tabBean.color = inactiveColor;
        tabFlavor.color = inactiveColor;
        activeTab.color = activeColor;
    }
}