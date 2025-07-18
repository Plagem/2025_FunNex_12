using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatComponent : MonoBehaviour
{
    public event Action<eStatType, float> OnAttributeChanged;
    
    private Dictionary<eStatType, float> _attributes;
    private List<BaseEffect> _activeEffects;

    public StatDataSO InitialData;

    private void Awake()
    {
        if (InitialData)
        {
            InitializeStatComponent(InitialData);
        }
    }

    public Dictionary<eStatType, float> GetAttributes()
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
        _attributes = new Dictionary<eStatType, float>();
        _activeEffects = new List<BaseEffect>();

        foreach (var stat in statData.stats)
        {
            _attributes[stat.eStatType] = stat.value;
            OnAttributeChanged?.Invoke(stat.eStatType, stat.value);
        }

        Debug.Log("LL");
    }

    public void UpdateFinalAttributeValue(eStatType statType)
    {
        // TODO
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
