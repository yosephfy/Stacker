using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] PowerUps;

    public static AudioPlayer Instance;
    public AudioSource audio;

    public GameObject AudioParent;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;
    }

    public void PlayAudioPowerUp(int num)
    {
        audioSource_powerups[num].Play();
    }

    public void PlayDefaultAudio()
    {
        DefaultBlockAudio.Play();
    }

    private List<AudioSource> audioSource_powerups = new List<AudioSource>();

    private AudioSource DefaultBlockAudio;
    public AudioClip DefaultClip;

    private void OnEnable()
    {
        for (int i = 0; i < PowerUps.Length; i++)
        {
            audioSource_powerups.Add(Instantiate(audio, AudioParent.transform));
            audioSource_powerups[i].clip = PowerUps[i];
        }

        DefaultBlockAudio = Instantiate(audio, AudioParent.transform);
        DefaultBlockAudio.clip = DefaultClip;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}