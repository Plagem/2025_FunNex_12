using System.Collections.Generic;
using UnityEngine;

public class HealEffect : BaseEffect
{

    public HealEffect(float Magnitude)
    {
        EffectName = "Damage Effect";
        CanStack = false;
        DurationType = DurationType.Instance;
        
        ModifyInfos = new List<ModifyInfo>();
        ModifyInfo tempModify = new ModifyInfo
        {
            ModifyType = ModifyType.Add,
            Magnitude = Magnitude,
            TargetStat = StatType.CurrentHealth
        };
        ModifyInfos.Add(tempModify);
    }
}
