using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject onlineScreen;
    [SerializeField] GameObject mainScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject buttonsScreen;

    public void OnlineMenu()
    {
        mainScreen.SetActive(false);
        onlineScreen.SetActive(true);
        creditsScreen.SetActive(false);
        buttonsScreen.SetActive(false);
    }

    public void BackToMenu()
    {
        mainScreen.SetActive(true);
        onlineScreen.SetActive(false);
        creditsScreen.SetActive(false);
        buttonsScreen.SetActive(false);
    }

    public void CreditsMenu()
    {
        mainScreen.SetActive(false);
        onlineScreen.SetActive(false);
        creditsScreen.SetActive(true);
        buttonsScreen.SetActive(false);
    }

    public void ButtonsMenu()
    {
        mainScreen.SetActive(false);
        onlineScreen.SetActive(false);
        creditsScreen.SetActive(false);
        buttonsScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
