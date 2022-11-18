using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEditor;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // Start is called before the first frame update
    public Canvas inventory;
    public Canvas pauseMenu;
    public GameObject player;
    public GameObject camtalk;
    public Camera cam;
    public Terrain terrain;
    public GameObject rockprefab;
    public GameObject woodprefab;
    public GameObject textBox;
    public Canvas dialogueCanvas;
    public GameObject lantern;
    public GameObject snorkel;
    public float fogDens;
    public Material sunset;
    public Material fogsky;
    private Collider pcollider;
    private int progress;
    private bool lerp;
    private TextMeshProUGUI dialogueText;
    private bool talk = false;
    private ArrayList dialogue;
    private int clicks;
    private bool canattack = true;
    private Vector3 camoriginalpos;
    private Quaternion camoriginalrot;
    private bool usensorkel = false;
    private bool hasLantern = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        progress = 0;
        lerp = false;
        dialogue = new ArrayList();
        dialogueText = textBox.GetComponent<TextMeshProUGUI>();
        pcollider = player.GetComponent<CapsuleCollider>();
        StartCoroutine(fogDensity());
        RaycastHit hit;
        
        for (int i = 0; i < 5001; i++)
        {
            Vector3 pos = new Vector3(Random.Range(2, 1998), 700, Random.Range(2, 1998));
            if (Physics.Raycast(pos, Vector3.down, out hit))
            {
                if (hit.transform.tag != "terrain")
                {
                    i -= 1;
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        Instantiate(rockprefab, hit.point, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(woodprefab, hit.point, Quaternion.identity);
                    }
                } 
            }
                
            
        }
    }
    private IEnumerator fogDensity()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        while (true)
        {
            yield return wait;
            if (player.transform.position.z < 400)
            {
                if (player.transform.position.x < 400)
                {
                    float density = Mathf.Min((Mathf.Pow(((50 -(Mathf.Max(player.transform.position.x, player.transform.position.z)-350)) / 50f),3) * fogDens), fogDens);
                    RenderSettings.fogDensity = density;
                    if (density > 0.1)
                    {
                        RenderSettings.skybox = fogsky;

                    }
                    else
                    {
                        RenderSettings.skybox = sunset;
                    }
                }
                else
                {
                    RenderSettings.fogDensity = 0;
                }
            }
            else
            {
                RenderSettings.fogDensity = 0;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if (hasLantern) {
            if (Input.GetKeyDown("l"))
            {
                if (lantern.active)
                {
                    deactivateLantern();
                }
                else
                {
                    activateLantern();
                }
            }
        }
        if (Input.GetKeyDown("q"))
        {
            if (inventory.gameObject.active)
            {
                inventory.gameObject.SetActive(false);
                resumeGame();
            }
            else
            {
                inventory.gameObject.SetActive(true);
                pauseGame();
            }
        }
        else if (Input.GetKeyDown("escape"))
        {
            if (pauseMenu.gameObject.active)
            {
                pauseMenu.gameObject.SetActive(false);
                resumeGame();
            }
            else
            {
                pauseMenu.gameObject.SetActive(true);
                pauseGame();
            }

           
        }
        
        if (lerp)
        {
            if (talk)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, camtalk.transform.position, Time.deltaTime * 2f);
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camtalk.transform.rotation, Time.deltaTime * 2f);
            }
            else
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, camoriginalpos, Time.deltaTime * 2f);
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camoriginalrot, Time.deltaTime * 2f);
                if (Vector3.Distance(cam.transform.position, camoriginalpos) < 0.01)
                {
                    lerp = false;
                    player.GetComponent<Movement>().setCanMove(true);
                    canattack = true;
                }
            }

        }
        
        if (talk)
        {
            Debug.Log(1);
            
            if (clicks < dialogue.Count)
            {

               dialogueText.text = (string) dialogue[clicks];
               if (Input.GetMouseButtonDown(0))
                {
                    clicks += 1;
                }
            }
            else
            {
                dialogueCanvas.gameObject.SetActive(false);
                dialogue = new ArrayList();
                talk = false;
            }
        }
        


        

    }

    public GameObject getPlayer()
    {
        return player;
    }
    public void pauseGame()
    {
        Time.timeScale = 0f;
        player.GetComponent<Movement>().setCanMove(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void resumeGame()
    {
        Time.timeScale = 1f;
        player.GetComponent<Movement>().setCanMove(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void removePauseMenu()
    {
        pauseMenu.gameObject.SetActive(false);
    }
    public Camera getcamera()
    {
        return cam;
    }

    public void addToInventory(string objectTag, string objectname)
    {

        Debug.Log(objectTag);
        Debug.Log(objectname);
        
        foreach (Transform child in inventory.transform)
        {
            Debug.Log(0);
            Debug.Log(child.name);
            if (child.name == objectTag)
            {
                Debug.Log(1);
                foreach(Transform grandchild in child)
                {
                    if (grandchild.name == objectname)
                    {
                        Debug.Log(2);
                        Debug.Log(grandchild.name);
                        GameObject t = grandchild.gameObject;
                        TextMeshProUGUI r = t.GetComponentInChildren<TextMeshProUGUI>();
                        string e = r.text;
                        int num = int.Parse(grandchild.gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
                        num += 1;
                        grandchild.GetComponentInChildren<TextMeshProUGUI>().SetText(num.ToString());
                    }
                }
            }
        }
    }



    public void oldWomanDialogue()
    {

        cam.GetComponent<CameraCollision>().enabled = false;
        player.GetComponent<Movement>().setCanMove(false);
        lerp = true;




        if (progress == 0)
        {
            dialogue.Add("Hello young traveler, nice to see you again! You look like you could use an adventure!");
            dialogue.Add("Luckily I have just the one for you. As you may have noticed, a dark presence has started taking over.");
            dialogue.Add("The land is covered in monsters, and we need someone to clear them out.");
            dialogue.Add("If you wish to start this perilous adventure, head to the mining shack in the southwest");
            dialogue.Add("Once you grab the tool from there, head to the north most cave and survive.");
            dialogue.Add("Good luck!");
        }
        else if (progress == 1)
        {
            dialogue.Add("Great job! Now take that lantern to the North most cave in the mountains and find the hidden treasure!");
        }
        clicks = 0;
        talk = true;
        canattack = false;
        dialogueCanvas.gameObject.SetActive(true);
        camoriginalpos = cam.transform.position;
        camoriginalrot = cam.transform.rotation;


    }

    public bool canAttack()
    {
        return canattack;
    }


    public void activateLantern()
    {
        hasLantern = true;
        lantern.active = true;
    }
    public void deactivateLantern()
    {
        lantern.active = false;
    }

    public void canusesnorkel()
    {
        usensorkel = true;
    }
    public void activateSnorkel()
    {
        
        lantern.active = true;
    }
    public void deactivateSnorkel()
    {
        lantern.active = false;
    }


    public void loadScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    public void addProgress()
    {
        progress += 1;
    }
}
