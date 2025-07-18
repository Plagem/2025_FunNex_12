using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : BaseEffect
{
    public void Initialize(float Damage)
    {
        EffectName = "Damage Effect";
        CanStack = false;
        DurationType = DurationType.Instance;
        
        ModifyInfos = new List<ModifyInfo>();
        ModifyInfo tempModify = new ModifyInfo
        {
            ModifyType = ModifyType.Add,
            Magnitude = -Damage,
            TargetStat = StatType.CurrentHealth
        };
        ModifyInfos.Add(tempModify);
    }

    public override void OnEffectApplied()
    {
        base.OnEffectApplied();
        
        Debug.Log("Damage Applied");
    }

    public override void OnEffectRemoved()
    {
        base.OnEffectRemoved();
        
        
    }
}
