using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private BaseStatComponent _statComponent;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private Slider _healthBar;

    private void Start()
    {
        GameObject Boss = GameObject.Find("KillerWhaleBoss");
        if (Boss)
        {
            //_statComponent = Boss.GetComponent<BaseStatComponent>();
            _healthText.SetText($"{_statComponent.GetCurrentValue(StatType.CurrentHealth)} / {_statComponent.GetCurrentValue(StatType.MaxHealth)}");
            _statComponent.OnAttributeChanged += OnAttributeChanged;
        }
    }
    
    private void OnAttributeChanged(StatType statType, AttributeData attributeData)
    {
        if (statType == StatType.MaxHealth || statType == StatType.CurrentHealth)
        {
            float HealthPercent = _statComponent.GetCurrentValue(StatType.CurrentHealth) /
                                  _statComponent.GetCurrentValue(StatType.MaxHealth);
            _healthText.SetText($"{(int)_statComponent.GetCurrentValue(StatType.CurrentHealth)} / {(int)_statComponent.GetCurrentValue(StatType.MaxHealth)}");
            _healthBar.value = HealthPercent;
        }
    }
}
