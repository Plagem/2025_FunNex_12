using System;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;

public class Satellite : MonoBehaviour
{
    public Transform orbitCenter;
    public float initialAngle = 0f;
    public float radius = 1.5f;
    public float rotationDuration = 2f;

    private Tween orbitTween;

    public void BeginOrbit()
    {
        float angle = initialAngle;

        orbitTween = DOVirtual.Float(0f, 360f, rotationDuration, t =>
            {
                float totalAngle = angle + t;
                float rad = totalAngle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

                if (orbitCenter != null)
                {
                    Vector3 orbitPos = orbitCenter.position + offset;
                    transform.position = orbitPos;

                    // 방향 벡터 계산 (중심 → 객체 방향)
                    Vector3 dir = (orbitPos - orbitCenter.position).normalized;
                    float angleDeg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    // 본인 회전 (Z축 기준)
                    transform.rotation = Quaternion.Euler(0, 0, angleDeg);
                }
            })
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    public void OnSkillUpdated()
    {
        orbitTween?.Kill();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            BaseStatComponent ownerStat = orbitCenter.GetComponent<BaseStatComponent>();
            BaseStatComponent targetStat = other.GetComponent<BaseStatComponent>();
            if (targetStat && ownerStat)
            {
                float playerDamage = ownerStat.GetFinalDamage();
                targetStat.ApplyDamage(playerDamage * 1f);
                Vector3 vec = (other.transform.position - orbitCenter.position).normalized;
                other.GetComponent<Rigidbody2D>().AddForce(vec  * 5f, ForceMode2D.Impulse);
            }
        }
        
    }
}