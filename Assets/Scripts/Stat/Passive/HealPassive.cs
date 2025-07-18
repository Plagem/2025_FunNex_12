using UnityEngine;

// ���� óġ�ϸ� ���� �ȴ�
public class HealPassive : BasePassive
{
    private float HealMagnitude = 10f;
    
    public HealPassive(BaseStatComponent statComponent)
    {
        EffectName = "HealPassive";
        DurationType = DurationType.Infinite;
        CanStack = true;

        _playerController = statComponent.GetComponent<PlayerController>();
    }

    private void OnMonsterEliminated()
    {
        HealEffect healEffect = new HealEffect(SkillLevel * HealMagnitude);
        _statComponent.ApplyEffect(healEffect);
    }
}
