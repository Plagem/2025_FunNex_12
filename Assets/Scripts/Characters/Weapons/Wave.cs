using UnityEngine;

public class WaveProjectile : EnemyBullet
{
    protected override void Start()
    {
        base.Start();
        lifeTime = 10f;
        damage = 30;

        transform.rotation = Quaternion.identity;

        // 아래 방향 고정
        SetDirection(Vector2.down);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WaveProjectile - 플레이어 피격됨!");

            var playerStat = other.GetComponent<BaseStatComponent>();
            if (playerStat != null)
            {
                playerStat.ApplyDamage(GetDamage()); // EnemyBullet에 protected으로 접근
            }

            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.AddForce(Vector2.down * knockbackForce, ForceMode2D.Impulse); // 아래로 넉백
            }
        }
    }
}
