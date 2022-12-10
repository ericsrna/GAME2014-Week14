using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioSource> channels;

    private List<AudioClip> audioClips;

    void Awake()
    {
        channels = GetComponents<AudioSource>().ToList();
        audioClips = new List<AudioClip>();
        InitializeSoundFX();
    }

    private void InitializeSoundFX()
    {
        // Pre-load sound_FX
        audioClips.Add(Resources.Load<AudioClip>("Audio/jump-sound"));
        audioClips.Add(Resources.Load<AudioClip>("Audio/hurt-sound"));
        audioClips.Add(Resources.Load<AudioClip>("Audio/lose-sound"));
        audioClips.Add(Resources.Load<AudioClip>("Audio/bullet-sound"));

        // Pre-load Music
        audioClips.Add(Resources.Load<AudioClip>("Audio/main-soundtrack"));
        audioClips.Add(Resources.Load<AudioClip>("Audio/end-soundtrack"));
    }

    public void PlaySoundFX(Sound sound, Channel channel)
    {
        channels[(int)channel].clip = audioClips[(int)sound];
        channels[(int)channel].Play();
    }

    public void PlayMusic(Sound sound)
    {
        channels[(int)Channel.MUSIC].clip = audioClips[(int)sound];
        channels[(int)Channel.MUSIC].volume = 0.25f;
        channels[(int)Channel.MUSIC].loop = true;
        channels[(int)Channel.MUSIC].Play();
    }
}
