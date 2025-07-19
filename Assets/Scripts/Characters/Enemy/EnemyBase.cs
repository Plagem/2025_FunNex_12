using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyBase : MonoBehaviour
{
    public BaseStatComponent stat;
    public Transform target;

    protected Rigidbody2D rb;
    protected bool isDead = false; // 사망 상태 확인용

    public BaseStatComponent GetStatComponent()
    {
        return stat;
    }

    protected virtual void Start()
    {
        stat = GetComponent<BaseStatComponent>();
        rb = GetComponent<Rigidbody2D>();

        // target이 비어 있으면 자동으로 Player 찾아 설정
        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
            else
            {
                Debug.LogWarning($"{name}: Player 태그 오브젝트를 찾을 수 없습니다.");
            }
        }
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


    public virtual void Die()
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
        Destroy(gameObject, 1f);
        GetComponent<Collider2D>().enabled = false;
    }
}
