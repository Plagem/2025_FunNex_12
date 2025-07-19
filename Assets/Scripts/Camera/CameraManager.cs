using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public Transform defaultTarget;          // 평소 따라갈 타겟 (플레이어)
    public float followSpeed = 5f;
    public float zoomedSize = 8f;
    public float defaultSize = 5f;
    public float focusDuration = 0.5f;

    private Camera cam;
    private bool isFocusing = false;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (!isFocusing && defaultTarget != null)
        {
            Vector3 targetPos = defaultTarget.position;
            targetPos.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }
    }

    public void FocusOnEnemy(Transform enemy)
    {
        if (!isFocusing)
        {
            StartCoroutine(FocusRoutine(enemy));
        }
    }

    private IEnumerator FocusRoutine(Transform enemy)
    {
        isFocusing = true;

        // 1. 카메라 흔들기
        CameraShake shake = GetComponent<CameraShake>();
        if (shake != null)
            shake.Shake(0.15f, 0.4f);  // 강도, 지속시간은 조절 가능

        yield return new WaitForSecondsRealtime(0.15f);  // 흔들림 대기

        // 2. 시간 느리게
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;  // 물리 업데이트도 맞춰줌

        // 3. 줌인 & 이동
        float t = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = enemy.position;
        targetPos.z = startPos.z;

        float startSize = cam.orthographicSize;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / 0.5f;  // Time.timeScale 영향 안 받게
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            cam.orthographicSize = Mathf.Lerp(startSize, zoomedSize, t);
            yield return null;
        }

        // 4. 느린 상태 유지
        yield return new WaitForSecondsRealtime(focusDuration);

        // 5. 시간 복구
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // 6. 줌아웃 & 복귀
        t = 0f;
        Vector3 backPos = defaultTarget.position;
        backPos.z = startPos.z;

        while (t < 1f)
        {
            t += Time.deltaTime / 0.5f;
            transform.position = Vector3.Lerp(targetPos, backPos, t);
            cam.orthographicSize = Mathf.Lerp(zoomedSize, defaultSize, t);
            yield return null;
        }

        isFocusing = false;
    }

    public void ShowDragPreview(Vector3 dragOffset)
    {
        if (isFocusing) return;

        float distance = dragOffset.magnitude;

        // 👇 더 부드럽고 자연스러운 줌아웃/줌인 범위 설정
        float targetSize = Mathf.Clamp(defaultSize - distance * 0.3f, defaultSize, zoomedSize);

        // 👇 Time.unscaledDeltaTime 대신 Time.deltaTime (입력에 반응하는 것이므로 시간 축소 영향 받게)
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize * 1.5f, Time.deltaTime * 5f);

        // 👇 이동도 부드럽게 처리
        Vector3 targetPos = defaultTarget.position - dragOffset * 0.3f;
        targetPos.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
    }


    public void ResetToDefaultView()
    {
        if (isFocusing) return;

        StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        float t = 0f;
        float currentSize = cam.orthographicSize;
        Vector3 currentPos = transform.position;

        Vector3 targetPos = defaultTarget.position;
        targetPos.z = currentPos.z;

        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            cam.orthographicSize = Mathf.Lerp(currentSize, defaultSize, t);
            transform.position = Vector3.Lerp(currentPos, targetPos, t);
            yield return null;
        }
    }

}
