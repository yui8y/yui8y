using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;  // DOTweenを使用

public class AnnotationPart : MonoBehaviour
{

    public SpriteRenderer dotSprite;   // ○のSpriteRenderer
    public SpriteRenderer lineSprite;  // 線のSpriteRenderer
    public TextMeshProUGUI labelText;      // 「目」などのラベル

    Vector3 lineOriginalScale;         // 線の元のスケールを記憶


    void Start()
    {
        lineOriginalScale = lineSprite.transform.localScale;
        dotSprite.transform.localScale = Vector3.zero;
        lineSprite.transform.localScale = new Vector3(0, lineOriginalScale.y, lineOriginalScale.z);
        labelText.text = "";
        
        gameObject.SetActive(false);

    }

    void OnEnable()
    {
        // SetActive(true)されたときにアニメーション開始
        if (lineOriginalScale == Vector3.zero) return;  // Start前に呼ばれたら無視
        
        // スケールをリセットしてからアニメーション開始
        dotSprite.transform.localScale = Vector3.zero;
        lineSprite.transform.localScale = new Vector3(0, lineOriginalScale.y, lineOriginalScale.z);
        labelText.text = "";
        
        PlayAppearAnimation();
    }

    void PlayAppearAnimation()
    {
        Sequence seq = DOTween.Sequence();  // 順番に実行するシーケンスを作成

        // ① ○が拡大
        seq.Append(
            dotSprite.transform
                .DOScale(Vector3.one, 0.1f)          // 0→1に0.1秒で拡大
                .SetEase(Ease.OutBack)               // バネっぽいイージング
        );

        // ② 線が伸びる
        seq.Append(
            lineSprite.transform
                .DOScaleX(lineOriginalScale.x, 0.15f) // X方向だけ伸ばす
                .SetEase(Ease.OutCubic)               // 機械的なイージング
        );

        // ③ シーケンス終了後にタイプライターを呼ぶ
        seq.OnComplete(() => 
        MakingManager.instance.StartCoroutine(TypeLabel("目")));  // MakingManagerから呼ぶ！
    }

    IEnumerator TypeLabel(string text)
    {
        labelText.text = "";                    // 最初は空にする

        foreach (char c in text)               // 1文字ずつ取り出す
        {
            labelText.text += c;               // 1文字追加
            yield return new WaitForSeconds(0.08f); // 1文字ごとに待つ
        }
}
}
