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
        _augmentImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnAugmentSelected?.Invoke(_augmentType);

        PlayerController pc = FindAnyObjectByType<PlayerController>();
        if (pc)
        {
            pc.GetStatComponent().BestowAugment(_augmentType);
        }
    }

    public void OpenShell(Sprite shellSprite)
    {
        _augmentBackgroundImage.sprite = shellSprite;
        _augmentImage.enabled = true;
    }
    
    public void CloseShell(Sprite shellSprite)
    {
        _augmentBackgroundImage.sprite = shellSprite;
        _augmentImage.enabled = false;
    }
}
