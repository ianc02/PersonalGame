using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource currentMusic;
    public AudioSource MainMenuMusic;
    public AudioSource FieldsMusic;
    public AudioSource CavernMusic;
    public AudioSource WaterMusic;
    public AudioSource ForestMusic;
    void Start()
    {
        StartCoroutine(FadeInMusic(FieldsMusic));
        StartCoroutine(forestMusicCheck());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeOutMusic(AudioSource source)
    {
        WaitForSeconds wait = new WaitForSeconds(0.04f);
        bool done = false;
        while (!done)
        {
            yield return wait;
            source.volume *= 0.95f;
            if (source.volume < 0.03f)
            {
                done = true;
                source.Stop();
            }

        }
    }
    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }

    public IEnumerator FadeInMusic(AudioSource source)
    {
        currentMusic = source;
        WaitForSeconds wait = new WaitForSeconds(0.04f);
        bool done = false;
        source.volume = 0.03f;
        source.Play();
        while (!done)
        {
            yield return wait;
            source.volume *= 1.05f;
            if (source.volume > 0.95f)
            {
                done = true;
                source.volume = 1f;
            }

        }
    }

    public IEnumerator forestMusicCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (true)
        {
            yield return wait;
            if (RenderSettings.fogDensity > 0f && currentMusic!=ForestMusic)
            {
                FadeOutMusic(currentMusic);
                FadeInMusic(ForestMusic);
            }
            else if ((RenderSettings.fogDensity == 0f && currentMusic == ForestMusic))
            {
                FadeOutMusic(currentMusic);
                FadeInMusic(FieldsMusic);
            }
        }
    }
}
