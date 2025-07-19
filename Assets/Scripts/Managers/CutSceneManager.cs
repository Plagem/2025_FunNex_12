using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [Header("�ƽ� �̹�����")]
    [SerializeField] private Sprite[] cutsceneImages;

    [Header("�̹����� ǥ���� UI")]
    [SerializeField] private Image displayImage;

    [Header("��ŵ ��ư")]
    [SerializeField] private Button skipButton;

    private int currentIndex = 0;

    private void Start()
    {
        if (cutsceneImages.Length == 0 || displayImage == null)
        {
            Debug.LogError("�ƽ� �̹��� �Ǵ� �̹��� ������Ʈ�� �������� �ʾҽ��ϴ�.");
            return;
        }

        displayImage.sprite = cutsceneImages[0];
        skipButton.onClick.AddListener(SkipCutscene);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowNextImage();
        }
    }

    private void ShowNextImage()
    {
        currentIndex++;

        if (currentIndex >= cutsceneImages.Length)
        {
            LoadGameScene();
        }
        else
        {
            displayImage.sprite = cutsceneImages[currentIndex];
        }
    }

    private void SkipCutscene()
    {
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}