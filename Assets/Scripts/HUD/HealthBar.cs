using System;
using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private BaseStatComponent _statComponent;
    
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private Slider _healthBar;
    
    public Transform target;
    public Vector3 offset = new Vector3(0, 1f, 0);
    
    private void Start()
    {
        _statComponent.OnAttributeChanged += OnAttributeChanged;
        _healthText.SetText($"{_statComponent.GetCurrentValue(StatType.CurrentHealth)}");
    }

    private void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.identity;
    }

    private void OnAttributeChanged(StatType statType, AttributeData attributeData)
    {
        if (statType == StatType.MaxHealth || statType == StatType.CurrentHealth)
        {
            float HealthPercent = _statComponent.GetCurrentValue(StatType.CurrentHealth) /
                                  _statComponent.GetCurrentValue(StatType.MaxHealth);
            _healthText.SetText($"{(int)_statComponent.GetCurrentValue(StatType.CurrentHealth)}");
            _healthBar.value = HealthPercent;
        }
    }
}
