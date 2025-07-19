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
}