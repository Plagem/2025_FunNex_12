using UnityEngine;
using System.Collections;

public class ImageCollisionHandler : MonoBehaviour
{
    public Sprite triggeredSprite; // 충돌 시 바꿀 스프라이트
    private SpriteRenderer spriteRenderer;

    private bool isTriggered = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer가 없습니다.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggered) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;

            // 이미지 변경
            if (triggeredSprite != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = triggeredSprite;
            }

            // 동작 분기 (태그로 구분)
            if (gameObject.CompareTag("StartImage"))
            {
                StartCoroutine(TriggerStart());
            }
            else if (gameObject.CompareTag("ExitImage"))
            {
                StartCoroutine(TriggerExit());
            }
        }
    }

    IEnumerator TriggerStart()
    {
        yield return new WaitForSeconds(1f);
        FindFirstObjectByType<MainMenuManager>()?.StartGame();
    }

    IEnumerator TriggerExit()
    {
        yield return new WaitForSeconds(1f);
        FindFirstObjectByType<MainMenuManager>()?.ExitGame();
    }
}
