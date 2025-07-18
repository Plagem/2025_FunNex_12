using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyBase : MonoBehaviour
{
    public BaseStatComponent stat;
    public Transform target;

    protected Rigidbody2D rb;
    protected bool isDead = false; // 사망 상태 확인용

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void FaceTarget()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        if (direction.sqrMagnitude < 0.0001f) return;

        transform.right = direction;
    }


    protected virtual void Update()
    {
        if (isDead) return;

        FaceTarget();

        if (stat.GetCurrentValue(StatType.CurrentHealth) <= 0)
        {
            Die();
        }
    }


    protected virtual void Die()
    {
        isDead = true;

        // 이동 정지
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic; // 물리 반응 끄기
        }

        // 카메라 연출
        Camera.main.GetComponent<CameraManager>().FocusOnEnemy(transform);

        // 쓰레기코드 투하 우하하 ㅋㅋ
        PlayerController pc = FindAnyObjectByType<PlayerController>();
        pc.OnMonsterEliminated?.Invoke();
        
        // 잠시 후 제거
        Destroy(gameObject, 0.3f);
    }
}
