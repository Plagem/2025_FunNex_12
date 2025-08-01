using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : BaseEffect
{
    private float _damage;
    
    public void Initialize(float Damage)
    {
        EffectName = "Damage Effect";
        CanStack = false;
        DurationType = DurationType.Instance;

        _damage = Damage;
        
        ModifyInfos = new List<ModifyInfo>();
        ModifyInfo tempModify = new ModifyInfo
        {
            ModifyType = ModifyType.Add,
            Magnitude = -Damage,
            TargetStat = StatType.CurrentHealth
        };
        ModifyInfos.Add(tempModify);
    }
}
