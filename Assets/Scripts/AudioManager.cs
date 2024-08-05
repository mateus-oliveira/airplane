using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource sound;
    public static AudioManager instance = null;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(gameObject);
        }
    }

    public void PlayAudio(AudioClip audio) {
        sound.clip = audio;
        sound.Play();
    }
}
