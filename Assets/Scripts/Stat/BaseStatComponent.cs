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

    public Dictionary<StatType, AttributeData> GetAttributes()
    {
        return _attributes;
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
            OnAttributeChanged?.Invoke(stat.eStatType, _attributes[stat.eStatType]);
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

        if (tempAttributeData.bOverride)
        {
            tempAttributeData.CurrentValue = tempAttributeData.OverrideValue;
        }
        else
        {
            tempAttributeData.CurrentValue =
                tempAttributeData.BaseValue * tempAttributeData.MulValue + tempAttributeData.AddValue;
        }
        
        Debug.Log($"{statType} base : {tempAttributeData.BaseValue}");
        Debug.Log($"{statType} mul : {tempAttributeData.MulValue}");
        Debug.Log($"{statType} override : {tempAttributeData.bOverride}");
        Debug.Log($"{statType} curValue Set to {tempAttributeData.CurrentValue}");
        _attributes[statType] = tempAttributeData;
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
        effect.OnEffectApplied();
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
