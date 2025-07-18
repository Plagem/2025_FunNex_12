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
    public List<ModifyInfo> ModifyInfos;
    
    private int _stack;
    
    private BaseStatComponent _statComponent;


    public string GetName()
    {
        return EffectName;
    }

    public void EffectUpdate()
    {
        // 쿨타임 줄이거나 하기
    }

    public void OnEffectApplied()
    {
        
    }

    public void OnEffectRemoved()
    {
        
    }
}
