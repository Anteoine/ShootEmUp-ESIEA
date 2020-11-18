using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusWeapon : MonoBehaviour
{
    private Player m_player;

    private Camera m_MainCamera;
    [SerializeField] private float m_bonusSpeed = 2;

    [SerializeField] private AudioClip m_yes;
    

    private int m_count = 0;

    Vector3 screenPos;

    private void Start()
    {
        m_MainCamera = Camera.main;
        m_player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        MoveBonus();
    }

    void MoveBonus()
    {
        Vector3 newPos = transform.position;
        screenPos = m_MainCamera.WorldToScreenPoint(transform.position);

        if (screenPos.y <= 0)
            Destroy(gameObject);
        else
            newPos.y -= m_bonusSpeed * Time.deltaTime;

        transform.position = newPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(m_yes, new Vector3(0, .5f, 0), 0.6f);
            if (m_player.PlayerShootPattern <= 2)
            {
                m_player.PlayerShootPattern++;
                switch (m_player.PlayerShootPattern)
                {
                    case 1:
                        m_player.AtqIcon1.gameObject.SetActive(true);
                        break;
                    case 2:
                        m_player.AtqIcon2.gameObject.SetActive(true);
                        break;
                    case 3:
                        m_player.AtqIcon3.gameObject.SetActive(true);
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
}
