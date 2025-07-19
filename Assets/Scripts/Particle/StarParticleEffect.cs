using UnityEngine;
using DG.Tweening;

public class StarEffect : MonoBehaviour
{
    public void Init(Vector3 direction, float distance, float fadeDuration)
    {
        float scale = Random.Range(2f, 3f);
        transform.localScale = Vector3.one * scale;

        // 랜덤 투명도 페이드
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color startColor = sr.color;
        startColor.a = 1f;
        sr.color = startColor;

        // 이동
        Vector3 targetPos = transform.position + direction * distance;

        // 동시에 이동과 투명도 감소 실행
        Sequence seq = DOTween.Sequence();
        seq.Join(transform.DOMove(targetPos, fadeDuration).SetEase(Ease.OutQuad));
        seq.Join(sr.DOFade(0f, fadeDuration));
        seq.OnComplete(() => Destroy(gameObject));
    }
}