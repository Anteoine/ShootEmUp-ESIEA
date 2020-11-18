using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class FinalBoss : MonoBehaviour
{
    private Player m_player;
    private Camera m_MainCamera;
    [SerializeField] private float m_bossSpeed = 1;
    [SerializeField] private int m_bossHP;
    [SerializeField] private float m_bossShootRate;

    // Projectile
    [SerializeField] private GameObject m_bulletBossObject;
    [SerializeField] private float m_projectileSpeed;
    [SerializeField] private int m_numberOfProjectiles;

    private Vector3 m_startPoint;
    private const float m_radius = 1f;

    [SerializeField] private AudioClip m_shootBoss, m_deathBoss;
    [SerializeField] private GameObject m_explosionFX;
    private AudioManager m_audioManager;

    private bool isPlaced = false, goesLeft = false, goesRight = true;
    Vector3 screenPos;
    public Stopwatch timer;

    private MeshRenderer m_meshRend;
    private bool m_hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = Camera.main;
        timer = new Stopwatch();
        timer.Start();
        m_meshRend = GetComponent<MeshRenderer>();
        m_audioManager = FindObjectOfType<AudioManager>();
        m_player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced)
            BossSpawn();
        else
        {
            BossMove();
            BossShoot(m_numberOfProjectiles);
        }
    }

    void BossSpawn()
    {
        Vector3 newPosSpawn = transform.position;

        if (transform.position.z > 1)
        {
             newPosSpawn.z -= transform.position.z * Time.deltaTime;

            if (transform.position.y >= 1.5f)
                newPosSpawn.y -= 4f * Time.deltaTime;

            transform.position = newPosSpawn;

            if (transform.position.z <= 1)
            {
                isPlaced = true;
                m_audioManager.bossHasSpawned = true;
            }
        }
    }

    void BossMove()
    {
        Vector3 newPos = transform.position;

        if (goesRight)
        {
            newPos.x += 0.002f;
            if (newPos.x >= 2)
            {
                goesRight = false;
                goesLeft = true;
            }
        }
        
        if (goesLeft)
        {
            newPos.x -= 0.002f;
            if (newPos.x <= -2)
            {
                goesRight = true;
                goesLeft = false;
            }
        }
        
        transform.position = newPos;
    }

    void BossShoot(int numberOfProjectiles)
    {
        if (timer.Elapsed.TotalSeconds > m_bossShootRate && !m_hasExploded)
        {
            float angleStep = 360f / numberOfProjectiles;
            float angle = 0f;

            m_startPoint = transform.position;

            for (int i = 0; i <= numberOfProjectiles - 1; i++)
            {
                float projectileDirXPosition = m_startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * m_radius;
                float projectileDirYPosition = m_startPoint.x + Mathf.Cos((angle * Mathf.PI) / 180) * m_radius;

                Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
                Vector3 projectileMoveDirection = (projectileVector - m_startPoint).normalized * m_projectileSpeed;

                GameObject tmpObj = Instantiate(m_bulletBossObject, m_startPoint, Quaternion.identity);
                tmpObj.GetComponent<Rigidbody>().velocity = new Vector3(projectileMoveDirection.x, projectileMoveDirection.y, 0);

                angle += angleStep;
            }

            AudioSource.PlayClipAtPoint(m_shootBoss, transform.position, .4f);

            timer.Stop();
            timer.Reset();
            timer.Start();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet" && !m_hasExploded)
        {
            m_bossHP -= 10;
            if (m_bossHP <= 0)
            {
                if (!m_hasExploded)
                    StartCoroutine(EndGame());
            }
        }
    }

    IEnumerator EndGame()
    {
        m_hasExploded = true;
        Instantiate(m_explosionFX, transform.position, Quaternion.identity);
        m_meshRend.enabled = false;
        AudioSource.PlayClipAtPoint(m_deathBoss, transform.position);

        yield return new WaitForSeconds(3);
        PlayerPrefs.SetInt("score", m_player.PlayerScore);
        SceneManager.LoadScene("Victory");
        Destroy(gameObject);

        
    }
}
