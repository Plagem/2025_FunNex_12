using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 5f;
    public float knockbackForce = 100f;

    private Vector2 moveDirection;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 피격됨!");

            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.AddForce(moveDirection * knockbackForce, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("No RB!");
            }

            Destroy(gameObject);
        }
    }
}
