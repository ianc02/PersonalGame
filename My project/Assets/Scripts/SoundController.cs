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
    public GameObject grasssteps;
    public GameObject stonesteps;

    private List<AudioSource> grassStepsList;
    private List<AudioSource> stoneStepsList;
    private List<AudioSource> currentList;
    private int step = 0;
    void Start()
    {
        StartCoroutine(FadeInMusic(FieldsMusic));
        StartCoroutine(forestMusicCheck());
        foreach (Transform child in grasssteps.transform)
        {
            grassStepsList.Add(child.transform.gameObject.GetComponent<AudioSource>());

        }
        foreach (Transform child in stonesteps.transform)
        {
            stoneStepsList.Add(child.transform.gameObject.GetComponent<AudioSource>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator footsteps()
    {
        while (currentList[step%4].isPlaying)
        {
            yield return null;
        }
        if (currentMusic==CavernMusic || currentMusic == WaterMusic) { currentList = stoneStepsList; }
        else { currentList = grassStepsList; }
        step += 1;
        if (Input.GetKey("w")) { currentList[step].Play(); }
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
                Debug.Log("bleep");
                StartCoroutine(FadeOutMusic(currentMusic));
                StartCoroutine(FadeInMusic(ForestMusic));
            }
            else if ((RenderSettings.fogDensity == 0f && currentMusic == ForestMusic))
            {
                Debug.Log("Fuck");
                StartCoroutine(FadeOutMusic(currentMusic));
                StartCoroutine(FadeInMusic(FieldsMusic));
            }
            else
            {
                Debug.Log(RenderSettings.fogDensity);
            }
        }
    }
}
