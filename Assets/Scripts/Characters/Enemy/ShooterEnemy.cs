using System.Collections;
using UnityEngine;

public class EnemyShooter : EnemyBase
{
    [Header("Combat Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval = 4f;
    public float recoilForce = 2f;

    [Header("Movement Settings")]
    public float moveSpeed = 0.7f;

    private Animator animator;
    private float timer = 0f;
    private bool isShooting = false;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (isDead) return;

        timer += Time.deltaTime;

        if (!isShooting && timer >= shootInterval)
        {
            StartCoroutine(ShootSequence());
            timer = 0f;
        }

        if (!isShooting)
        {
            Walk();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            SetWalking(false);
        }
    }

    void Walk()
    {
        if (target == null) return;

        FaceTarget();

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        SetWalking(true);
    }

    IEnumerator ShootSequence()
    {
        isShooting = true;
        rb.linearVelocity = Vector2.zero;

        SetWalking(false); // 애니메이션 파라미터 false로 설정

        yield return new WaitForSeconds(1.0f); // 발사 전 딜레이

        if (target == null)
        {
            isShooting = false;
            SetWalking(true);
            yield break;
        }

        FaceTarget();

        if (animator && !animator.GetCurrentAnimatorStateInfo(0).IsName("OctoShoot"))
        {
            animator.ResetTrigger("ShootTrigger");  // 중복 방지
            animator.SetTrigger("ShootTrigger");    // Shoot 애니메이션 단발 트리거
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (target.position - firePoint.position).normalized;
        bullet.GetComponent<EnemyBullet>().SetDirection(direction);
        rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.0f);

        SetWalking(true);  // Walk로 전환 조건 설정
        isShooting = false;
    }


    void SetWalking(bool value)
    {
        if (animator)
        {
            animator.SetBool("IsWalking", value);
        }
    }
}
