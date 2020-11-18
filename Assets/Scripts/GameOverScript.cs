using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    private int m_playerScore;
    [SerializeField] private TextMeshProUGUI m_displayFinalScore;

    // Start is called before the first frame update
    void Start()
    {
        m_playerScore = PlayerPrefs.GetInt("score");
        m_displayFinalScore.text = "Your score : " + m_playerScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
