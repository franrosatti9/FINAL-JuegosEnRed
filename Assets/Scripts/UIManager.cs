using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameManager gameManager;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            // DESHABILITAR PLAYER CONTROLLER Y ESO
        }
    }

    public void EnablePauseMenu(bool enabled)
    {
        if (pauseMenu.activeSelf) pauseMenu.SetActive(false);

    }

    public void BackToMainMenu()
    {
        if (PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel("Menu");
        }
        else
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel("Menu");
        }
    }
}
