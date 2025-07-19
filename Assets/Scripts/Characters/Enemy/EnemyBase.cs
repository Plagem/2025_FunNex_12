using System.Collections;
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

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // 남은 EnemyBase가 자기 자신뿐이라면 연출하고 죽기
        EnemyBase[] enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);

        if (enemies.Length == 1)
        {
            StartCoroutine(DieRoutine()); // 연출
        }
        else
        {
            // 그냥 즉시 제거
            GetComponent<Collider2D>().enabled = false;

            PlayerController pc = FindAnyObjectByType<PlayerController>();
            pc.OnMonsterEliminated?.Invoke();

            Destroy(gameObject);
        }
    }

    private IEnumerator DieRoutine()
    {
        // 카메라 연출 먼저 수행
        yield return StartCoroutine(Camera.main.GetComponent<CameraManager>().FocusRoutine(transform));

        // 콜라이더 비활성화
        GetComponent<Collider2D>().enabled = false;

        // 쓰레기 코드 실행
        PlayerController pc = FindAnyObjectByType<PlayerController>();
        pc.OnMonsterEliminated?.Invoke();

        // 오브젝트 파괴
        Destroy(gameObject);
    }

}
