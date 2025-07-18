using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    private string _effectName;
    private int _stack;
    private bool _canStack;

    public string GetName()
    {
        return _effectName;
    }

    public void EffectUpdate()
    {
        // 쿨타임 줄이거나 하기
    }

    public void OnEffectApplied()
    {
        
    }

    public void OnEffectRemoved()
    {
        
    }
}
