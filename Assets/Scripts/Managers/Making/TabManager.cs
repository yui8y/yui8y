using UnityEngine;
using DG.Tweening;

public class TabManager : MonoBehaviour
{
    public GameObject beanPanel;
    public GameObject flavorPanel;

    public SpriteRenderer tabBean;
    public SpriteRenderer tabFlavor;

    public Color activeColor;   // 選択中のタブの色
    public Color inactiveColor; // 非選択のタブの色

    public static TabManager instance;

    public float tabMoveAmount = 0.1f;  // 右に移動する量

    Vector3 tabBeanOriginalPos;    // 豆タブの元の位置
    Vector3 tabFlavorOriginalPos;  // 香りタブの元の位置

    void Start()
    {
        // 最初は豆パネルを表示
        ShowBeanPanel();
    }

    void Awake()
    {
        instance = this;
        tabBeanOriginalPos = tabBean.transform.localPosition;      // 元の位置を記憶
        tabFlavorOriginalPos = tabFlavor.transform.localPosition;  // 元の位置を記憶
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
        // 全タブを元の位置に戻す
        tabBean.transform
            .DOLocalMoveX(tabBeanOriginalPos.x, 0.1f)    // 元のX座標に戻す
            .SetEase(Ease.OutCubic);

        tabFlavor.transform
            .DOLocalMoveX(tabFlavorOriginalPos.x, 0.1f)  // 元のX座標に戻す
            .SetEase(Ease.OutCubic);

        // 選択したタブだけ右に移動
        activeTab.transform
            .DOLocalMoveX(
                activeTab == tabBean ?
                tabBeanOriginalPos.x + tabMoveAmount :    // 豆タブなら豆の元の位置+移動量
                tabFlavorOriginalPos.x + tabMoveAmount,   // 香りタブなら香りの元の位置+移動量
                0.1f
            )
            .SetEase(Ease.OutCubic);
    }

    
}