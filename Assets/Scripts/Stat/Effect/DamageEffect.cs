using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : BaseEffect
{
    void Start()
    {
        EffectName = "Damage Effect";
        CanStack = false;
        DurationType = DurationType.Instance;
    }

    public void Initialize(float Damage)
    {
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
