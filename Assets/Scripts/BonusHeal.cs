using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusHeal : MonoBehaviour
{
    private Player m_player;

    private Camera m_MainCamera;
    [SerializeField] private float m_bonusSpeed = 2;

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
            Destroy(gameObject);
        }
    }
}
