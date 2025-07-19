using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Awake()
    {
        SoundManager.Instance?.Init();
    }

    private void Start()
    {
        
        SoundManager.Instance.Play(Define.BGM.BGM_Main);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("CutScene");
    }

    public void ExitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
