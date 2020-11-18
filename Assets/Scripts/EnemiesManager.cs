using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private GameObject m_enemyObject;
    [SerializeField] private GameObject m_bossObject;
    [SerializeField] private GameObject m_finalBossObject;
    [SerializeField] private float m_spawnDelay, m_spawnBossDelay;
    [SerializeField] private Player m_player;

    private float m_spawnX;
    private Vector3 m_spawnPosition;
    private bool m_finalBossHasSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnBoss());
    }

    // Update is called once per frame
    void Update()
    {
        if (m_player.PlayerScore >= 2000 && !m_finalBossHasSpawned)
        {
            m_finalBossHasSpawned = true;
            StartCoroutine(SpawnFinalBoss());
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2);
        while (Application.isPlaying && m_player.PlayerScore < 2000)
        {
            m_spawnDelay -= 0.03f;
            if (m_spawnDelay < 0.5f)
                m_spawnDelay = 0.5f;
            m_spawnX = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 1.2f)).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 1.2f)).x);


            m_spawnPosition = new Vector3(m_spawnX, 2.3f, 1);
            Instantiate(m_enemyObject, m_spawnPosition, Quaternion.Euler(70, 180, 0));
            yield return new WaitForSeconds(m_spawnDelay);
        }
    }

    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(m_spawnBossDelay);
        while (Application.isPlaying && m_player.PlayerScore < 2000)
        {
            m_spawnBossDelay -= .5f;
            if (m_spawnBossDelay < 2f)
                m_spawnBossDelay = 2f;

            m_spawnX = Random.Range
                    (Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 1.2f)).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 1.2f)).x);


            m_spawnPosition = new Vector3(m_spawnX, 2.3f, 1);
            Instantiate(m_bossObject, m_spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(m_spawnBossDelay);
        }

    }

    IEnumerator SpawnFinalBoss()
    {
        yield return new WaitForSeconds(2);
        
        m_spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, 1));
        m_spawnPosition.z = 180;
        m_spawnPosition.y += 15;
        Instantiate(m_finalBossObject, m_spawnPosition, Quaternion.Euler(25, 180, 0));
    }
}
