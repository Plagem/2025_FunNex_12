using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    private Sequence seq;
    
    public void Init(string text)
    {
        damageText.SetText(text);

        // 초기 상태
        transform.localScale = Vector3.one;
        CanvasGroup group = gameObject.AddComponent<CanvasGroup>();

        // 애니메이션 시퀀스
        seq = DOTween.Sequence();

        seq.Append(transform.DOMoveY(transform.position.y + 1f, 0.5f).SetEase(Ease.OutQuad)); // 위로 뜨기
        seq.Join(group.DOFade(0.5f, 0.8f));                                                      // 점점 사라지기
        seq.Join(transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo));                    // 튕기듯이 확대
        
        seq.AppendCallback(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        seq?.Kill();
    }
}
