using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayRollSoundFX()
    {
        Debug.Log("Roll Sound FX Played!");
        _audioSource.PlayOneShot(WorldSoundFXManager.Instance._rollSFX);
    }
}
