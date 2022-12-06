using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    public Material sunset;
    public Material black;
    public GameObject light;
    public GameObject cavernprops;
    public GameObject cavernMobs;
    
    // Start is called before the first frame update
    public Vector3 pos;
    
    public IEnumerator waiter()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        yield return wait;
        while (true)
        {
            yield return wait;
            if (GameManager.Instance.getPlayer().transform.position.y < -310)
            {
                break;
            }
            
        }
        GameManager.Instance.getPlayer().GetComponent<Movement>().canMove = true;
        GameManager.Instance.waterLevel.GetComponent<WaterKeys>().speed = 0.4f;
        GameManager.Instance.waterLevel.GetComponent<WaterKeys>().playerFollow = false;
        GameManager.Instance.getPlayer().gameObject.GetComponent<Movement>().gravity = 30;
    }
    public void OnTriggerEnter(Collider other)
    {
        SoundController musicController = GameManager.Instance.soundController.GetComponent<SoundController>();
        AudioSource curMusic = musicController.currentMusic;
        string curMusicName = curMusic.name;
        if (other.CompareTag("Player"))
        {
            if (curMusicName.Equals("CavernMusic") || curMusicName.Equals("WaterMusic")){
                StartCoroutine(musicController.FadeOutMusic(curMusic));
                StartCoroutine(musicController.FadeInMusic(musicController.FieldsMusic));

            }
            if (pos.y < 0)
            {
                RenderSettings.ambientIntensity = 0;
                RenderSettings.skybox = null;
                light.active = false;
                cavernprops.active = true;
                cavernMobs.active = true;
                StartCoroutine(musicController.FadeOutMusic(curMusic));
                StartCoroutine(musicController.FadeInMusic(musicController.CavernMusic));
            }
            else
            {
                RenderSettings.skybox = sunset;
                RenderSettings.ambientIntensity = 1;

                light.active = true;
                cavernprops.active = false;
                cavernMobs.active = false;
            }
            if (pos.z > 1300)
            {
                other.gameObject.GetComponent<Movement>().canMove = false;
                other.gameObject.GetComponent<Movement>().gravity = 0;
                GameManager.Instance.waterLevel.GetComponent<WaterKeys>().playerFollow = true;
                GameManager.Instance.waterLevel.GetComponent<WaterKeys>().calcDist();
                GameManager.Instance.waterLevel.GetComponent<WaterKeys>().speed = 1f;
                StartCoroutine(musicController.FadeOutMusic(curMusic));
                StartCoroutine(musicController.FadeInMusic(musicController.WaterMusic));
                StartCoroutine(waiter());
            }
            
            other.transform.position = pos;
        }
    }
    
}
