using System;
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

public class AugmentManager : MonoBehaviour
{
    
}
