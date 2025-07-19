using UnityEngine;
using DG.Tweening;

public class StarEffect : MonoBehaviour
{
    public void Init(Vector3 direction, float distance, float fadeDuration)
    {
        float scale = Random.Range(2f, 3f);
        transform.localScale = Vector3.one * scale;

        // ���� ���� ���̵�
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color startColor = sr.color;
        startColor.a = 1f;
        sr.color = startColor;

        // �̵�
        Vector3 targetPos = transform.position + direction * distance;

        // ���ÿ� �̵��� ���� ���� ����
        Sequence seq = DOTween.Sequence();
        seq.Join(transform.DOMove(targetPos, fadeDuration).SetEase(Ease.OutQuad));
        seq.Join(sr.DOFade(0f, fadeDuration));
        seq.OnComplete(() => Destroy(gameObject));
    }
}