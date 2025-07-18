using System;
using System.Collections.Generic;
using UnityEngine;

public class RepelPassive : BasePassive
{
    private PlayerController _playerController;
    
    public void Initialize(PlayerController Instigator)
    {
        ModifyInfos = new List<ModifyInfo>();
        
        EffectName = "RepelPassive";
        DurationType = DurationType.Infinite;
        CanStack = false;

        _playerController = Instigator;
        _playerController.OnComboTriggered += OnFirstCombo;
    }

    private void OnFirstCombo(int combo)
    {
        Debug.Log($"Combo : {combo}");
        if (combo == 1)
        {
            RepelEffect repelEffect = new RepelEffect();
            repelEffect.Initialize(_playerController.GetStatComponent().GetCurrentValue(StatType.AttackPower), 3f, SkillLevel);
            _playerController.GetStatComponent().ApplyEffect(repelEffect);
        }
    }
}
