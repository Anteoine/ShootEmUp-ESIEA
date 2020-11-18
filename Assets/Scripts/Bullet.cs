using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Camera m_MainCamera;
    [SerializeField] private int m_bulletSpeed = 8;
    Vector3 screenPos;

    public delegate void OnIncrementScore(int score);
    public static event OnIncrementScore IncrementScore;

    private void Awake()
    {
        m_MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        BulletMove();
    }

    void BulletMove()
    {
        Vector3 newPos = transform.position;
        screenPos = m_MainCamera.WorldToScreenPoint(transform.position);

        if (screenPos.y >= Screen.height)
            Destroy(gameObject);
        else
            newPos.y += m_bulletSpeed * Time.deltaTime;

        transform.position = newPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (IncrementScore != null)
                IncrementScore(5);
            Destroy(gameObject);
        }
    }
}
