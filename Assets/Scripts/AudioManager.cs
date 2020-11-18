using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource m_music;
    public bool bossHasSpawned = false;

    private void Start()
    {
        m_music = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (bossHasSpawned)
            m_music.pitch = 1.25f;
    }
}
