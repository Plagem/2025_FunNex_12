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
    public float moveForce = 5f;
    public float maxSpeed = 1.2f;

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
    }

    private void FixedUpdate()
    {
        if (isDead || isShooting)
        {
            rb.linearVelocity = Vector2.zero;
            SetWalking(false);
            return;
        }

        Walk();
    }

    void Walk()
    {
        if (target == null) return;

        FaceTarget();

        Vector2 direction = (target.position - transform.position).normalized;

        // 힘을 통해 이동
        rb.AddForce(direction * moveForce, ForceMode2D.Force);

        SetWalking(true);
    }

    IEnumerator ShootSequence()
    {
        isShooting = true;
        rb.linearVelocity = Vector2.zero;
        SetWalking(false);

        yield return new WaitForSeconds(1.0f);

        if (target == null)
        {
            isShooting = false;
            SetWalking(true);
            yield break;
        }

        FaceTarget();

        if (animator && !animator.GetCurrentAnimatorStateInfo(0).IsName("OctoShoot"))
        {
            animator.ResetTrigger("ShootTrigger");
            animator.SetTrigger("ShootTrigger");
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Vector2 direction = (target.position - firePoint.position).normalized;
        bullet.GetComponent<EnemyBullet>().SetDirection(direction);

        // 데미지 넘기기
        float damage = stat.GetFinalDamage(); // AttackPower 등 내부 계산 포함
        bullet.GetComponent<EnemyBullet>().SetDamage(damage);

        rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.0f);

        isShooting = false;
        SetWalking(true);
    }

    void SetWalking(bool value)
    {
        if (animator)
        {
            animator.SetBool("IsWalking", value);
        }
    }
}
