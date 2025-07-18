public class RepelPassive : BasePassive
{
    public RepelPassive(BaseStatComponent statComponent)
    {
        EffectName = "RepelPassive";
        DurationType = DurationType.Infinite;
        CanStack = true;

        _playerController = statComponent.GetComponent<PlayerController>();
        _playerController.OnComboTriggered += OnFirstCombo;
    }

    private void OnFirstCombo(int combo)
    {
        if (combo == 1)
        {
            RepelEffect repelEffect = new RepelEffect(_playerController.GetStatComponent().GetCurrentValue(StatType.AttackPower), 3f, SkillLevel);
            _statComponent.ApplyEffect(repelEffect);
        }
    }
}
