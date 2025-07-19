using UnityEngine;

[CreateAssetMenu(fileName = "NewAugmentData", menuName = "Game/Asset Data")]
public class AugmentDataSO : ScriptableObject
{
    public AugmentType augmentType;
    
    [Header("Visual")]
    public Sprite icon;

    [Header("Text Info")]
    public string displayName;
    [TextArea]
    public string description;
}