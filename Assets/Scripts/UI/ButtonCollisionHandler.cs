using UnityEngine;
using System.Collections;

public class ButtonCollisionHandler : MonoBehaviour
{
    private bool isTriggered = false; // 중복 방지

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggered) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
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
