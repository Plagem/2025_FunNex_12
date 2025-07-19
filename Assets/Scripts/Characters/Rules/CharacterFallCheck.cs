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
            StartCoroutine(FallRoutine());
        }
    }

    private IEnumerator FallRoutine()
    {
        GameObject obj = parentCollider.gameObject;

        // 애니메이션 (줄어들고 회전)
        float duration = 1.5f;
        float elapsed = 0f;

        Vector3 startScale = parentTransform.localScale;
        Quaternion startRot = parentTransform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            parentTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            parentTransform.Rotate(0, 0, 720f * Time.deltaTime); // 시계방향 회전
            yield return null;
        }

        // 낙사 처리
        if (obj.CompareTag("Player"))
        {
            Debug.Log("플레이어 낙사 → 위치 초기화 + 체력 절반 깎기");

            // 렌더러 숨김
            foreach (var r in renderers)
                r.enabled = false;

            // 위치 초기화
            parentTransform.position = new Vector3(0, 0, -1f);

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;

            // 체력 절반 감소
            if (statComponent != null)
            {
                float currentHP = statComponent.GetCurrentValue(StatType.CurrentHealth);
                statComponent.ApplyDamage(currentHP * 0.5f);
            }

            // 스케일 초기화
            parentTransform.localScale = startScale;
            parentTransform.rotation = startRot;

            // 렌더러 다시 보이기
            foreach (var r in renderers)
                r.enabled = true;

            ResetFallState();
        }
        else
        {
            if (obj.TryGetComponent<EnemyBase>(out var enemy))
            {
                enemy.Die();
            }
            else
            {
                Destroy(obj);
            }
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
