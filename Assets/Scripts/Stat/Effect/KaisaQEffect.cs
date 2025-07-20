using System.Collections.Generic;
using UnityEngine;

public class KaisaQEffect : BaseEffect
{
    private readonly string _projectilePath = "Prefabs/HomingMissile";
        
    private GameObject _projectilePrefab;
    private Transform _originTransform;
    private int _projectileCount;
    private float _spreadAngle;
    private float _baseDistance;
    private float _targetingRadius;

    public KaisaQEffect(
        Transform originTransform,
        int projectileCount,
        float spreadAngle,
        float baseDistance,
        float targetingRadius)
    {
        _originTransform = originTransform;
        _projectileCount = projectileCount;
        _spreadAngle = spreadAngle;
        _baseDistance = baseDistance;
        _targetingRadius = targetingRadius;
        
        LoadProjectilePrefab();
    }
    
    private void LoadProjectilePrefab()
    {
        _projectilePrefab = Resources.Load<GameObject>(_projectilePath);
        if (_projectilePrefab == null)
        {
            Debug.LogError($"[KaisaQEffect] Failed to load projectile prefab from Resources/{_projectilePath}");
        }
    }

    public override void OnEffectApplied()
    {
        List<Transform> targets = FindTargets();

        Vector3 baseDir = -_originTransform.up;
        float angleStep = (_projectileCount > 1) ? _spreadAngle / (_projectileCount - 1) : 0f;

        for (int i = 0; i < _projectileCount; i++)
        {
            float angle = -_spreadAngle / 2f + angleStep * i;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 dir = rotation * baseDir;

            Vector3 spawnPos = _originTransform.position + dir * _baseDistance;
            GameObject proj = Object.Instantiate(_projectilePrefab, spawnPos, Quaternion.identity);

            HomingProjectile homing = proj.GetComponent<HomingProjectile>();
            Transform target = targets[i % targets.Count];

            PlayerController pc = _statComponent.GetComponent<PlayerController>();
            homing.Initialize(pc, target, dir);
        }
    }

    private List<Transform> FindTargets()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(_originTransform.position, _targetingRadius);
        List<Transform> targetList = new();

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Boss"))
            {
                targetList.Add(hit.transform);
            }
        }

        targetList.Sort((a, b) =>
            Vector2.Distance(_originTransform.position, a.position)
            .CompareTo(Vector2.Distance(_originTransform.position, b.position)));

        return targetList;
    }
}
