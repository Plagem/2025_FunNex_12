using System.Collections;
using UnityEngine;

public class CharacterFallCheck : MonoBehaviour
{
    public LayerMask terrainLayer;
    public float checkMargin = 0.05f;

    private Collider2D parentCollider;
    private Transform parentTransform;
    private BaseStatComponent statComponent;

    private bool hasFallen = false; // 낙사 판정 1회 처리용

    private void Start()
    {
        parentCollider = GetComponentInParent<Collider2D>();
        parentTransform = GetComponentInParent<Transform>();
        statComponent = GetComponentInParent<BaseStatComponent>();

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

            GameObject obj = parentCollider.gameObject;

            if (obj.CompareTag("Player"))
            {
                Debug.Log("플레이어 낙사 → 위치 초기화 + 체력 절반 깎기");

                parentTransform.position = new Vector3(0, 0, -1f);

                if (statComponent != null)
                {
                    float currentHP = statComponent.GetCurrentValue(StatType.CurrentHealth);
                    statComponent.ApplyDamage(currentHP * 0.5f);
                }

                ResetFallState();
            }
            else
            {
                Debug.Log($"{obj.name} 낙사 → 파괴");
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
        yield return null; // 1프레임 대기

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
