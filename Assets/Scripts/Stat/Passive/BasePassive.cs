using UnityEngine;

public class BasePassive : BaseEffect
{
    protected PlayerController _playerController;
    
    public string PassiveName;

    public string PassiveDescription;

    public override void OnEffectApplied()
    {
        base.OnEffectApplied();

        BaseEffect effect = _statComponent.FindEffect(EffectName);
        if (effect != null && effect != this)
        {
            if (CanStack)
            {
                int lastSkillLevel = effect.SkillLevel;
                SkillLevel = lastSkillLevel + 1;
                _statComponent.RemoveEffect(effect);
            }
            else
            {
                _statComponent.RemoveEffect(effect);
            }
        }
    }
}
