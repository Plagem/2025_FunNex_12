using System.Collections.Generic;
using UnityEngine;

public class KaisaQPassive : BasePassive
{
    public KaisaQPassive(BaseStatComponent statComponent)
    {
        EffectName = "KaisaQPassive";
        DurationType = DurationType.Infinite;
        CanStack = true;

        _playerController = statComponent.GetComponent<PlayerController>();
        _playerController.OnAttackStarted += OnAttackStarted;
    }

    private void OnAttackStarted()
    {
        KaisaQEffect kq = new KaisaQEffect(
            _playerController.transform,
            3 * SkillLevel,
            120f,
            0f,
            10f);
        _statComponent.ApplyEffect(kq);
    }
}
