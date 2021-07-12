using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject pauseMenu;
    //[SerializeField] AudioSource backgroundMusic;
    [SerializeField] GameObject reloadButton;
    [SerializeField] GameObject rewindButton;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void ReloadOnClick() {
        gameManager.ReloadCurrentLevel();
        reloadButton.GetComponent<Animator>().SetTrigger("Clicked");
    }

    public void RewindOnClick() {
        gameManager.Rewind();
        rewindButton.GetComponent<Animator>().SetTrigger("Clicked");
    }

    public void SettingsOnClick() {
        pauseMenu.SetActive(true);
    }

    public void QuitOnClick() {
        Application.Quit();
    }

    public void MuteOnClick() {
        //backgroundMusic.mute = true;
        //backgroundMusic.Pause();
        AudioListener.pause = !AudioListener.pause;
    }

    public void CloseSettingsOnClick() {
        pauseMenu.SetActive(false);
    }
}
