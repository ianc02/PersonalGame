using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;
using UnityEditor;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // Start is called before the first frame update
    public Canvas inventory;
    public Canvas pauseMenu;
    public Canvas shopCanvas;
    public Canvas statusCanvas;
    public Canvas waterlevelCanvas;
    public GameObject postProcessing;
    public GameObject player;
    public GameObject camtalk;
    public Camera cam;
    public Terrain terrain;
    public GameObject lensOfTruthListHolder;
    public GameObject rockprefab;
    public GameObject woodprefab;
    public GameObject textBox;
    public Canvas dialogueCanvas;
    public GameObject lantern;
    public GameObject snorkel;
    public GameObject lensOfTruth;

    public float fogDens;
    public Material sunset;
    public Material fogsky;
    public GameObject signs;
    public GameObject coinPrefab;
    public GameObject coinsInStoreText;
    public GameObject coinsInInvText;
    public int coinCount = 0;
    public GameObject sword;
    public GameObject axe;
    public GameObject bow;
    public GameObject arrow;
    public GameObject quiver;
    public GameObject shield;
    public GameObject currentWeapon;



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
    private bool usesnorkel = false;
    private bool hasLantern = true;
    private bool hasLensOfTruth = true;
    private bool hasMedallion = true;
    private GameObject curNode;
    private GameObject prevNode;
    private GameObject nextNode;
    public GameObject ctp;
    public GameObject ctn;
    private LinkedList<GameObject> signll;
    private bool slowfog = true;
    private bool reachedEndMaze=false;
    private GameObject talkingVillager;
    private Volume volume;
    private ColorAdjustments ca;

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
        
        currentWeapon = sword;
        volume = postProcessing.GetComponent<Volume>();
        volume.profile.TryGet<ColorAdjustments>(out ca);
        progress = 0;
        lerp = false;
        dialogue = new ArrayList();
        dialogueText = textBox.GetComponent<TextMeshProUGUI>();
        pcollider = player.GetComponent<CapsuleCollider>();
        signll = new LinkedList<GameObject>();
        foreach (Transform child in signs.transform) 
        {
            signll.AddLast(child.gameObject);
        }
        StartCoroutine(fogDensity());
        StartCoroutine(waterCheck());
        StartCoroutine(coinCheck());
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
        deactivateLensOfTruth();

    }

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            currentWeapon = sword;
            sword.active = true;
            axe.active = false;
            bow.active = false;
            arrow.active = false;

        }
        if (Input.GetKeyDown("2"))
        {
            currentWeapon = bow;
            sword.active = false;
            axe.active = false;
            bow.active = true;
            arrow.active = true;

        }
        if (Input.GetKeyDown("3"))
        {
            currentWeapon = axe;
            sword.active = false;
            axe.active = true;
            bow.active = false;
            arrow.active = false;

        }
        if (Input.GetKeyDown("e"))
        {
            terrain.GetComponent<TreeCollsionDetector>().checkTrees();
        }
        if (hasLantern) {
            if (Input.GetKeyDown("l"))
            {
                if (lantern.active)
                {
                    deactivateLantern();
                }
                else
                {
                    if (!lensOfTruth.active)
                    {
                        activateLantern();
                    }
                }
            }
        }
        if (hasLensOfTruth)
        {
            if (Input.GetKeyDown("t"))
            {
                if (lensOfTruth.active)
                {
                    deactivateLensOfTruth();
                }
                else
                {
                    if (!lantern.active)
                    {
                        activateLensOfTruth();
                    }
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
                talkingVillager.GetComponent<TownsfolkBehavior>().talking = true;
                talkingVillager.transform.GetChild(0).gameObject.active = false;
                cam.transform.position = Vector3.Lerp(cam.transform.position, camtalk.transform.position, Time.deltaTime * 2f);
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camtalk.transform.rotation, Time.deltaTime * 2f);
            }
            else
            {
                if (!shopCanvas.gameObject.active)
                {
                    cam.transform.position = Vector3.Lerp(cam.transform.position, camoriginalpos, Time.deltaTime * 2f);
                    cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, camoriginalrot, Time.deltaTime * 2f);
                    player.GetComponent<Movement>().setCanMove(false);
                    if (Vector3.Distance(cam.transform.position, camoriginalpos) < 0.01)
                    {
                        lerp = false;
                        player.GetComponent<Movement>().setCanMove(true);
                        canattack = true;

                        talkingVillager.GetComponent<TownsfolkBehavior>().talking = false;
                        talkingVillager.transform.GetChild(0).gameObject.active = true;
                    }
                }
            }

        }
        
        if (talk)
        {
            
            if (clicks < dialogue.Count)
            {

               dialogueText.text = (string) dialogue[clicks];
               if (Input.GetMouseButtonDown(0))
                {
                    clicks += 1;
                }
            }
            if (clicks >= dialogue.Count)
            {
                if (talkingVillager.name.Equals("Merchant")){
                    shopCanvas.gameObject.active = true;
                    coinsInStoreText.GetComponent<TextMeshProUGUI>().text = (coinCount.ToString());
                    coinsInInvText.GetComponent<TextMeshProUGUI>().text = (coinCount.ToString());
                    pauseGame();
                }
                dialogueCanvas.gameObject.SetActive(false);
                dialogue = new ArrayList();
                talk = false;
                
            }
        }
    }

    public void turnOffShop()
    {
        shopCanvas.gameObject.active = false;
        resumeGame();
    }

    public void purchase()
    {
        GameObject selection = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        TextMeshProUGUI stringcost = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
        int cost = int.Parse(stringcost.text);
        if (coinCount >= cost)
        {
            coinCount -= cost;
            addToInventory(selection.tag, selection.name);
            Debug.Log(selection.tag);
            Debug.Log(selection.name);
            coinsInStoreText.GetComponent<TextMeshProUGUI>().SetText(coinCount.ToString());
            coinsInInvText.GetComponent<TextMeshProUGUI>().text = (coinCount.ToString());
            if (selection.name.Equals("bow") || selection.name.Equals("quiver") || selection.name.Equals("shield"))
            {
                selection.GetComponent<Button>().enabled = false;
                Image[] images = selection.GetComponentsInChildren<Image>();
                foreach(Image i in images)
                {
                    i.color = new Color(1f, 1f, 1f, 0.5f);
                }
                selection.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                selection.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }
        }
    }

    public IEnumerator waterCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            
            int layermask = 1 << 4;
            yield return wait;
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z), Vector3.up, out hit, 1000f, layermask))
            {
                player.GetComponent<Movement>().isSwimming = true;
                if (!lensOfTruth.active) { ca.colorFilter.value = new Color(.6f, 0.6f, 1f, 1f); }
                if (!usesnorkel) { player.GetComponent<HealthAndHunger>().changeHealth(-0.4f); }
                
            }
            else
            {
                if (player.GetComponent<Movement>().isSwimming) { ca.colorFilter.value = new Color(1f, 1f, 1f, 1f); }
                player.GetComponent<Movement>().isSwimming = false;
                
            }
        }
    }

    public IEnumerator coinCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            int stupidfuckingshit = 1 << 9;
            Collider[] rangeChecks = Physics.OverlapSphere(player.transform.position,2,stupidfuckingshit);
            foreach(Collider coin in rangeChecks)
            {
                Debug.Log(coin.name);
                addToInventory("Collectable", "coin");
                Destroy(coin.gameObject);
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
        canattack = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void resumeGame()
    {
        Time.timeScale = 1f;
        player.GetComponent<Movement>().setCanMove(true);
        canattack = true;
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

        if (objectTag.Equals("key"))
        {
            foreach (Transform child in waterlevelCanvas.transform.GetChild(1))
            {
                Debug.Log(child.name);
                if (child.name.Equals(objectname))
                {
                    child.transform.GetChild(0).gameObject.active = true;
                }
            }
        }
        else
        {
            foreach (Transform child in inventory.transform)
            {
                if (child.name == objectTag)
                {
                    foreach (Transform grandchild in child)
                    {
                        if (grandchild.name == objectname)
                        {
                            GameObject itemgo = grandchild.gameObject;
                            if (objectTag.Equals("WeaponsAndArmor"))
                            {
                                grandchild.GetChild(0).gameObject.active = true;
                            }
                            else if (objectTag.Equals("Special"))
                            {
                                grandchild.GetChild(0).gameObject.active = true;
                            }
                            else
                            {
                                TextMeshProUGUI text = itemgo.GetComponentInChildren<TextMeshProUGUI>();
                                string e = text.text;
                                int num = int.Parse(grandchild.gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
                                num += 1;
                                if (objectname.Equals("coin"))
                                {
                                    coinCount = num;
                                }
                                if (objectname.Equals("arrows"))
                                {
                                    num += 2;
                                    if (num > 10)
                                    {
                                        num = 10;
                                    }
                                }
                                grandchild.GetComponentInChildren<TextMeshProUGUI>().SetText(num.ToString());
                            }
                        }
                    }
                }
            }
        }
    }


    public void merchantDialogue(GameObject m)
    {
        talkingVillager = m;
        cam.GetComponent<CameraCollision>().enabled = false;
        player.GetComponent<Movement>().setCanMove(false);
        lerp = true;

        dialogue.Add("Greetings traveler, please peruse my wares if you have the coin for it.");
        clicks = 0;
        talk = true;
        canattack = false;
        dialogueCanvas.gameObject.SetActive(true);
        camoriginalpos = cam.transform.position;
        camoriginalrot = cam.transform.rotation;
    }
    public void oldWomanDialogue(GameObject ow)
    {
        talkingVillager = ow;
        cam.GetComponent<CameraCollision>().enabled = false;
        player.GetComponent<Movement>().setCanMove(false);
        lerp = true;




        if (progress == 0)
        {
            dialogue.Add("Hello young traveler, nice to see you again! You look like you could use an adventure!");
            dialogue.Add("Luckily I have just the one for you. As you may have noticed, a dark presence has started taking over.");
            dialogue.Add("The land is covered in monsters, and we need someone to clear them out.");
            dialogue.Add("If you wish to start this perilous adventure, head to the mining shack in the southwest");
            dialogue.Add("Once you grab the tool from there, head back here.");
            dialogue.Add("Good luck!");
        }
        else if (progress == 1)
        {
            dialogue.Add("Great job! Now take that lantern to the North most cave in the mountains and find the hidden treasure!");
            dialogue.Add("It's a helpful tool that will stop you from drowning in the waters.");
            dialogue.Add("You'll know the Right way.");
        }
        else if (progress == 2)
        {
            dialogue.Add("Terrific! You did well to survive that time.");
            dialogue.Add("This next part is a bit tricky. Come back if you need a reminder.");
            progress += 1;
        }
        if (progress == 3)
        {
            dialogue.Add("Your destination is the old water ruins somewhere along the bed of the river near the Northern mountains.");
            dialogue.Add("The only help I can provide is an old rune with a small poem.");
            dialogue.Add("'Eleven steps you must climb'");
            dialogue.Add("'Sword sharp, hows the mind?'");
            dialogue.Add("'Keys can come out, rain or shine'");
            dialogue.Add("'To activate? Dry.'");
            dialogue.Add("I wish you the best of luck. Remember, many have never returned from there, so make sure you have plenty of supplies for a long journey.");
        }
        else if (progress == 4)
        {
            dialogue.Add("Wonderful! That is a truly powerful item. I'm surprised you managed to get it this time and not starve to death.");
            dialogue.Add("You must head to the Southern town, then follow the path towards the forest");
            dialogue.Add("The Lens will show you the way.");
        }
        else if (progress == 5)
        {
            dialogue.Add("Way to not get lost, it's happened plenty of times before.");
            dialogue.Add("Sorry for all the tricks, it's just fun to watch you run around for no reason.");
            dialogue.Add("It's in there still, remember where to take it?");
            dialogue.Add("Cemetery East of the main town. Second row, second column. Follow the signs to where she's buried.");
            dialogue.Add("Maybe this time you'll break the cycle, but who am I kidding, we both know you can't let go.");
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


    public void activateMedallion()
    {
        hasMedallion = true;
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
    public void activateLensOfTruth()
    {
        hasLensOfTruth = true;
        lensOfTruth.active = true;
        ca.colorFilter.value = new Color(1f, 0.6f, 0.6f, 1f);
        lensOfTruthListHolder.GetComponent<lensOfTruthStuff>().lensOn();
        foreach (Transform child in signs.transform)
        {
            child.gameObject.GetComponent<MeshRenderer>().enabled = true;
            child.GetChild(0).gameObject.active = true;
        }

    }
    public void deactivateLensOfTruth()
    {
        lensOfTruth.active = false;
        ca.colorFilter.value = new Color(1f, 1f, 1f, 1f);
        lensOfTruthListHolder.GetComponent<lensOfTruthStuff>().lensOff();
        foreach (Transform child in signs.transform)
        {
            child.gameObject.GetComponent<MeshRenderer>().enabled = false;
            child.GetChild(0).gameObject.active = false;
        }
    }

    public void canusesnorkel()
    {
        usesnorkel = true;
        snorkel.active = true;
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


    private RaycastHit castray()
    {
        Vector3 screenmouseposfar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenmouseposnear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldmouseposfar = Camera.main.ScreenToWorldPoint(screenmouseposfar);
        Vector3 worldmouseposnear = Camera.main.ScreenToWorldPoint(screenmouseposnear);
        RaycastHit hit;
        Physics.Raycast(worldmouseposnear, worldmouseposfar - worldmouseposnear, out hit);
        return hit;
    }

    public void spawnCoins(int amount, Vector3 pos, Quaternion rot)
    {
        for (int i = 0; i < amount; i++) {
            float randposx = randomNumber(-1, 1);
            float randposz = randomNumber(-1, 1);
            float randrotz = randomNumber(0, 360);
            float randrotx = randomNumber(0, 360);
            float randroty = randomNumber(0, 360);
            Vector3 newpos = new Vector3(pos.x + randposx, pos.y + 1, pos.z + randposz);
            Quaternion newrot = Quaternion.Euler(rot.x + randrotx, rot.y + randroty, randrotz + rot.z);
            Instantiate(coinPrefab, newpos, newrot);
        }
    }


    public float randomNumber(float min, float max)
    {
        return Random.RandomRange(min, max);
    }






    // FOG STUFF BELOW
    
    public IEnumerator SignGone()
    {
        reachedEndMaze = true;
        WaitForSeconds s = new WaitForSeconds(0.1f);
        reachedEndMaze = true;
        slowfog = false;
        StopCoroutine(fogOut());
        reachedEndMaze = true;
        slowfog = false;
        StopCoroutine(fogDensity());
        reachedEndMaze = true;
        slowfog = false;
        Destroy(ctn);
        reachedEndMaze = true;
        slowfog = false;
        Destroy(ctp);
        reachedEndMaze = true;
        slowfog = false;
        StopCoroutine(fogOut());
        reachedEndMaze = true;
        slowfog = false;
        reachedEndMaze = true;
        StopCoroutine(fogDensity());
        reachedEndMaze = true;
        slowfog = false;

        while (RenderSettings.fogDensity > 0.02f)
        {
            yield return s;
            Debug.Log(RenderSettings.fogDensity);
            cam.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, cam.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color.a - 0.02f);
            RenderSettings.fogDensity = RenderSettings.fogDensity - 0.02f;
            if (RenderSettings.fogDensity > 0.02)
            {
                RenderSettings.skybox = fogsky;


            }
            else
            {
                RenderSettings.skybox = sunset;

            }
        }
        signs.active = false;
        RenderSettings.fogDensity = 0;
        cam.transform.GetChild(0).gameObject.active = false;
    }
    public void nodeChanger(GameObject collided)
    {
        if (collided.gameObject.name.Equals("ArrowSign (40)"))
        {
            slowfog = false;
            StartCoroutine(SignGone());
        }
        if (curNode == null)
        {
            if (collided.Equals(signll.First.Value))
            {
                curNode = collided;
                nextNode = signll.Find(curNode).Next.Value;
                ctn = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Vector3 between2 = new Vector3();
                between2 = nextNode.transform.position - curNode.transform.position;
                float dist2 = between2.magnitude;
                Vector3 scale = ctn.transform.localScale;
                scale.Set(15f, 15f, dist2 + 10f);
                ctn.transform.localScale = scale;
                ctn.transform.position = curNode.transform.position + (between2 / 2.0f);
                ctn.transform.LookAt(nextNode.transform.position);
                ctn.GetComponent<MeshRenderer>().enabled = false;
                ctn.GetComponent<BoxCollider>().isTrigger = true;
                ctn.AddComponent<PathwayColliion>();
                ctn.layer = 8;
            }

        }
        else
        {

            if (collided.Equals(signll.Find(curNode).Next.Value) || collided.Equals(signll.Find(curNode).Previous.Value))
            {
                prevNode = curNode;
                curNode = collided;
                if (ctp != null)
                {
                    Destroy(ctp);
                }
                ctp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Vector3 between = new Vector3();
                between = curNode.transform.position - prevNode.transform.position;
                float dist = between.magnitude;
                Vector3 scale = ctp.transform.localScale;
                scale.Set(15f, 15f, dist + 10f);
                ctp.transform.localScale = scale;
                ctp.transform.position = prevNode.transform.position + (between / 2.0f);
                ctp.transform.LookAt(curNode.transform.position);
                ctp.GetComponent<MeshRenderer>().enabled = false;
                ctp.GetComponent<BoxCollider>().isTrigger = true;
                ctp.AddComponent<PathwayColliion>();
                ctp.layer = 8;

                if (signll.Find(curNode).Next != null)
                {
                    nextNode = signll.Find(curNode).Next.Value;
                    Destroy(ctn);
                    ctn = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Vector3 between2 = new Vector3();
                    between2 = nextNode.transform.position - curNode.transform.position;
                    float dist2 = between2.magnitude;
                    Vector3 scale2 = ctn.transform.localScale;
                    scale2.Set(15f, 15f, dist2 + 10f);
                    ctn.transform.localScale = scale2;
                    ctn.transform.position = curNode.transform.position + (between2 / 2.0f);
                    ctn.transform.LookAt(nextNode.transform.position);
                    ctn.GetComponent<MeshRenderer>().enabled = false;
                    ctn.GetComponent<BoxCollider>().isTrigger = true;
                    ctn.AddComponent<PathwayColliion>();
                    ctn.layer = 8;

                }

            }
        }
    }

    public void outsidePathway()
    {
        bool n = false;
        bool p = false;
        if (ctn != null)
        {

            if (ctn.GetComponent<PathwayColliion>().inside)
            {
                n = true;
            }
        }
        if (ctp != null)
        {
            if (ctp.GetComponent<PathwayColliion>().inside)
            {
                p = true;
            }
        }
        if (!n && !p && !reachedEndMaze)
        {
            StartCoroutine(fogOut());
        }
    }

    public void inPathway()
    {
        StopCoroutine(fogOut());
        slowfog = true;
        RenderSettings.skybox = sunset;
    }
    public IEnumerator fogOut()
    {
        slowfog = false;
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        deactivateLantern();

        while (RenderSettings.fogDensity < 0.98f)
        {
            yield return wait;
            if (!reachedEndMaze)
            {
                RenderSettings.fogDensity = RenderSettings.fogDensity + 0.02f;
            }
            if (slowfog)
            {
                break;
            }
        }
        if (!slowfog)
        {
            Vector3 tp = new Vector3();
            tp.Set(77.6181488f, 97.7436829f, 454.7600f);
            player.transform.position = tp;
        }
        slowfog = true;
        RenderSettings.skybox = sunset;
    }
    private IEnumerator fogDensity()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        while (!reachedEndMaze)
        {
            yield return wait;
            while (slowfog)
            {
                yield return wait;
                if (!reachedEndMaze)
                {
                    if (player.transform.position.z < 400)
                    {
                        if (player.transform.position.x < 400)
                        {

                            //signs.active = true;
                            cam.transform.GetChild(0).gameObject.active = true;
                            float density = Mathf.Min((Mathf.Pow(((50 - (Mathf.Max(player.transform.position.x, player.transform.position.z) - 350)) / 50f), 3) * fogDens), fogDens);

                            cam.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, Mathf.Min((Mathf.Pow(((50 - (Mathf.Max(player.transform.position.x, player.transform.position.z) - 350)) / 50f), 2)), 1));
                            if (!reachedEndMaze)
                            {
                                RenderSettings.fogDensity = density;
                            }
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
                            //signs.active = false;
                            RenderSettings.fogDensity = 0;
                            cam.transform.GetChild(0).gameObject.active = false;
                        }
                    }
                    else
                    {
                        //signs.active = false;
                        RenderSettings.fogDensity = 0;
                        cam.transform.GetChild(0).gameObject.active = false;
                    }
                }
            }
        }
    }


    public void showCross()
    {
        statusCanvas.gameObject.transform.GetChild(4).gameObject.active = true;
    }
    public void hideCross()
    {
        statusCanvas.gameObject.transform.GetChild(4).gameObject.active = false;
    }

}
