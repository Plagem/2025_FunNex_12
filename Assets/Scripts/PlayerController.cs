using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Vector2 startMousePos;
    private Vector2 endMousePos;
    private Rigidbody2D rb;
    private bool isDragging = false;
    private bool isMoving = false;

    public float forceMultiplier = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isMoving) return;

        // 마우스 누름 시작
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startMousePos = new Vector2(mouseWorld.x, mouseWorld.y);
            isDragging = true;
        }

        // 마우스 뗌
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endMousePos = new Vector2(mouseWorld.x, mouseWorld.y);

            Vector2 direction = (startMousePos - endMousePos).normalized;
            float power = Vector2.Distance(startMousePos, endMousePos) * forceMultiplier;

            rb.AddForce(direction * power, ForceMode2D.Impulse);
            isDragging = false;
            isMoving = true;
        }

        // 움직임 멈췄는지 확인
        if (isMoving && rb.velocity.magnitude < 0.05f)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            isMoving = false;
        }
    }
}
