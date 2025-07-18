using UnityEngine;

public class DashEnemy : EnemyBase
{
    public float dashForce = 10f;
    public float dashInterval = 3f;
    public float dashDuration = 0.2f;

    private float timer = 0f;
    private bool isDashing = false;
    private Vector2 dashDirection;

    protected override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (!isDashing && timer >= dashInterval)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        if (target == null) return;

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
}
