using UnityEngine;

// 적을 처치하면 힐이 된다
public class HealPassive : BasePassive
{
    private float HealMagnitude = 10f;
    
    public HealPassive(BaseStatComponent statComponent)
    {
        EffectName = "HealPassive";
        DurationType = DurationType.Infinite;
        CanStack = true;

        _playerController = statComponent.GetComponent<PlayerController>();
        _playerController.OnMonsterEliminated += OnMonsterEliminated;
    }

    private void OnMonsterEliminated()
    {
        HealEffect healEffect = new HealEffect(SkillLevel * HealMagnitude);
        _statComponent.ApplyEffect(healEffect);
    }
}
