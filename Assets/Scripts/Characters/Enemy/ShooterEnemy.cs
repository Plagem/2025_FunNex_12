using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform target;
    public float shootInterval = 5f;
    public float recoilForce = 2f; // 반동 세기

    private float timer;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= shootInterval)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        if (target == null) return;

        // 총알 발사
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (target.position - firePoint.position).normalized;
        bullet.GetComponent<EnemyBullet>().SetDirection(direction);

        // 반동 적용 (발사 방향의 반대 방향)
        rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);

        Debug.Log("적이 총알 발사!");
    }
}
