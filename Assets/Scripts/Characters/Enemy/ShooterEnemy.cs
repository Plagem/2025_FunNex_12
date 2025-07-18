using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform target;
    public float shootInterval = 5f;
    public float recoilForce = 2f; // �ݵ� ����

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

        // �Ѿ� �߻�
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (target.position - firePoint.position).normalized;
        bullet.GetComponent<EnemyBullet>().SetDirection(direction);

        // �ݵ� ���� (�߻� ������ �ݴ� ����)
        rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);

        Debug.Log("���� �Ѿ� �߻�!");
    }
}
