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
    public AudioSource hitSound;
    public AudioSource collectSound;
    public GameObject grasssteps;
    public GameObject stonesteps;
    public float stepSpeed;
    public float runSpeed;

    private List<AudioSource> grassStepsList;
    private List<AudioSource> stoneStepsList;
    private List<AudioSource> currentList;
    private int step = 0;
    public GameObject player;
    void Start()
    {
        StartCoroutine(FadeInMusic(MainMenuMusic));
        StartCoroutine(forestMusicCheck());
        grassStepsList = new List<AudioSource>();
        stoneStepsList = new List<AudioSource>();
        foreach (Transform child in grasssteps.transform)
        {
            grassStepsList.Add(child.transform.gameObject.GetComponent<AudioSource>());

        }
        foreach (Transform child in stonesteps.transform)
        {
            stoneStepsList.Add(child.transform.gameObject.GetComponent<AudioSource>());
        }
        currentList = grassStepsList;
        StartCoroutine(footsteps());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator footsteps()
    {
        WaitForSeconds wait = new WaitForSeconds(0.07f);
        while (true)
        {
            yield return wait;
            if (!player.GetComponent<Movement>().isSwimming)
            {
                if (!currentList[step % 4].isPlaying)
                {
                    if (currentMusic == CavernMusic || currentMusic == WaterMusic) { currentList = stoneStepsList; }
                    else { currentList = grassStepsList; }
                    step += 1;
                    if (Input.GetKey("w"))
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            currentList[step % 4].pitch = runSpeed;
                        }
                        else
                        {
                            currentList[step % 4].pitch = stepSpeed;
                        }
                        currentList[step % 4].Play();
                    }
                }
            }
            
        }
    }
    public IEnumerator FadeOutMusic(AudioSource source)
    {
        WaitForSeconds wait = new WaitForSeconds(0.04f);
        bool done = false;
        while (!done)
        {
            yield return wait;
            source.volume *= 0.94f;
            if (source.volume < 0.03f)
            {
                done = true;
                source.Stop();
            }

        }
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
            if (source.volume > 0.52f)
            {
                done = true;
                source.volume = 0.55f;
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
                StartCoroutine(FadeOutMusic(currentMusic));
                StartCoroutine(FadeInMusic(ForestMusic));
            }
            else if ((RenderSettings.fogDensity == 0f && currentMusic == ForestMusic))
            {
                StartCoroutine(FadeOutMusic(currentMusic));
                StartCoroutine(FadeInMusic(FieldsMusic));
            }
        }
    }

    public void playHitSound()
    {
        hitSound.Play();
    }

    public void playCollectSound()
    {
        collectSound.Play();
    }
}
