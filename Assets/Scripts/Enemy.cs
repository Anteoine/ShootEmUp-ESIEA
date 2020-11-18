    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Camera m_MainCamera;
    [SerializeField] private float m_enemySpeed = 2;
    [SerializeField] private int m_enemyHP;

    [SerializeField] private GameObject m_explosionFX;

    [SerializeField] private AudioClip m_deathSound;

    Vector3 screenPos;

    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        Vector3 newPos = transform.position;
        screenPos = m_MainCamera.WorldToScreenPoint(transform.position);

        if (screenPos.y <= 0)
            Destroy(gameObject);
        else
            newPos.y -= m_enemySpeed * Time.deltaTime;

        transform.position = newPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(m_explosionFX, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(m_deathSound, transform.position);
            Destroy(gameObject);
        }

        else if (collision.gameObject.tag == "Bullet")
        {
            m_enemyHP -= 10;
            if (m_enemyHP <= 0)
            {
                Instantiate(m_explosionFX, transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(m_deathSound, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
