using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    private Sequence seq;
    
    public void Init(string damage)
    {
        damageText.SetText(damage);

        // 초기 설정
        transform.localScale = Vector3.one;
        RectTransform rt = GetComponent<RectTransform>();
        CanvasGroup group = gameObject.AddComponent<CanvasGroup>();
        group.alpha = 1f;

        Vector3 startPos = rt.anchoredPosition;
        Vector3 endPos = startPos + new Vector3(0, 30f, 0); // 위로 30만큼

        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOAnchorPos(endPos, 0.5f).SetEase(Ease.OutQuad));     // 위로 올라감
        seq.Append(group.DOFade(0f, 0.3f));                                // 투명해짐
        seq.OnComplete(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        seq?.Kill();
    }
}
