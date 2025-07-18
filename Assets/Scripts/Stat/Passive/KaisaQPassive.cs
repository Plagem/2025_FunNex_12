using System.Collections.Generic;
using UnityEngine;

public class KaisaQPassive : BasePassive
{
    private PlayerController _playerController;

    public void Initialize(PlayerController Instigator)
    {
        ModifyInfos = new List<ModifyInfo>();

        EffectName = "KaisaQPassive";
        DurationType = DurationType.Infinite;
        CanStack = true;

        _playerController = Instigator;
        _playerController.OnAttackStarted += OnAttackStarted;
    }

    private void OnAttackStarted()
    {
        KaisaQEffect kq = new KaisaQEffect(
            _playerController.transform,
            4 * SkillLevel,
            120f,
            0f,
            10f);
        _statComponent.ApplyEffect(kq);
    }
}
