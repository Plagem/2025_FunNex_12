using System;
using UnityEngine;
using DG.Tweening;

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
                    transform.position = orbitCenter.position + offset;
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