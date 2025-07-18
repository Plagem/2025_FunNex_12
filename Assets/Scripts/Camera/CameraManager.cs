using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public Transform defaultTarget;          // ��� ���� Ÿ�� (�÷��̾�)
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

        // 1. ī�޶� ����
        CameraShake shake = GetComponent<CameraShake>();
        if (shake != null)
            shake.Shake(0.15f, 0.4f);  // ����, ���ӽð��� ���� ����

        yield return new WaitForSecondsRealtime(0.15f);  // ��鸲 ���

        // 2. �ð� ������
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;  // ���� ������Ʈ�� ������

        // 3. ���� & �̵�
        float t = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = enemy.position;
        targetPos.z = startPos.z;

        float startSize = cam.orthographicSize;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / 0.5f;  // Time.timeScale ���� �� �ް�
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            cam.orthographicSize = Mathf.Lerp(startSize, zoomedSize, t);
            yield return null;
        }

        // 4. ���� ���� ����
        yield return new WaitForSecondsRealtime(focusDuration);

        // 5. �ð� ����
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // 6. �ܾƿ� & ����
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

}
