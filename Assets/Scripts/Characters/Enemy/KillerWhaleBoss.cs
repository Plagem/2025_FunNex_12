using System.Collections;
using UnityEngine;

public class KillerWhaleBoss : EnemyBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Skill Settings")]
    public float projectileDamage = 10f;
    public float angleInterval = 20f;
    public float shootInterval = 0.7f;

    private Animator animator;

    public float skillDelay = 1.5f;

    private bool isShooting = false;

    public bool phase1 = true;
    public bool phase2 = false;
    public bool phase3 = false;

    override protected void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();

        target = null;

        // 초기화
        phase1 = true;
        phase2 = false;
        phase3 = false;

        StartCoroutine(MainPatternLoop());
    }

    private IEnumerator MainPatternLoop()
    {
        while (true)
        {
            yield return Pattern1();
            yield return new WaitForSeconds(skillDelay);

            yield return Pattern2();
            yield return new WaitForSeconds(skillDelay);

            if (phase2)
            {
                yield return Pattern3();
                yield return new WaitForSeconds(skillDelay);
            }

            if(phase3)
            {
                yield return Pattern4();
                yield return new WaitForSeconds(skillDelay);
            }
        }
    }

    public void CheckPhaseTransition(float currentHealth, float maxHealth)
    {
        float hpPercent = currentHealth / maxHealth;

        if (!phase2 && hpPercent <= 0.66f)
        {
            phase2 = true;
            Debug.Log("Phase 2 진입");
            OnEnterPhase2();
        }

        if (!phase3 && hpPercent <= 0.33f)
        {
            phase3 = true;
            Debug.Log("Phase 3 진입");
            OnEnterPhase3();
        }
    }

    private void OnEnterPhase2()
    {
        // ex: speed 증가, animation 변경 등
    }

    private void OnEnterPhase3()
    {
        // ex: 더 빠르게, 더 강한 패턴
    }

    private IEnumerator Pattern1()
    {
        isShooting = true;

        // 위치 이동 + 보이기
        transform.position = new Vector3(0f, 11.5f, -1f);
        Show();

        for (float angle = 180f; angle <= 360f; angle += angleInterval)
        {
            ShootAtAngle(angle);
            yield return new WaitForSeconds(shootInterval);
        }

        Hide();
        isShooting = false;
    }


    private IEnumerator Pattern2()
    {
        isShooting = true;

        transform.position = new Vector3(0f, 11.5f, -1f); // 위치 유지 or 바꾸기
        Show();

        for (int i = 0; i < 5; i++)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            ShootInDirection(randomDir);
            yield return new WaitForSeconds(shootInterval);
        }

        Hide();
        isShooting = false;
    }

    private IEnumerator Pattern3()
    {
        isShooting = true;

        Show();

        for (float angle = 0; angle < 360f; angle += angleInterval / 2f)
        {
            ShootAtAngle(angle);
        }
        yield return new WaitForSeconds(shootInterval * 2f);

        Hide();
        isShooting = false;
    }

    private IEnumerator Pattern4()
    {
        isShooting = true;
        Show();

        for (float angle = 0; angle < 360f; angle += angleInterval / 2f)
        {
            ShootAtAngle(angle);
        }
        yield return new WaitForSeconds(shootInterval * 2f);

        Hide();
        isShooting = false;
    }

    private void Show()
    {
        var renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers)
            r.enabled = true;
    }

    private void Hide()
    {
        var renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers)
            r.enabled = false;
    }

    private void ShootAtAngle(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        ShootInDirection(dir);
    }

    private void ShootInDirection(Vector2 dir)
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // 방향에 맞게 회전 설정
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        var bulletScript = bullet.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(dir);
            bulletScript.SetDamage(projectileDamage);
        }
    }
}
