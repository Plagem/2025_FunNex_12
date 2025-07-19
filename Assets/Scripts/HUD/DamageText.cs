using System;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    private Sequence seq;
    private Transform _target;
    private float upFloat = 0f;
    
    public void Init(string damage, Transform target)
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
        seq.Append(group.DOFade(0f, 0.3f));                                // 투명해짐
        seq.OnComplete(() => Destroy(gameObject));

        _target = target;
    }

    private void LateUpdate()
    {
        upFloat += 0.3f * Time.deltaTime;
        transform.position = _target.transform.position + new Vector3(0, 1.3f + upFloat, 0);
        transform.rotation = quaternion.identity;
    }

    private void OnDestroy()
    {
        seq?.Kill();
    }
}
