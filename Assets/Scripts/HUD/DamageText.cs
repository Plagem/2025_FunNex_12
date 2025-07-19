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

        // �ʱ� ����
        transform.localScale = Vector3.one;
        RectTransform rt = GetComponent<RectTransform>();
        CanvasGroup group = gameObject.AddComponent<CanvasGroup>();
        group.alpha = 1f;

        Vector3 startPos = rt.anchoredPosition;
        Vector3 endPos = startPos + new Vector3(0, 30f, 0); // ���� 30��ŭ

        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOAnchorPos(endPos, 0.5f).SetEase(Ease.OutQuad));     // ���� �ö�
        seq.Append(group.DOFade(0f, 0.3f));                                // ��������
        seq.OnComplete(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        seq?.Kill();
    }
}
