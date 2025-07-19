using UnityEngine;

public class WaveProjectile : EnemyBullet
{
    private SpriteRenderer spriteRenderer;
    private float elapsed = 0f;

    protected override void Start()
    {
        base.Start();
        lifeTime = 8f;
        damage = 30;

        transform.rotation = Quaternion.identity;

        SetDirection(Vector2.down); // 아래 방향 고정

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        if (spriteRenderer != null)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / lifeTime);
            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;
        }

        if (elapsed >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WaveProjectile - 플레이어 피격됨!");

            var playerStat = other.GetComponent<BaseStatComponent>();
            if (playerStat != null)
            {
                playerStat.ApplyDamage(GetDamage());
            }

            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.AddForce(Vector2.down * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
