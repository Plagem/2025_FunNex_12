using System;
using System.Collections.Generic;
using UnityEngine;

public enum DurationType
{
    Instance,
    Duration,
    Infinite,
}

public enum ModifyType
{
    Add,
    Multiply,
    Override,
}

public class ModifyInfo
{
    public StatType TargetStat;
    public ModifyType ModifyType;
    public float Magnitude;
}

public class BaseEffect
{
    public int SkillLevel = 1;
    public string EffectName;
    public bool CanStack;
    public DurationType DurationType;
    public List<ModifyInfo> ModifyInfos;
    public BaseStatComponent _statComponent;
    public float EffectDuration;

    private float _effectTimePassed;
    
    private int _stack;

    public string GetName()
    {
        return EffectName;
    }

    public void EffectUpdate()
    {
        if (DurationType == DurationType.Duration || DurationType == DurationType.Infinite)
        {
            _effectTimePassed += Time.deltaTime;
        }

        if (DurationType == DurationType.Duration)
        {
            if (_effectTimePassed > EffectDuration)
            {
                _statComponent.RemoveEffect(this);
            }
        }
    }

    public virtual void OnEffectApplied()
    {
        _effectTimePassed = 0f;
    }

    public virtual void OnEffectRemoved()
    {
        
    }
}
