using UnityEngine;
using System.Collections;

/// <summary>
/// カメラを一瞬揺らすクラス
/// </summary>
public class CameraShake : MonoBehaviour
{
    // シングルトン
    public static CameraShake Instance;

    // カメラの初期位置
    private Vector3 originalPosition;

    private void Awake()
    {
        Instance = this;
        originalPosition = transform.localPosition;
    }

    /// <summary>
    /// カメラシェイク開始
    /// </summary>
    public void Shake(float duration, float strength)
    {
        StartCoroutine(ShakeCoroutine(duration, strength));
    }

    /// <summary>
    /// カメラを一定時間ランダムに揺らす
    /// </summary>
    private IEnumerator ShakeCoroutine(float duration, float strength)
    {
        float timer = 0.0f;

        while (timer < duration)
        {
            // XとYだけランダムに揺らす
            Vector3 offset = new Vector3(
                Random.Range(-strength, strength),
                Random.Range(-strength, strength),
                0.0f);

            // カメラ位置を更新
            transform.localPosition = originalPosition + offset;

            timer += Time.deltaTime;

            yield return null;
        }

        // 元の位置へ戻す
        transform.localPosition = originalPosition;
    }
}