using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;        
    public Transform target;            
    public float shootInterval = 5f;

    private float timer;

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

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Vector2 direction = target.position - firePoint.position;
        bullet.GetComponent<EnemyBullet>().SetDirection(direction);

        Debug.Log("적이 총알 발사!");
    }
}
