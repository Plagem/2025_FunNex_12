using System;
using UnityEngine;

public class RepelEffect : BaseEffect
{
    private float _repelDamage;
    private float _repelRange;
    private void Start()
    {
        EffectName = "Repel Effect";
        CanStack = false;
        DurationType = DurationType.Instance;
    }

    public void Initialize(float damage, float repelRange)
    {
        _repelDamage = damage;
        _repelRange = repelRange;
    }

    public override void OnEffectApplied()
    {
        base.OnEffectApplied();

        Transform origin = _statComponent.transform;
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(origin.position, _repelRange, LayerMask.GetMask("Enemy")); // Enemy 레이어 필터링

        foreach (var collider in hitColliders)
        {
            if (!collider.CompareTag("Enemy")) continue;
            
            Transform enemyTransform = collider.transform;
            Vector2 direction = (enemyTransform.position - origin.position).normalized;

            float distance = Vector2.Distance(origin.position, enemyTransform.position);
            float distanceFactor = 1f - Mathf.Clamp01(distance / _repelRange); // 1 (가까움) ~ 0 (가장자리)

            float repelForce = distanceFactor * 10f; // 힘 조절

            Rigidbody2D rb = collider.attachedRigidbody;
            if (rb != null)
            {
                rb.AddForce(direction * repelForce, ForceMode2D.Impulse);
            }

            var enemyStatComponent = collider.GetComponent<BaseStatComponent>();
            if (enemyStatComponent)
            {
                DamageEffect damageEffect = new DamageEffect();
                damageEffect.Initialize(_statComponent.GetCurrentValue(StatType.AttackPower));
                enemyStatComponent.ApplyEffect(damageEffect);
            }
        }

        Debug.Log($"[RepelEffect] {_repelRange} 범위 내 적 밀침 + {_repelDamage} 데미지 적용");
    }
}
