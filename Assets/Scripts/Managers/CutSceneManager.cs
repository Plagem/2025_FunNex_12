using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [Header("컷신 이미지들")]
    [SerializeField] private Sprite[] cutsceneImages;

    [Header("이미지를 표시할 UI")]
    [SerializeField] private Image displayImage;

    [Header("스킵 버튼")]
    [SerializeField] private Button skipButton;

    private int currentIndex = 0;

    private void Start()
    {
        if (cutsceneImages.Length == 0 || displayImage == null)
        {
            Debug.LogError("컷신 이미지 또는 이미지 컴포넌트가 설정되지 않았습니다.");
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