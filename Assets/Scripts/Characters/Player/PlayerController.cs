using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Sprite idleSprite;   // 원래 이미지
    [SerializeField] private Sprite dashSprite;   // 이동 중 이미지

    private SpriteRenderer spriteRenderer;

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
    public float forceMultiplier = 10f;

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

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 이동 중일 땐 멈췄는지만 체크하고, 아래 코드는 실행 안 함
        if (isMoving)
        {
            if (spriteRenderer != null)
                spriteRenderer.sprite = dashSprite;

            if (rb.linearVelocity.magnitude < 0.3f)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                isMoving = false;

                if (spriteRenderer != null)
                    spriteRenderer.sprite = idleSprite;

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
            Vector3 dir = (currentPoint - startMousePos).normalized;
            float dist = Mathf.Clamp((currentPoint - startMousePos).magnitude, 0f, 5f);
            tl.RenderLine(transform.position, transform.position + dir * dist);

            Vector3 dragOffset = startMousePos - currentPoint;
            cameraManager?.ShowDragPreview(dragOffset);
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            Vector3 mouse = Input.mousePosition;
            endMousePos = Camera.main.ScreenToWorldPoint(mouse);
            endMousePos.z = 0f;

            Vector2 direction = startMousePos - endMousePos;
            float distance = Mathf.Clamp(direction.magnitude, 0.3f, 5f);
            direction.Normalize();

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * distance * forceMultiplier * _statComponent.GetCurrentValue(StatType.Weight) * _statComponent.GetCurrentValue(StatType.InitialSpeed), ForceMode2D.Impulse);

            float torque = distance * 0.1f * forceMultiplier;
            rb.AddTorque(torque, ForceMode2D.Impulse);

            tl.EndLine();
            isDragging = false;
            isMoving = true;

            if (spriteRenderer != null)
                spriteRenderer.sprite = dashSprite;

            cameraManager?.ResetToDefaultView();
            SoundManager.Instance.Play(Define.SFX.Dash);
            OnAttackStarted?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("Enemy")) && isMoving)
        {
            SoundManager.Instance.Play(Define.SFX.Oct_Fire);
            float speed = rb.linearVelocity.magnitude;

            // Apply Damage
            Debug.Log("플레이어와 적이 부딪혔다! (플레이어가 감지함)");
            BaseStatComponent enemyStatComponent = collision.gameObject.GetComponent<BaseStatComponent>();
            if (enemyStatComponent)
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                float baseDamage = _statComponent.GetCurrentValue(StatType.AttackPower);

                // 1. 속도 기반 배수 (speed: 1 → 0.5배, 10+ → 2배)
                float speedMultiplier = Mathf.Lerp(0.5f, 1.5f, Mathf.InverseLerp(1f, 10f, speed));

                // 2. 질량 기반 배수 (mass: 1 → 1배, 5+ → 2배)
                float mass = rb.mass;
                float massMultiplier = Mathf.Lerp(1f, 2f, Mathf.InverseLerp(1f, 1.5f, mass));

                // 최종 데미지
                float finalDamage = baseDamage * speedMultiplier * massMultiplier;

                /*
                if(finalDamage >= 20)
                {
                    CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
                    if (cameraShake != null)
                    {
                        cameraShake.Shake();
                        Debug.Log("CameraShake 발동");
                    }
                }
                */

                // 데미지 이펙트 생성
                enemyStatComponent.ApplyDamage(finalDamage);
                
                DamageTextManager.Instance.SpawnStarEffects(collision.contacts[0].point);
            }
            
            // Combo Delegate
            _attackCombo++;
            OnComboTriggered?.Invoke(_attackCombo);
        }
    }

}
