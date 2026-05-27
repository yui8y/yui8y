using System.Collections;          // IEnumeratorを使うために必要
using UnityEngine;                  // MonoBehaviourを使うために必要
using UnityEngine.SceneManagement;  // SceneManagerを使うために必要
using UnityEngine.UI;               // Imageを使うために必要

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;  // どこからでもアクセスできるシングルトン

    public Image fadeImage;          // 黒いパネルのImageコンポーネント
    public float fadeDuration = 1f;  // フェードにかかる秒数

    void Awake()
    {
        instance = this;             // 自分自身を登録
    }

    void Start()
    {
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
    }
}