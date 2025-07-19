using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private Dictionary<AugmentType, int> PlayerAugments;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    
}
