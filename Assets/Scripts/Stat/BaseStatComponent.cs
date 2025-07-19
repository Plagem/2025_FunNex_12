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
        OnAttributeChanged += AttributeChanged;
        if (InitialData)
        {
            InitializeStatComponent(InitialData);
        }
    }

    public Dictionary<StatType, AttributeData> GetAttributes()
    {
        return _attributes;
    }

    public float GetCurrentValue(StatType statType)
    {
        return _attributes[statType].CurrentValue;
    }
    
    public float GetBaseValue(StatType statType)
    {
        return _attributes[statType].BaseValue;
    }

    public void BestowAugment(AugmentType augmentType)
    {
        BaseEffect augment = EffectFactory.CreateEffect(augmentType, this);
        ApplyEffect(augment);
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

    public float GetFinalDamage()
    {
        return GetCurrentValue(StatType.AttackPower);
    }

    public void ApplyDamage(float damage)
    {
        Debug.Log($"{damage}");
        DamageEffect damageEffect = new DamageEffect();
        damageEffect.Initialize(damage);
        ApplyEffect(damageEffect);
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
        Debug.Log($"이펙트 {effect.GetName()} 추가");
        
        _activeEffects.Add(effect);
        
        effect._statComponent = this;
        
        effect.OnEffectApplied();
        
        if (effect.DurationType == DurationType.Instance)
        {
            if (effect.ModifyInfos != null)
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
            RemoveEffect(effect);
        }
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

    public BaseEffect FindEffect(string effectName)
    {
        foreach (var effect in _activeEffects)
        {
            if (effect.GetName() == effectName)
            {
                return effect;
            }
        }
        return null;
    }

    private void AttributeChanged(StatType statType, AttributeData data)
    {
        if (statType == StatType.Weight)
        {
            Debug.Log($"무게 {data.CurrentValue}로 세팅");
            GetComponent<Rigidbody2D>().mass = data.CurrentValue;
        }

        if (statType == StatType.CurrentHealth && GetCurrentValue(StatType.CurrentHealth) <= 0)
        {
            if (CompareTag("Player"))
            {
                // 플레이어일 경우 GameManager에 보고
                GameManager gm = FindFirstObjectByType<GameManager>();
                gm?.OnPlayerDied();
            }
        }
    }
}
