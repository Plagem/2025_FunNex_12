using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Vector3 startMousePos;
    private Vector3 endMousePos;
    private Rigidbody rb;
    private bool isDragging = false;
    private bool isMoving = false;

    public float forceMultiplier = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        if (isMoving) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            mouse.z = Mathf.Abs(Camera.main.transform.position.z);
            startMousePos = Camera.main.ScreenToWorldPoint(mouse);
            isDragging = true;
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            Vector3 mouse = Input.mousePosition;
            mouse.z = Mathf.Abs(Camera.main.transform.position.z);
            endMousePos = Camera.main.ScreenToWorldPoint(mouse);

            Vector3 direction = (startMousePos - endMousePos).normalized;
            float distance = Vector3.Distance(startMousePos, endMousePos);

            rb.linearVelocity = Vector3.zero;
            rb.AddForce(direction * distance * forceMultiplier, ForceMode.Impulse);

            isDragging = false;
            isMoving = true;
        }

        if (isMoving && rb.linearVelocity.magnitude < 0.05f)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isMoving = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("플레이어와 적이 부딪혔다! (플레이어가 감지함)");
        }
    }

}
