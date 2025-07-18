using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyBase : MonoBehaviour
{
    public BaseStatComponent stat;
    public Transform target;

    protected Rigidbody2D rb;
    protected bool isDead = false; // ��� ���� Ȯ�ο�

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (isDead) return;

        if (stat.GetCurrentValue(StatType.CurrentHealth) <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;

        // �̵� ����
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic; // ���� ���� ����
        }

        // ī�޶� ����
        Camera.main.GetComponent<CameraManager>().FocusOnEnemy(transform);

        // ��� �� ����
        Destroy(gameObject, 0.3f);
    }
}
