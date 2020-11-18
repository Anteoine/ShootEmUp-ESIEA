using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;
using System.Net.Http;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera m_MainCamera;
    [SerializeField] private int m_VerticalSpeed = 5, m_HorizontalSpeed = 5;
    Vector3 screenPos;

    [SerializeField] private float m_shootRate;
    [SerializeField] private GameObject m_bulletObject;
    public int PlayerShootPattern = 0;

    [SerializeField] public int m_playerHP;
    [SerializeField] private RawImage Heart1, Heart2, Heart3, Shield;
    [SerializeField] public RawImage AtqIcon1, AtqIcon2, AtqIcon3;
    [SerializeField] public RawImage Power1, Power2, Power3;

    public int PlayerScore = 0;
    [SerializeField] private TextMeshProUGUI m_displayScore;

    private AudioSource m_laserShotAudio;
    [SerializeField] private AudioClip m_yeahBoi, m_wow, m_oof;

    private int m_count = 0;

    public Stopwatch timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = new Stopwatch();
        timer.Start();
        m_laserShotAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl();
    }
    
    void PlayerControl()
    {
        Vector3 newPos = transform.position;
        screenPos = m_MainCamera.WorldToScreenPoint(transform.position);

        // Move player
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (screenPos.x <= 5)
                newPos.x += 0.00001f;
            else
                newPos.x -= m_HorizontalSpeed * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (screenPos.x >= Screen.width)
                newPos.x -= 0.00001f;
            else
                newPos.x += m_HorizontalSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (screenPos.y >= Screen.height)
                newPos.y -= 0.00001f;
            else
                newPos.y += m_VerticalSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (screenPos.y <= 5)
                newPos.y += 0.00001f;
            else
                newPos.y -= m_VerticalSpeed * Time.deltaTime;
        }

        transform.position = newPos;
        //

        // Generate and shoot bullet
        if (Input.GetKey(KeyCode.Space))
        {        
            if (timer.Elapsed.TotalSeconds > m_shootRate)
            {
                if (PlayerShootPattern == 0)
                {
                    Instantiate(m_bulletObject, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.identity);
                    m_laserShotAudio.PlayOneShot(m_laserShotAudio.clip);
                }

                else if (PlayerShootPattern == 1)
                {
                    Instantiate(m_bulletObject, new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z), Quaternion.identity);
                    Instantiate(m_bulletObject, new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z), Quaternion.identity);
                    m_laserShotAudio.PlayOneShot(m_laserShotAudio.clip);
                }

                else if (PlayerShootPattern == 2)
                {
                    Instantiate(m_bulletObject, new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z), Quaternion.identity);
                    Instantiate(m_bulletObject, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.identity);
                    Instantiate(m_bulletObject, new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z), Quaternion.identity);
                    m_laserShotAudio.PlayOneShot(m_laserShotAudio.clip);
                }

                else if (PlayerShootPattern == 3)
                {
                    Instantiate(m_bulletObject, new Vector3(transform.position.x - 0.2f, transform.position.y - 0.1f, transform.position.z), Quaternion.identity);
                    Instantiate(m_bulletObject, new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z), Quaternion.identity);
                    Instantiate(m_bulletObject, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.identity);
                    Instantiate(m_bulletObject, new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z), Quaternion.identity);
                    Instantiate(m_bulletObject, new Vector3(transform.position.x + 0.2f, transform.position.y - 0.1f, transform.position.z), Quaternion.identity);
                    m_laserShotAudio.PlayOneShot(m_laserShotAudio.clip);
                }

                timer.Stop();
                timer.Reset();
                timer.Start();
            }
        }
        //
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "BulletBoss")
        {
            m_playerHP -= 20;
            AudioSource.PlayClipAtPoint(m_oof, new Vector3(0, .5f, 0), 0.3f);
            switch(m_playerHP)
            {
                case 60:
                    Shield.gameObject.SetActive(false);
                    break;
                case 40:
                    Heart3.gameObject.SetActive(false);
                    break;
                case 20:
                    Heart2.gameObject.SetActive(false); ;
                    break;
                case 0:
                    Heart1.gameObject.SetActive(false);
                    break;
            }
            if (m_playerHP <= 0)
            {
                Destroy(gameObject);

                // Display Game Over Scene + Score
                PlayerPrefs.SetInt("score", PlayerScore);
                SceneManager.LoadScene("GameOver");
            }
        }

        if (collision.gameObject.tag == "BonusHeal")
        {
            AudioSource.PlayClipAtPoint(m_yeahBoi, new Vector3(0, .5f, 0), 0.6f) ;
            if (m_playerHP == 60)
            {
                // Gagner une charge de shield (unique)
                m_playerHP += 20;
                Debug.Log("Activated the shield icon");
                Shield.gameObject.SetActive(true);
            }
            else if (m_playerHP == 40)
            {
                m_playerHP += 20;
                Heart3.gameObject.SetActive(true);
            }
            else if (m_playerHP == 20)
            {
                m_playerHP += 20;
                Heart2.gameObject.SetActive(true);
            }
        }

        if (collision.gameObject.tag == "BonusAS")
        {
            AudioSource.PlayClipAtPoint(m_wow, new Vector3(0, .5f, 0), 1);
            if (m_shootRate > 0.15f)
            {
                m_shootRate -= 0.05f;
                m_count++;
                switch (m_count)
                {
                    case 1:
                        Power1.gameObject.SetActive(true);
                        break;
                    case 2:
                        Power2.gameObject.SetActive(true);
                        break;
                    case 3:
                        Power3.gameObject.SetActive(true);
                        break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BulletBoss")
        {
            AudioSource.PlayClipAtPoint(m_oof, new Vector3(0, .5f, 0), 0.3f);
            m_playerHP -= 20;
            switch (m_playerHP)
            {
                case 60:
                    Shield.gameObject.SetActive(false);
                    break;
                case 40:
                    Heart3.gameObject.SetActive(false);
                    break;
                case 20:
                    Heart2.gameObject.SetActive(false); ;
                    break;
                case 0:
                    Heart1.gameObject.SetActive(false);
                    break;
            }
            if (m_playerHP <= 0)
            {
                Destroy(gameObject);

                // Display Game Over Scene + Score
                Debug.Log("YOU DIED WITH A SCORE OF : " + PlayerScore + " POINTS !");
                PlayerPrefs.SetInt("score", PlayerScore);
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    private void OnEnable()
    {
        Bullet.IncrementScore += OnBulletHit;
    }

    void OnBulletHit(int score)
    {
        PlayerScore += score;
        m_displayScore.SetText(""+PlayerScore);
        //Debug.Log("PlayerScore is " + PlayerScore);
    }
}
