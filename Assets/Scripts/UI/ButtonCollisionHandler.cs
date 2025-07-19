using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonCollisionHandler : MonoBehaviour
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pressedSprite;

    private bool isTriggered = false;
    private Image buttonImage;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage == null)
        {
            Debug.LogError("Image 컴포넌트를 찾을 수 없습니다.");
        }

        if (defaultSprite != null)
            buttonImage.sprite = defaultSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggered) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;

            if (pressedSprite != null)
                buttonImage.sprite = pressedSprite;

            if (gameObject.CompareTag("StartBtn"))
            {
                StartCoroutine(TriggerStart());
            }
            else if (gameObject.CompareTag("ExitBtn"))
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
