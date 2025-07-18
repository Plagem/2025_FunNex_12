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

public class BaseEffect : MonoBehaviour
{
    public string EffectName;
    public bool CanStack;
    public DurationType DurationType;
    public List<ModifyInfo> ModifyInfos =  new List<ModifyInfo>();
    public BaseStatComponent _statComponent;
    public float EffectDuration;

    private float _effectTimePassed;
    
    private int _stack;

    private void Awake()
    {
    }

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
    }

    public virtual void OnEffectApplied()
    {
        _effectTimePassed = 0f;
    }

    public virtual void OnEffectRemoved()
    {
        
    }
}
