using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeCollsionDetector : MonoBehaviour
{
    public GameObject player;
    TreeInstance[] trees;
    TreeInstance[] apples;
    TreeInstance[] pears;
    TreeInstance[] plums;
    private void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        trees = terrain.terrainData.treeInstances;
        apples = trees.Where(
            t => t.prototypeIndex == 1
            ).ToArray();
        pears = trees.Where(
            t => t.prototypeIndex == 2
            ).ToArray();
        plums = trees.Where(
            t => t.prototypeIndex == 3
            ).ToArray();

    }
    public IEnumerator checkTrees()
    {
        WaitForSeconds wait = new WaitForSeconds(0.3f);
        while (true)
        {
            yield return wait;
            foreach (TreeInstance tree in apples)
            {
                Debug.Log(Vector3.Distance(tree.position, player.transform.position));
            }
            foreach (TreeInstance tree in pears)
            {
                Debug.Log(Vector3.Distance(tree.position, player.transform.position));
            }
            foreach (TreeInstance tree in plums)
            {
                Debug.Log(Vector3.Distance(tree.position, player.transform.position));
            }
        }
    }
    private void Awake()
    {
        

    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("What");
        if (other.CompareTag("Player"))
        {
            Debug.Log("HOLY FUCK THIS WSORKS");
        }
    }
}
