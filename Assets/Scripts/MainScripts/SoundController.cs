using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _audios;
    [SerializeField] private float _volume;


    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        StartCoroutine(StartMusicSoundPad(_volume, 10, 10));
    }

    public void StartManually()
    {
        _audioSource = GetComponent<AudioSource>();

        StartCoroutine(StartMusicSoundPad(_volume, 10, 10));
    }

    public void Stop()
    {
        _audioSource.mute = true;
    }

    public void Play()
    {
        _audioSource.mute = false;
    }

    private AudioClip RandomClip()
    {
        return _audios[Random.Range(0, _audios.Length)];
    }

    private IEnumerator StartMusicSoundPad(float volume, float delay, float changeSpeed)
    {
        while (true)
        {
            StartCoroutine(StartSong(RandomClip(), volume, changeSpeed));

            yield return new WaitUntil(() => !_audioSource.isPlaying);

            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator StartSong(AudioClip song, float volume, float changeSpeed)
    {
        float timeLeft = changeSpeed;

        _audioSource.volume = 0;

        _audioSource.clip = song;

        _audioSource.Play();

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            _audioSource.volume = volume * (1f - timeLeft / changeSpeed);

            yield return new WaitForEndOfFrame();
        }
        _audioSource.volume = volume;

        yield return new WaitUntil(() => _audioSource.time >= (song.length - changeSpeed));

        timeLeft = changeSpeed;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            _audioSource.volume = volume * (timeLeft / changeSpeed);

            yield return new WaitForEndOfFrame();
        }

        _audioSource.Stop();
    }
}
