using UnityEngine;

public class DashEnemy : EnemyBase
{
    public float dashForce = 10f;
    public float dashInterval = 3f;
    public float dashDuration = 0.5f;

    private float timer = 0f;
    private bool isDashing = false;
    private Vector2 dashDirection;
    
    protected override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (!isDashing && timer >= dashInterval && !isFalling)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        if (target == null) return;

        // 플레이어가 낙사 중이면 대시하지 않음
        var player = target.GetComponent<PlayerController>();
        if (player != null && player.isFalling)
            return;

        dashDirection = (target.position - transform.position).normalized;
        rb.linearVelocity = dashDirection * dashForce;
        isDashing = true;
        timer = 0f;

        Invoke(nameof(EndDash), dashDuration);
    }


    void EndDash()
    {
        isDashing = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isDashing) 
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
                    playerRb.AddForce(-direction * 10f, ForceMode2D.Impulse);
                }
            }
        }
    }
}
