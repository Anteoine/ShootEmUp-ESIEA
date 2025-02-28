﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_pauseMenuUI;
    [SerializeField] private bool isPaused;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (isPaused)
            ActivateMenu();

        else
            DeactivateMenu();
    }

    void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        m_pauseMenuUI.SetActive(true);
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        m_pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        m_pauseMenuUI.SetActive(false);
        isPaused = false;

        SceneManager.LoadScene("Menu");
    }

}
