using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Boss : MonoBehaviour
{
    private Camera m_MainCamera;
    [SerializeField] private float m_bossSpeed = 1;
    [SerializeField] private int m_bossHP;
    [SerializeField] private float m_bossShootRate;
    [SerializeField] private GameObject m_bulletBossObject;
    [SerializeField] private GameObject m_bonusWeapon;
    [SerializeField] private GameObject m_bonusHeal;
    [SerializeField] private GameObject m_bonusAS;

    [SerializeField] private AudioClip m_deathSound;
    [SerializeField] private GameObject m_explosionFX;
    private AudioSource m_shootSound;

    private float m_movementDuration = 4, m_waitBeforeMoving = .5f;
    private bool hasArrived = false;
    private bool isPlaced = false;

    Vector3 screenPos;

    public Stopwatch timer;

    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = Camera.main;
        timer = new Stopwatch();
        timer.Start();
        m_shootSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        BossMove();
        BossShoot();
    }


    void BossMove()
    {
        Vector3 newPos = transform.position;
        screenPos = m_MainCamera.WorldToScreenPoint(transform.position);

        if (screenPos.y <= 0)
            Destroy(gameObject);
        else
            newPos.y -= m_bossSpeed * Time.deltaTime;

        transform.position = newPos;
    }

    void BossShoot()
    {
        if (timer.Elapsed.TotalSeconds > m_bossShootRate)
        {

            Instantiate(m_bulletBossObject, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.identity);
            AudioSource.PlayClipAtPoint(m_shootSound.clip, new Vector3(0, .5f, 1), 0.15f);

            timer.Stop();
            timer.Reset();
            timer.Start();
        }
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
            m_bossHP -= 10;
            if (m_bossHP <= 0)
            {
                float RandomOrb = Random.Range(0, 40);

                if (RandomOrb <= 10)
                    Instantiate(m_bonusWeapon, transform.position, Quaternion.identity);
                else if (RandomOrb <= 20)
                    Instantiate(m_bonusAS, transform.position, Quaternion.identity);
                else if (RandomOrb <= 30)
                    Instantiate(m_bonusHeal, transform.position, Quaternion.identity);

                AudioSource.PlayClipAtPoint(m_deathSound, transform.position);
                Instantiate(m_explosionFX, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

}
