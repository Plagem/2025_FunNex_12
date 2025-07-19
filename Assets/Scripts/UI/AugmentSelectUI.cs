using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class AugmentSelectUI : MonoBehaviour
{
    [SerializeField] private AugmentUI _augmentUI1;
    [SerializeField] private AugmentUI _augmentUI2;
    [SerializeField] private AugmentUI _augmentUI3;

    [SerializeField] private Sprite _shellClosedImage;

    public void ShowOption()
    {
        gameObject.SetActive(false);
        
        int count = 3;
        // MAX 제외하고 enum 값 배열로 가져오기
        var values = Enum.GetValues(typeof(AugmentType))
            .Cast<AugmentType>()
            .Where(a => a != AugmentType.MAX)
            .ToArray();

        // 셔플 후 앞에서 count개 선택
        var random = new System.Random();
        AugmentType[] selectedAugment = values.OrderBy(x => random.Next()).Take(count).ToArray();
        
        _augmentUI1.Initialize(AugmentDataManager.GetAugmentData(selectedAugment[0]));
        _augmentUI2.Initialize(AugmentDataManager.GetAugmentData(selectedAugment[1]));
        _augmentUI3.Initialize(AugmentDataManager.GetAugmentData(selectedAugment[2]));
        
        _augmentUI1.OnAugmentSelected += AugmentSelected;
        _augmentUI2.OnAugmentSelected += AugmentSelected;
        _augmentUI3.OnAugmentSelected += AugmentSelected;
    }

    private void AugmentSelected(AugmentType augmentType)
    {
        // 선택한 augmentType 해당하는거 말고 
        if (_augmentUI1._augmentType != augmentType)
        {
            _augmentUI1.CloseShell(_shellClosedImage);
        }
        if (_augmentUI2._augmentType != augmentType)
        {
            _augmentUI2.CloseShell(_shellClosedImage);
        }
        if (_augmentUI3._augmentType != augmentType)
        {
            _augmentUI3.CloseShell(_shellClosedImage);
        }
        
        CloseUIWithDelay();
    }
    
    public void CloseUIWithDelay(float delay = 1f)
    {
        StartCoroutine(CloseAfterDelay(delay));
    }

    private IEnumerator CloseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
