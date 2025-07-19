using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 5f;
    public float knockbackForce = 10f;

    protected Vector2 moveDirection;
    public float damage;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    protected float GetDamage()
    {
        return damage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("EnemyBullet - 플레이어 피격됨!");

            var playerStat = other.GetComponent<BaseStatComponent>();
            if (playerStat != null)
            {
                playerStat.ApplyDamage(damage);
            }

            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.AddForce(moveDirection * knockbackForce, ForceMode2D.Impulse);
            }

            Destroy(gameObject);
        }
    }
}
