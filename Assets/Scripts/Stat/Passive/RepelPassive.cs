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
        _playerController.OnPlayerStopped += OnPlayerStopped;
    }

    private void OnPlayerStopped()
    {
        Debug.Log("OnPlayerStopped");
        RepelEffect repelEffect = new RepelEffect();
        repelEffect.Initialize(_playerController.GetStatComponent().GetCurrentValue(StatType.AttackPower), 3f);
        _playerController.GetStatComponent().ApplyEffect(repelEffect);
    }

}
