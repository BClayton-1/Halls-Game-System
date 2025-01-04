using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource gameAudio;

    public Object[] musicDatabase;



    public void Start()
    {
        musicDatabase = Resources.LoadAll("Sounds/BG Music", typeof(AudioClip));
    }

    public void PlayMusic(string trackName)
    {
        AudioClip audioClip = null;

        foreach (Object _object in musicDatabase)
        {
            if (_object != null && _object.name == trackName)
            {
                audioClip = (AudioClip)_object;
                StartCoroutine(PlayMusicCor(audioClip));
                return;
            }
        }

        StartCoroutine(PlayMusicCor(audioClip));
    }

    public IEnumerator PlayMusicCor(AudioClip audioClip)
    {
        if (gameAudio.clip != null)
        {
            for (float i = 1.0f; i >= 0; i -= 0.03f)
            {
                gameAudio.volume = i;
                yield return new WaitForSeconds(0.03f);
            }
        }
        gameAudio.volume = 0;
        gameAudio.clip = audioClip;
        gameAudio.Play();
        for (float i = 0.0f; i <= 1; i += 0.03f)
        {
            gameAudio.volume = i;
            yield return new WaitForSeconds(0.03f);
        }
        gameAudio.volume = 1;
    }

}
