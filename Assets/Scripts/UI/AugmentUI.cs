using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AugmentUI : MonoBehaviour, IPointerClickHandler
{
    public event Action<AugmentType> OnAugmentSelected;

    [SerializeField] private TMP_Text _augmentNameText;
    [SerializeField] private TMP_Text _augmentDescriptionText;
    [SerializeField] private Image _augmentBackgroundImage;
    [SerializeField] private Image _augmentImage;
    public AugmentType _augmentType;

    public void Initialize(AugmentDataSO data)
    {
        _augmentImage.sprite = data.icon;
        _augmentNameText.SetText(data.displayName);
        _augmentDescriptionText.SetText(data.description);
        _augmentType = data.augmentType;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnAugmentSelected?.Invoke(_augmentType);
    }

    public void CloseShell(Sprite shellSprite)
    {
        _augmentBackgroundImage.sprite = shellSprite;
        _augmentImage.sprite = null;
    }
}
