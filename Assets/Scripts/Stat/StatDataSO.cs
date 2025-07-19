using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    MaxHealth,        // 최대 체력
    CurrentHealth,    // 현재 체력
    AttackPower,      // 공격력
    InitialSpeed,     // 초기 이동 속도
    Weight,           // 무게
    Barrier,
    CriticalChance,    // 크리티컬 확률
    Size,
}

public class AttributeData
{
    public float BaseValue;
    public float CurrentValue;
    public float AddValue;
    public float MulValue;
    public float OverrideValue;
    public bool bOverride;
    
    public AttributeData()
    {
        BaseValue = 0f;
        CurrentValue = 0f;
        AddValue = 0f;
        MulValue = 1f;
        OverrideValue = 0f;
        bOverride = false;
    }

    public AttributeData(float initialValue)
    {
        BaseValue = initialValue;
        CurrentValue = initialValue;
        AddValue = 0f;
        MulValue = 1f;
        OverrideValue = 0f;
        bOverride = false;
    }
}

[Serializable]
public class StatModifier
{
    public StatType eStatType;
    public float value;
}
[CreateAssetMenu(fileName = "New StatData", menuName = "Stats/StatData")]
public class StatDataSO : ScriptableObject
{
    public List<StatModifier> stats = new List<StatModifier>();

    public float GetStatValue(StatType type)
    {
        foreach (var stat in stats)
        {
            if (stat.eStatType == type)
                return stat.value;
        }

        return 0f;
    }
}
