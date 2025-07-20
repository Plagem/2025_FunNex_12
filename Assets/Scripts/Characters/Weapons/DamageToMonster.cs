using UnityEngine;

public class DamageToMonster : MonoBehaviour
{
    public float damage = 150f;

    private void Start()
    {
        Destroy(gameObject, 15f); // 15초 후 자동 삭제
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            GameObject repelPrefab = Resources.Load<GameObject>("Prefabs/RepelEffect");
            Instantiate(repelPrefab, transform.position, Quaternion.identity);
            
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
