using System;
using System.Collections.Generic;
using UnityEngine;

public enum AugmentType
{
    Trigger_KaisaQ,
    Trigger_Repel,
    Passive_Satellite,
    Stat_Weight,
    Stat_Health,
    Stat_Size,
    Stat_Speed,
    Stat_Power,
    MAX,
}

public static class EffectFactory
{
    public static BaseEffect CreateEffect(AugmentType type, BaseStatComponent statComp)
    {
        BaseEffect effect = null;

        ModifyInfo statModify;
        switch (type)
        {
            case AugmentType.Trigger_KaisaQ:
                effect = new KaisaQPassive(statComp);
                break;

            case AugmentType.Trigger_Repel:
                effect = new RepelPassive(statComp);
                break;
            case AugmentType.Stat_Weight:
                statModify = new ModifyInfo
                {
                    ModifyType = ModifyType.Add,
                    Magnitude = statComp.GetBaseValue(StatType.Weight) * 0.15f,
                    TargetStat = StatType.Weight
                };
                effect = new StatPassive(statComp, statModify);
                break;
            case AugmentType.Stat_Health:
                statModify = new ModifyInfo
                {
                    ModifyType = ModifyType.Add,
                    Magnitude = statComp.GetBaseValue(StatType.MaxHealth) * 0.1f,
                    TargetStat = StatType.MaxHealth
                };
                effect = new StatPassive(statComp, statModify);
                break;
            case AugmentType.Stat_Size:
                statModify = new ModifyInfo
                {
                    ModifyType = ModifyType.Add,
                    Magnitude = statComp.GetBaseValue(StatType.Size) * 0.1f,
                    TargetStat = StatType.Size
                };
                effect = new StatPassive(statComp, statModify);
                break;
            case AugmentType.Stat_Speed:
                statModify = new ModifyInfo
                {
                    ModifyType = ModifyType.Add,
                    Magnitude = statComp.GetBaseValue(StatType.InitialSpeed) * 0.1f,
                    TargetStat = StatType.InitialSpeed
                };
                effect = new StatPassive(statComp, statModify);
                break;
            case AugmentType.Stat_Power:
                statModify = new ModifyInfo
                {
                    ModifyType = ModifyType.Add,
                    Magnitude = statComp.GetBaseValue(StatType.AttackPower) * 0.1f,
                    TargetStat = StatType.AttackPower
                };
                effect = new StatPassive(statComp, statModify);
                break;
            case AugmentType.Passive_Satellite:
                effect = new SatellitePassive(statComp);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        if (effect != null)
            effect._statComponent = statComp;

        return effect;
    }
}

public class AugmentDataManager : MonoBehaviour
{
    private static Dictionary<AugmentType, AugmentDataSO> _augmentDict;

    public static AugmentDataSO GetAugmentData(AugmentType type)
    {
        if (_augmentDict == null)
            LoadAllAugments();

        if (_augmentDict.TryGetValue(type, out var data))
            return data;

        Debug.LogWarning($"AugmentData for type {type} not found.");
        return null;
    }

    private static void LoadAllAugments()
    {
        _augmentDict = new Dictionary<AugmentType, AugmentDataSO>();

        var all = Resources.LoadAll<AugmentDataSO>("Data/Augments");
        foreach (var augment in all)
        {
            if (!_augmentDict.ContainsKey(augment.augmentType))
                _augmentDict.Add(augment.augmentType, augment);
            else
                Debug.LogWarning($"Duplicate AugmentType found: {augment.augmentType}");
        }
    }
}

public class AugmentManager : MonoBehaviour
{
    
}
