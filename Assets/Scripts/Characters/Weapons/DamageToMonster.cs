using UnityEngine;

public class DamageToMonster : MonoBehaviour
{
    public float damage = 50f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            Debug.Log("보스와 충돌하여 데미지를 줌");

            BaseStatComponent bossStat = other.GetComponent<BaseStatComponent>();
            if (bossStat != null)
            {
                bossStat.ApplyDamage(damage);
            }

            // (선택) 데미지 이펙트 표시
            if (DamageTextManager.Instance != null)
            {
                DamageTextManager.Instance.SpawnStarEffects(transform.position);
            }

            Destroy(gameObject);
        }
    }
}
