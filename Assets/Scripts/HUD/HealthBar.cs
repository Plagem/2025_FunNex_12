using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private BaseStatComponent _statComponent;
    [SerializeField] private Canvas _healthBarCanvas;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private Slider _healthBar;
    
    public Transform target;
    public Vector3 offset = new Vector3(0, 1f, 0);
    
    private void Start()
    {
        _statComponent.OnDamaged += PrintDamage;
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

    public void PrintDamage(float damage)
    {
        DamageText damageText = DamageTextManager.Instance.CreateDamageText(_healthBarCanvas);
        damageText.transform.position = target.transform.position + offset + new Vector3(0, 0.3f, 0);
        damageText.transform.rotation = quaternion.identity;
        
        damageText.Init($"{(int)damage}");
    }
}
