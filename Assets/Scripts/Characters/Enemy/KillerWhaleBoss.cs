using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class KillerWhaleBoss : EnemyBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private GameObject[] summonedMonsterPrefabs;
    [SerializeField] private int summonCount = 4;               // 한번에 소환할 수
    [SerializeField] private float summonRadius = 5f;           // 소환 위치 범위

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

        phase1 = true;
        phase2 = false;
        phase3 = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        StartCoroutine(MainPatternLoop());
    }

    private IEnumerator MainPatternLoop()
    {
        while (true)
        {
            if (phase3) // Phase 3 이상
            {
                yield return Pattern3();
                yield return new WaitForSeconds(skillDelay);

                yield return Pattern4();  // phase3 내 조건은 Pattern4 내부에서 처리
                yield return new WaitForSeconds(skillDelay);
            }
            else // Phase 1 또는 2
            {
                yield return Pattern1();
                yield return new WaitForSeconds(skillDelay);

                yield return Pattern2();
                yield return new WaitForSeconds(skillDelay);

                yield return Pattern4();  // phase1,2 조건은 Pattern4 내부에서 처리
                yield return new WaitForSeconds(skillDelay);
            }
        }
    }


    private void SummonAtAngle(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * summonRadius;
        Vector3 spawnPos = firePoint.position + (Vector3)offset;

        SummonMonsterAt(spawnPos);
    }

    private void SummonMonsterAt(Vector3 position)
    {
        if (summonedMonsterPrefabs == null || summonedMonsterPrefabs.Length == 0)
        {
            Debug.LogWarning("소환할 몬스터 프리팹이 없습니다.");
            return;
        }

        GameObject prefabToSummon = summonedMonsterPrefabs[Random.Range(0, summonedMonsterPrefabs.Length)];
        Instantiate(prefabToSummon, position, Quaternion.identity);
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
        phase1 = false;
        phase2 = false;
        phase3 = true;

        // Rigidbody2D 설정 변경
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.mass = 5f;
            rb.linearDamping = 3f;
            rb.angularDamping = 3f;
        }
    }

    private void PlayAnim(string animName)
    {
        if (animator != null)
            animator.Play(animName);
    }


    private IEnumerator Pattern1()
    {
        isShooting = true;

        float localAngleInterval = angleInterval;
        float localShootInterval = shootInterval;

        if (phase3)
        {
            localAngleInterval = 10f;
            localShootInterval = 0.3f;
        }
        else if (phase2)
        {
            localAngleInterval = 15f;
            localShootInterval = 0.4f;
        }
        else
        {
            localAngleInterval = 18f;
            localShootInterval = 0.6f;
        }

        transform.position = new Vector3(0f, 13.5f, -1f);
        PlayAnim("Rise");
        yield return new WaitForSeconds(0.5f);
        Show();

        PlayAnim("Spit");
        for (float angle = 180f; angle <= 360f; angle += localAngleInterval)
        {
            SoundManager.Instance.Play(Define.SFX.Big_Fire);
            ShootAtAngle(angle);
            yield return new WaitForSeconds(localShootInterval);
        }

        PlayAnim("Dive");
        yield return new WaitForSeconds(0.5f);
        Hide();
        isShooting = false;
    }


    [SerializeField] private GameObject wavePrefab; // 추가
    [SerializeField] private float waveSpeed = 6f;   // 내려가는 속도
    [SerializeField] private GameObject urchinPrefab;     // 성게 프리팹
    [SerializeField] private float urchinSpawnDelay = 1.0f; // 성게 간격 (초)


    private IEnumerator Pattern2()
    {
        SoundManager.Instance.Play(Define.SFX.Boss_Aqua);
        
        isShooting = true;

        int waveCount = 1;
        float interval = 0.5f;

        if (phase3)
        {
            waveCount = 3;
        }
        else if (phase2)
        {
            waveCount = 2;
        }

        Show();

        for (int i = 0; i < waveCount; i++)
        {
            float randomX = Random.Range(-11f, 12.5f);
            Vector3 spawnPos = new Vector3(randomX, 17.5f, -1f);
            transform.position = spawnPos;

            PlayAnim("Rise");
            yield return new WaitForSeconds(0.5f);
            Show();

            PlayAnim("Idle");

            yield return new WaitForSeconds(1.0f);

            GameObject wave = Instantiate(wavePrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = wave.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.down * waveSpeed;

            StartCoroutine(GameManager.instance.ShowInfoBox());
            // 🟣 성게 뿌리기 (wave 지나간 위치에)
            StartCoroutine(SpawnUrchinsAlongWave(randomX, 1 + Random.Range(0, 2))); // 1 or 2개

            yield return new WaitForSeconds(interval * 2); // 다음 웨이브까지 대기
        }

        yield return new WaitForSeconds(1f); // 마지막 웨이브 보여줄 시간

        PlayAnim("Dive");
        yield return new WaitForSeconds(0.5f);
        Hide();

        Hide();
        isShooting = false;
    }
    
    private IEnumerator SpawnUrchinsAlongWave(float x, int count)
    {
        float startY = 13.5f;
        float endY = -5f;

        for (int i = 0; i < count; i++)
        {
            float t = Random.Range(0.2f, 0.8f); // 지나가는 중간 지점
            float spawnY = Mathf.Lerp(startY, endY, t);

            Vector3 spawnPos = new Vector3(x, spawnY, -1f);
            Instantiate(urchinPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(urchinSpawnDelay); // 살짝 시간차로 뿌릴 경우
        }
    }

    private bool isCharging = false;
    private bool hasHitPlayerThisCharge = false;

    private bool PlayerIsFalling()
    {
        var player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        return player != null && player.isFalling;
    }

    private IEnumerator Pattern3()
    {
        Show();
        Debug.Log("Phase 3 발동");
        isShooting = true;

        float waitBetweenCharges = 2f;
        int chargeCount = 3;
        float dashForce = 30f;
        float dashDuration = 1.0f;

        rb.linearVelocity = Vector2.zero;

        for (int i = 0; i < chargeCount; i++)
        {
            hasHitPlayerThisCharge = false;
            isCharging = true;

            if (target == null)
                target = GameObject.FindGameObjectWithTag("Player")?.transform;

            // 🔹 낙사 체크
            if (PlayerIsFalling())
            {
                Debug.Log("플레이어 낙사 중 - 돌진 중단");
                rb.linearVelocity = Vector2.zero;
                isCharging = false;
                break;
            }

            if (target != null)
            {
                Vector2 dir = (target.position - transform.position).normalized;

                // 돌진 시작
                PlayAnim("Spit");
                SoundManager.Instance.Play(Define.SFX.Dash);
                rb.linearVelocity = dir * dashForce;

                yield return new WaitForSeconds(dashDuration);

                rb.linearVelocity = Vector2.zero;
                PlayAnim("Idle");
            }

            isCharging = false;
            yield return new WaitForSeconds(waitBetweenCharges);
        }

        yield return new WaitForSeconds(5f);
        isShooting = false;
    }


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (this is KillerWhaleBoss boss)
            {
                if (boss.isCharging && boss.hasHitPlayerThisCharge)
                    return; // 이미 맞혔으면 무시

                if (boss.isCharging)
                    boss.hasHitPlayerThisCharge = true;

                if (boss.isCharging)
                {
                    var playerStat = other.GetComponent<BaseStatComponent>();
                    if (playerStat != null)
                    {
                        float damage = stat.GetFinalDamage();
                        playerStat.ApplyDamage(damage);
                    }

                    Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
                    if (playerRb != null)
                    {
                        Vector3 direction = (other.transform.position - transform.position).normalized;
                        playerRb.AddForce(-direction * 30f, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }

    private IEnumerator Pattern4()
    {
        isShooting = true;
        transform.position = new Vector3(0f, 13.5f, -1f);

        if (!phase3)  // Phase 1,2 에서는 등장/퇴장 연출 있음
        {
            PlayAnim("Rise");
            yield return new WaitForSeconds(0.5f);
            Show();

            PlayAnim("Spit");

            yield return StartCoroutine(SummonEnemiesInWaves());

            yield return new WaitForSeconds(1f);

            PlayAnim("Dive");
            yield return new WaitForSeconds(0.5f);
            Hide();
        }
        else  // Phase 3에서는 등장/퇴장 없이 바로 소환
        {
            PlayAnim("Spit");

            yield return StartCoroutine(SummonEnemiesInWaves());

            yield return new WaitForSeconds(1f);
        }

        isShooting = false;
    }

    private IEnumerator SummonEnemiesInWaves()
    {
        int maxExistingCount = 3;
        if (phase2) maxExistingCount = 4;
        if (phase3) maxExistingCount = 5;

        int totalSummon = maxExistingCount; // 소환 시도 수도 제한
        int summonPerWave = 2;
        float waveDelay = 4f;

        float startAngle = 200f;
        float endAngle = 340f;

        for (int i = 0; i < totalSummon; i++)
        {
            // 현재 존재하는 소환몹 수 확인
            int currentSummons = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (currentSummons >= maxExistingCount)
            {
                Debug.Log("현재 최대 몬스터 수 도달: 소환 스킵");
                break;
            }

            float angle = Random.Range(startAngle, endAngle);
            SummonAtAngle(angle);

            if ((i + 1) % summonPerWave == 0 && (i + 1) < totalSummon)
                yield return new WaitForSeconds(waveDelay);
        }
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