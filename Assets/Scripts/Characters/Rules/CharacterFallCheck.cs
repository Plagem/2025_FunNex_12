using UnityEngine;

public class CharacterFallCheck : MonoBehaviour
{
    public LayerMask terrainLayer;
    public float checkMargin = 0.05f;

    private Collider2D parentCollider;

    private void Start()
    {
        parentCollider = GetComponentInParent<Collider2D>();
        if (parentCollider == null)
        {
            Debug.LogError("부모에 Collider2D가 없습니다!");
        }
    }

    private void Update()
    {
        if (parentCollider == null) return;

        Bounds bounds = parentCollider.bounds;

        // 하단에서 살짝 아래로 박스 겹침 검사
        Vector2 checkCenter = new Vector2(bounds.center.x, bounds.min.y - checkMargin);
        Vector2 checkSize = new Vector2(bounds.size.x * 0.9f, checkMargin * 2f); // 가로는 살짝 줄임

        Collider2D hit = Physics2D.OverlapBox(checkCenter, checkSize, 0f, terrainLayer);

        /*
        if (hit == null)
        {
            Debug.Log("지형 벗어남 - 떨어졌다고 판단");
        }
        */
    }

    // 시각화용
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
