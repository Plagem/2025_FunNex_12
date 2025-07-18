using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private BaseStatComponent _statComponent;
    private ProgressBar _healthBar;
    
    private void Start()
    {
        _statComponent = GetComponent<BaseStatComponent>();
        _statComponent.OnAttributeChanged += OnAttributeChanged;
    }

    private void OnAttributeChanged(StatType statType, AttributeData attributeData)
    {
        if (statType == StatType.MaxHealth || statType == StatType.CurrentHealth)
        {
            float HealthPercent = _statComponent.GetCurrentValue(StatType.CurrentHealth) /
                                  _statComponent.GetCurrentValue(StatType.MaxHealth) * 100;
            Debug.Log($"Health Percent : {HealthPercent}%");
        }
    }
}
