using System.Collections;
using UnityEngine;

public class CharacterFallCheck : MonoBehaviour
{
    public LayerMask terrainLayer;
    public float checkMargin = 0.05f;

    private Collider2D parentCollider;
    private Transform parentTransform;
    private BaseStatComponent statComponent;
    private SpriteRenderer[] renderers;

    private bool hasFallen = false;

    private void Start()
    {
        parentCollider = GetComponentInParent<Collider2D>();
        parentTransform = GetComponentInParent<Transform>();
        statComponent = GetComponentInParent<BaseStatComponent>();
        renderers = parentTransform.GetComponentsInChildren<SpriteRenderer>();

        if (parentCollider == null)
        {
            Debug.LogError("부모에 Collider2D가 없습니다!");
        }
    }

    private void Update()
    {
        if (hasFallen || parentCollider == null) return;

        Bounds bounds = parentCollider.bounds;
        Vector2 checkCenter = new Vector2(bounds.center.x, bounds.min.y - checkMargin);
        Vector2 checkSize = new Vector2(bounds.size.x * 0.9f, checkMargin * 2f);

        Collider2D hit = Physics2D.OverlapBox(checkCenter, checkSize, 0f, terrainLayer);

        if (hit == null)
        {
            hasFallen = true;
            SoundManager.Instance.Play(Define.SFX.Fall);
            StartCoroutine(FallRoutine());
        }
    }

    private IEnumerator FallRoutine()
    {
        GameObject obj = parentCollider.gameObject;

        float duration = 1.5f;
        float elapsed = 0f;

        Vector3 startScale = parentTransform.localScale;
        Quaternion startRot = parentTransform.rotation;

        if (obj.CompareTag("Player"))
        {
            Debug.Log("플레이어 낙사 → 위치 초기화 + 체력 절반 깎기");

            // 낙사 애니메이션
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                parentTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
                parentTransform.Rotate(0, 0, 720f * Time.deltaTime);
                yield return null;
            }

            // 렌더러 숨김
            foreach (var r in renderers)
                r.enabled = false;

            // 위치 초기화
            parentTransform.position = new Vector3(0, 0, -1f);

            // 속도 초기화
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;

            // 체력 반감
            if (statComponent != null)
            {
                float currentHP = statComponent.GetCurrentValue(StatType.CurrentHealth);
                statComponent.ApplyDamage(currentHP * 0.5f);
            }

            // 복원
            parentTransform.localScale = startScale;
            parentTransform.rotation = startRot;

            foreach (var r in renderers)
                r.enabled = true;

            ResetFallState();
        }
        else
        {
            // 마지막 적 여부 판단
            var allEnemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
            int aliveCount = 0;
            foreach (var e in allEnemies)
            {
                if (!e.isDead) aliveCount++;
            }
            bool isLastEnemy = aliveCount <= 1;

            if (isLastEnemy)
            {
                Debug.Log("마지막 적 낙사 → 줌인 연출 실행");
                var camManager = Camera.main.GetComponent<CameraManager>();
                if (camManager != null)
                    yield return StartCoroutine(camManager.FocusRoutine(parentTransform));
            }

            // Rigidbody 정지 + 물리 반응 차단
            Rigidbody2D rb = parentCollider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            if (obj.TryGetComponent<EnemyBase>(out var enemyBase))
            {
                enemyBase.isFalling = true;
            }

            // 낙사 애니메이션
            elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                parentTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
                parentTransform.Rotate(0, 0, 720f * Time.deltaTime);
                yield return null;
            }


            // 제거
            if (obj.TryGetComponent<EnemyBase>(out var enemy))
                enemy.Die();
            else
                Destroy(obj);
        }
    }


    private void ResetFallState()
    {
        StartCoroutine(ResetFallNextFrame());
    }

    private IEnumerator ResetFallNextFrame()
    {
        yield return null;
        hasFallen = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (parentCollider == null) return;

        Bounds bounds = parentCollider.bounds;
        Vector2 checkCenter = new Vector2(bounds.center.x, bounds.min.y - checkMargin);
        Vector2 checkSize = new Vector2(bounds.size.x * 0.9f, checkMargin * 2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(checkCenter, checkSize);
    }
}
