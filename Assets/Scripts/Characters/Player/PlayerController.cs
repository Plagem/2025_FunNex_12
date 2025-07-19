using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerStopped;
    public event Action<int> OnComboTriggered;
    public event Action OnAttackStarted;
    public Action OnMonsterEliminated;
    
    private Vector3 startMousePos;
    private Vector3 endMousePos;
    private Rigidbody2D rb;
    private bool isDragging = false;
    private bool isMoving = false;
    private CameraManager cameraManager;

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
        cameraManager = Camera.main.GetComponent<CameraManager>();

        //test
        _statComponent.BestowAugment(AugmentType.Trigger_KaisaQ);
        _statComponent.BestowAugment(AugmentType.Passive_Satellite);
        _statComponent.BestowAugment(AugmentType.Passive_Satellite);
        _statComponent.BestowAugment(AugmentType.Passive_Satellite);
    }

    void Update()
    {
        // 이동 중일 땐 멈췄는지만 체크하고, 아래 코드는 실행 안 함
        if (isMoving)
        {
            if (rb.linearVelocity.magnitude < 0.3f)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                isMoving = false;
                OnPlayerStopped?.Invoke();
                _attackCombo = 0;
            }
            return;
        }

        // ↓ 발사 입력 처리 부분
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

            Vector3 dragOffset = startMousePos - currentPoint;
            cameraManager?.ShowDragPreview(dragOffset);
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
            rb.AddForce(direction * distance * forceMultiplier * _statComponent.GetCurrentValue(StatType.Weight), ForceMode2D.Impulse);

            float torque = distance * 70 * forceMultiplier;
            rb.AddTorque(torque, ForceMode2D.Impulse);

            tl.EndLine();
            isDragging = false;
            isMoving = true;

            cameraManager?.ResetToDefaultView();

            OnAttackStarted?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isMoving)
        {
            float speed = rb.linearVelocity.magnitude;
            Debug.Log($"충돌 시 속도: {speed}");

            if (speed >= 12f) // 예시 기준 속도
            {
                CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
                if (cameraShake != null)
                {
                    cameraShake.Shake();
                }
            }

            // Apply Damage
            Debug.Log("플레이어와 적이 부딪혔다! (플레이어가 감지함)");
            BaseStatComponent enemyStatComponent = collision.gameObject.GetComponent<BaseStatComponent>();
            if (enemyStatComponent)
            {
                DamageEffect damageEffect = new DamageEffect();
                damageEffect.Initialize(_statComponent.GetCurrentValue(StatType.AttackPower));
                enemyStatComponent.ApplyEffect(damageEffect);
                Debug.Log($"적 남은 체력: {enemyStatComponent.GetCurrentValue(StatType.CurrentHealth)}");
            }
            
            // Combo Delegate
            _attackCombo++;
            OnComboTriggered?.Invoke(_attackCombo);
        }
    }

}
