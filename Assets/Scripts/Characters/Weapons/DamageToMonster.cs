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
            GameObject onePrefab = Resources.Load<GameObject>("Prefabs/ShellEffect");
            Instantiate(onePrefab, transform.position, Quaternion.identity);
            GameObject twoPrefab = Resources.Load<GameObject>("Prefabs/ShellBreakEffect");
            Instantiate(twoPrefab, transform.position, Quaternion.identity);
            
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
