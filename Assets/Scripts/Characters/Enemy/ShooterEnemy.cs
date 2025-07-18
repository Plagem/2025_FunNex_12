using UnityEngine;

public class EnemyShooter : EnemyBase
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval = 5f;
    public float recoilForce = 2f;

    private float timer;

    protected override void Update()
    {
        base.Update();

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

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (target.position - firePoint.position).normalized;
        bullet.GetComponent<EnemyBullet>().SetDirection(direction);

        rb.AddForce(-direction * recoilForce, ForceMode2D.Impulse);

        Debug.Log("적이 총알 발사!");
    }
}
