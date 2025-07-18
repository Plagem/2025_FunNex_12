using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerStopped;
    public event Action<int> OnComboTriggered;
    public event Action OnAttackStarted;
    
    private Vector3 startMousePos;
    private Vector3 endMousePos;
    private Rigidbody2D rb;
    private bool isDragging = false;
    private bool isMoving = false;

    private BaseStatComponent _statComponent;
    
    // Event ���ۿ� ����
    private int _attackCombo = 0;

    public TrajectoryLine tl;
    public float forceMultiplier = 5f;

    public BaseStatComponent GetStatComponent()
    {
        return _statComponent;
    }
    
    void Start()
    {
        _statComponent = GetComponent<BaseStatComponent>();
        rb = GetComponent<Rigidbody2D>();
        tl = GetComponent<TrajectoryLine>();
        
        //test
        KaisaQPassive kqp = new KaisaQPassive();
        kqp.Initialize(this);
        _statComponent.ApplyEffect(kqp);
    }

    void Update()
    {
        // �̵� ���� �� ��������� üũ�ϰ�, �Ʒ� �ڵ�� ���� �� ��
        if (isMoving)
        {
            if (rb.linearVelocity.magnitude < 0.05f)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                isMoving = false;
                OnPlayerStopped?.Invoke();
                _attackCombo = 0;
            }
            return;
        }

        // �� �߻� �Է� ó�� �κ�
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            startMousePos = Camera.main.ScreenToWorldPoint(mouse);
            startMousePos.z = 0f;
            isDragging = true;
        }

        if (isDragging)
        {
            Vector3 mouse = Input.mousePosition;
            Vector3 currentPoint = Camera.main.ScreenToWorldPoint(mouse);
            currentPoint.z = 0f;
            startMousePos.z = 0f;
            tl.RenderLine(startMousePos, currentPoint);
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            Vector3 mouse = Input.mousePosition;
            endMousePos = Camera.main.ScreenToWorldPoint(mouse);
            endMousePos.z = 0f;

            Vector2 direction = (startMousePos - endMousePos);
            float distance = direction.magnitude;
            direction.Normalize();

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * distance * forceMultiplier, ForceMode2D.Impulse);

            float torque = distance * 100 * forceMultiplier;
            rb.AddTorque(torque, ForceMode2D.Impulse);

            tl.EndLine();
            isDragging = false;
            isMoving = true;
            
            OnAttackStarted?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isMoving)
        {
            float speed = rb.linearVelocity.magnitude;
            Debug.Log($"�浹 �� �ӵ�: {speed}");

            if (speed >= 5f) // ���� ���� �ӵ�
            {
                CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
                if (cameraShake != null)
                {
                    cameraShake.Shake();
                }
            }

            // Apply Damage
            BaseStatComponent enemyStatComponent = collision.gameObject.GetComponent<BaseStatComponent>();
            if (enemyStatComponent)
            {
                DamageEffect damageEffect = new DamageEffect();
                damageEffect.Initialize(_statComponent.GetCurrentValue(StatType.AttackPower) * 5);
                enemyStatComponent.ApplyEffect(damageEffect);
            }
            
            // Combo Delegate
            _attackCombo++;
            OnComboTriggered?.Invoke(_attackCombo);
        }
    }

}
