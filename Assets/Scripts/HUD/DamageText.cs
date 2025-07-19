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

        // �ʱ� ����
        transform.localScale = Vector3.one;
        CanvasGroup group = gameObject.AddComponent<CanvasGroup>();

        // �ִϸ��̼� ������
        seq = DOTween.Sequence();

        seq.Append(transform.DOMoveY(transform.position.y + 1f, 0.5f).SetEase(Ease.OutQuad)); // ���� �߱�
        seq.Join(group.DOFade(0.5f, 0.8f));                                                      // ���� �������
        seq.Join(transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo));                    // ƨ����� Ȯ��
        
        seq.AppendCallback(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        seq?.Kill();
    }
}
