using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_creditsPanel;
    [SerializeField] private GameObject m_mainMenuPanel;
    public void PressStartButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void PressCreditsButton()
    {
        // Désactiver certains éléments de l'UI et afficher ceux des règles
        m_mainMenuPanel.SetActive(false);
        m_creditsPanel.SetActive(true);
    }

    public void PressBackToMenuButton()
    {
        // Désactiver le panel des règles et réafficher le menu principal
        m_creditsPanel.SetActive(false);
        m_mainMenuPanel.SetActive(true);
    }

}
