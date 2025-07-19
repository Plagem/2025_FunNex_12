using System;
using UnityEngine;

public class SatellitePassive : BasePassive
{
    private readonly string _satellitePath = "Prefabs/Satellite";

    private GameObject _satellitePrefab;
    
    public event Action OnSkillUpdated;

    public SatellitePassive(BaseStatComponent statComponent)
    {
        EffectName = "Satellite Passive";
        DurationType = DurationType.Infinite;
        CanStack = true;

        _playerController = statComponent.GetComponent<PlayerController>();
        
        LoadSatellitePrefab();
    }

    public override void OnEffectApplied()
    {
        base.OnEffectApplied();
        
        SpawnSatellites();
    }

    public override void OnEffectRemoved()
    {
        base.OnEffectRemoved();
        OnSkillUpdated?.Invoke();
    }

    private void LoadSatellitePrefab()
    {
        _satellitePrefab = Resources.Load<GameObject>(_satellitePath);
        if (_satellitePrefab == null)
        {
            Debug.LogError($"[SatelliteEffect] Failed to load projectile prefab from Resources/{_satellitePath}");
        }
    }
    void SpawnSatellites()
    {
        if (_satellitePrefab == null || _playerController == null)
            return;

        int count = Mathf.Clamp(SkillLevel, 1, 3); // 위성 개수는 1~3개
        float radius = 1.5f; // 위성 거리
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = angleStep * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

            Vector3 spawnPos = _playerController.transform.position + offset;
            GameObject satObj = UnityEngine.Object.Instantiate(_satellitePrefab, spawnPos, Quaternion.identity);

            Satellite satellite = satObj.GetComponent<Satellite>();
            if (satellite != null)
            {
                satellite.orbitCenter = _playerController.transform;
                satellite.initialAngle = angle;
                satellite.radius = radius;
                satellite.rotationDuration = 2f; // 회전 속도 설정
                satellite.BeginOrbit(); // 위성 동작 시작

                OnSkillUpdated += satellite.OnSkillUpdated;
            }
        }
    }
}
