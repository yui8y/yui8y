using System.Collections;         
using UnityEngine;                  
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;  // どこからでもアクセスできるシングルトン

    public Image fadeImage;          // 黒いパネルのImageコンポーネント
    public float fadeDuration = 1f;  // フェードにかかる秒数

    public System.Action onFadeInComplete;


    void Awake()
    {
        instance = this;             // 自分自身を登録
        Debug.Log("SceneTransition起動！ instance = " + instance);

    }

    void Start()
    {
        Debug.Log("SceneTransition.instance = " + SceneTransition.instance);
        if (GameData.shouldFadeIn)
        {
            GameData.shouldFadeIn = false;  // フラグをリセット
            StartCoroutine(FadeIn());
        }
    }

    // 外から呼ぶメソッド。シーン名を渡す
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));  // フェードアウト開始
    }

    IEnumerator FadeOut(string sceneName)
    {
        float time = 0f;                     // 経過時間

        while (time < fadeDuration)
        {
            time += Time.deltaTime;          // フレームごとに時間を加算
            float alpha = time / fadeDuration; // 0→1に変化するアルファ値
            fadeImage.color = new Color(0, 0, 0, alpha); // 黒くなっていく
            yield return null;               // 1フレーム待つ
        }

        fadeImage.color = new Color(0, 0, 0, 1f); // 完全に黒にする
        GameData.shouldFadeIn = true;   // 次のシーンでフェードインする
        SceneManager.LoadScene(sceneName);         // シーンを読み込む
    }

    IEnumerator FadeIn()
    {
        float time = 0f;                      // 経過時間

        fadeImage.color = new Color(0, 0, 0, 1f);  // 最初は真っ黒からスタート

        while (time < fadeDuration)
        {
            time += Time.deltaTime;            // フレームごとに時間を加算
            float alpha = 1f - (time / fadeDuration); // 1→0に変化するアルファ値
            fadeImage.color = new Color(0, 0, 0, alpha); // だんだん透明になる
            yield return null;                 // 1フレーム待つ
        }

        fadeImage.color = new Color(0, 0, 0, 0f);  // 完全に透明にする
        onFadeInComplete?.Invoke();  // フェードイン完了を通知！

    }

}