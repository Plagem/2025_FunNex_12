using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseStatComponent : MonoBehaviour
{
    public event Action<StatType, AttributeData> OnAttributeChanged;
    
    private Dictionary<StatType, AttributeData> _attributes;
    private List<BaseEffect> _activeEffects;

    public StatDataSO InitialData;

    private void Awake()
    {
        if (InitialData)
        {
            InitializeStatComponent(InitialData);
        }
    }

    private void Start()
    {
        DamageEffect damageEffect = new DamageEffect();
        damageEffect.Initialize(10f);
        ApplyEffect(damageEffect);
    }

    public Dictionary<StatType, AttributeData> GetAttributes()
    {
        return _attributes;
    }

    public float GetCurrentValue(StatType statType)
    {
        return _attributes[statType].CurrentValue;
    }

    public void InitializeStatComponent(StatDataSO statData)
    {
        if (statData == null)
        {
            Debug.LogWarning("StatData is null.");
            return;
        }
        _attributes = new Dictionary<StatType, AttributeData>();
        _activeEffects = new List<BaseEffect>();

        foreach (var stat in statData.stats)
        {
            _attributes[stat.eStatType] = new AttributeData();
            _attributes[stat.eStatType].BaseValue = stat.value;
            UpdateFinalAttributeValue(stat.eStatType);
        }
    }

    public void UpdateFinalAttributeValue(StatType statType)
    {
        // TODO
        if (!_attributes.ContainsKey(statType)) return;

        AttributeData tempAttributeData = new AttributeData();
        
        foreach(var effect in _activeEffects)
        {
            if (effect.DurationType == DurationType.Instance) continue;
            
            foreach (var modifyInfo in effect.ModifyInfos)
            {
                if (modifyInfo.TargetStat != statType) continue;

                switch (modifyInfo.ModifyType)
                {
                    case ModifyType.Add:
                        tempAttributeData.AddValue += modifyInfo.Magnitude;
                        break;
                    case ModifyType.Multiply:
                        tempAttributeData.MulValue += (modifyInfo.Magnitude - 1);
                        break;
                    case ModifyType.Override:
                        tempAttributeData.OverrideValue = modifyInfo.Magnitude;
                        tempAttributeData.bOverride = true;
                        break;
                }
            }
        }

        tempAttributeData.BaseValue = _attributes[statType].BaseValue;
        tempAttributeData.AddValue += _attributes[statType].AddValue;
        tempAttributeData.MulValue += _attributes[statType].MulValue - 1;
        // TODO
        // Override 관련 문제있어요~
        
        if (tempAttributeData.bOverride)
        {
            tempAttributeData.CurrentValue = tempAttributeData.OverrideValue;
        }
        else
        {
            tempAttributeData.CurrentValue =
                tempAttributeData.BaseValue * tempAttributeData.MulValue + tempAttributeData.AddValue;
        }
        
        _attributes[statType] = tempAttributeData;
        
        OnAttributeChanged?.Invoke(statType, _attributes[statType]);
    }

    private void Update()
    {
        foreach (var effect in _activeEffects)
        {
            effect.EffectUpdate();
        }
    }
    
    // Effect
    public void ApplyEffect(BaseEffect effect)
    {
        _activeEffects.Add(effect);
        
        effect._statComponent = this;
        effect.OnEffectApplied();
        
        if (effect.DurationType == DurationType.Instance)
        {
            foreach(var modifyInfo in effect.ModifyInfos)
            {
                switch (modifyInfo.ModifyType)
                {
                    case ModifyType.Add:
                        _attributes[modifyInfo.TargetStat].AddValue += modifyInfo.Magnitude;
                        break;
                    case ModifyType.Multiply:
                        _attributes[modifyInfo.TargetStat].MulValue += modifyInfo.Magnitude - 1;
                        break;
                    case ModifyType.Override:
                        _attributes[modifyInfo.TargetStat].bOverride = true;
                        _attributes[modifyInfo.TargetStat].OverrideValue += modifyInfo.Magnitude;
                        break;
                }
                UpdateFinalAttributeValue(modifyInfo.TargetStat);
            }
        }
        
        Debug.Log($"이펙트 {effect.GetName()} 추가");
    }

    public void RemoveEffect(BaseEffect effect)
    {
        if (_activeEffects.Contains(effect))
        {
            _activeEffects.Remove(effect);
            effect.OnEffectRemoved();
            Debug.Log($"이펙트 {effect.GetName()} 제거");
            return;
        }
    }

    public void RemoveEffect(string effectName)
    {
        foreach (var effect in _activeEffects)
        {
            if (effect.GetName() == effectName)
            {
                _activeEffects.Remove(effect);
                effect.OnEffectRemoved();
                Debug.Log($"이펙트 {effectName} 제거");
                return;
            }
        }
    }
}
