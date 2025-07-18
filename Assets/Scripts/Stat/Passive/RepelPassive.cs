using System;
using UnityEngine;

public class RepelPassive : BasePassive
{
    private PlayerController _playerController;
    private void Start()
    {
        //_playerController.OnPlayerStopped += OnPlayerStopped
    }

    private void OnPlayerStopped()
    {
        RepelEffect repelEffect = new RepelEffect();
        repelEffect.Initialize(_playerController.GetStatComponent().GetCurrentValue(StatType.AttackPower), 3f);
        _playerController.GetStatComponent().ApplyEffect(repelEffect);
    }

}
