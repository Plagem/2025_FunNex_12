using System;
using System.Collections.Generic;
using UnityEngine;

public enum eStatType
{
    MaxHealth,        // 최대 체력
    CurrentHealth,    // 현재 체력
    AttackPower,      // 공격력
    InitialSpeed,     // 초기 이동 속도
    Weight,           // 무게
    CriticalChance    // 크리티컬 확률
}

[Serializable]
public class StatModifier
{
    public eStatType eStatType;
    public float value;
}

public class StatDataSO : ScriptableObject
{
    public List<StatModifier> stats = new List<StatModifier>();

    public float GetStatValue(eStatType type)
    {
        foreach (var stat in stats)
        {
            if (stat.eStatType == type)
                return stat.value;
        }

        return 0f;
    }
}
