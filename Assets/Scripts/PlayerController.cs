using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Vector3 startMousePos;
    private Vector3 endMousePos;
    private Rigidbody rb;
    private bool isDragging = false;
    private bool isMoving = false;

    private BaseStatComponent _statComponent;
    
    public TrajectoryLine tl;

    public float forceMultiplier = 5f;

    void Start()
    {
        _statComponent = GetComponent<BaseStatComponent>();
        
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        tl = GetComponent<TrajectoryLine>();
    }

    void Update()
    {
        if (isMoving) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            mouse.z = Mathf.Abs(Camera.main.transform.position.y);  // Y�� ���̴ϱ�
            startMousePos = Camera.main.ScreenToWorldPoint(mouse);
            isDragging = true;
        }

        if (isDragging)
        {
            Vector3 mouse = Input.mousePosition;
            mouse.z = Mathf.Abs(Camera.main.transform.position.y);
            Vector3 currentPoint = Camera.main.ScreenToWorldPoint(mouse);

            // ���� �ǽð� ǥ�� (XZ ���)
            currentPoint.y = 0.1f;
            startMousePos.y = 0.1f;
            tl.RenderLine(startMousePos, currentPoint);
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            Vector3 mouse = Input.mousePosition;
            mouse.z = Mathf.Abs(Camera.main.transform.position.y);
            endMousePos = Camera.main.ScreenToWorldPoint(mouse);

            Vector3 direction = (startMousePos - endMousePos).normalized;
            float distance = Vector3.Distance(startMousePos, endMousePos);

            rb.linearVelocity = Vector3.zero;
            rb.AddForce(direction * distance * forceMultiplier, ForceMode.Impulse);

            Vector3 torqueAxis = Vector3.forward;
            rb.AddTorque(torqueAxis * distance * 100 * forceMultiplier, ForceMode.Impulse);

            tl.EndLine();
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
            BaseStatComponent enemyStatComponent = collision.gameObject.GetComponent<BaseStatComponent>();
            if (enemyStatComponent)
            {
                DamageEffect damageEffect = new DamageEffect();
                damageEffect.Initialize(_statComponent.GetCurrentValue(StatType.AttackPower));
                enemyStatComponent.ApplyEffect(damageEffect);
                Debug.Log($"플레이어와 적이 부딪혔다! 적 체력 : {enemyStatComponent.GetCurrentValue(StatType.CurrentHealth)}");
            }
        }
    }

}
