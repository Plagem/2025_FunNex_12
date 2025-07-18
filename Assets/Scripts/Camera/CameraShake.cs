using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.3f;

    private Vector3 originalPos;

    void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(
            duration > 0f ? duration : shakeDuration,
            magnitude > 0f ? magnitude : shakeMagnitude
        ));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 randomPoint = originalPos + (Vector3)Random.insideUnitCircle * magnitude;
            transform.localPosition = randomPoint;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
