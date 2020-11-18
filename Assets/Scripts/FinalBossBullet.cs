using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossBullet : MonoBehaviour
{
    private Camera m_MainCamera;
    Vector3 screenPos;

    private void Awake()
    {
        m_MainCamera = Camera.main;
    }

    void Update()
    {
        screenPos = m_MainCamera.WorldToScreenPoint(transform.position);

        if (screenPos.y <= 0 || screenPos.x <= 0 || screenPos.x >= Screen.width || screenPos.y >= Screen.height)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
