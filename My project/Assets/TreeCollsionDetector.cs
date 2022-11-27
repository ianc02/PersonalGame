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
    private TerrainData terrainData;
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
        Debug.Log(apples.Length);
        Debug.Log(pears.Length);
        Debug.Log(plums.Length);
        terrainData = terrain.terrainData;
        StartCoroutine(checkTrees());
        //
        //MAKE A COPY, DELETE TREE OR REPLACE, THEN SET TERRAINDATA>TREEINSTANDCES AS COPY

    }
    public IEnumerator checkTrees()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (true)
        {
            yield return wait;
            foreach (TreeInstance tree in apples)
            {
                var treeInstancePos = tree.position;
                var localPos = new Vector3(treeInstancePos.x * terrainData.size.x, treeInstancePos.y * terrainData.size.y, treeInstancePos.z * terrainData.size.z);
                var worldPos = Terrain.activeTerrain.transform.TransformPoint(localPos);
                //Debug.Log(Vector3.Distance(tree.position, player.transform.position));
                if (Vector3.Distance(worldPos, player.transform.position) < 5)
                {
                    Debug.Log("APPLESSS");
                }
                
            }
            foreach (TreeInstance tree in pears)
            {
                var treeInstancePos = tree.position;
                var localPos = new Vector3(treeInstancePos.x * terrainData.size.x, treeInstancePos.y * terrainData.size.y, treeInstancePos.z * terrainData.size.z);
                var worldPos = Terrain.activeTerrain.transform.TransformPoint(localPos);
                //Debug.Log(Vector3.Distance(tree.position, player.transform.position));
                if (Vector3.Distance(worldPos, player.transform.position) < 5)
                {
                    Debug.Log("PEAARSSS");
                }
                
                
            }
            foreach (TreeInstance tree in plums)
            {
                var treeInstancePos = tree.position;
                var localPos = new Vector3(treeInstancePos.x * terrainData.size.x, treeInstancePos.y * terrainData.size.y, treeInstancePos.z * terrainData.size.z);
                var worldPos = Terrain.activeTerrain.transform.TransformPoint(localPos);
                if (Vector3.Distance(worldPos, player.transform.position) < 5)
                {
                    Debug.Log("PLUMSSSS");
                    Destroy(tree);
                }
            }
        }
    }
    private void Awake()
    {
        

    }
    // Start is called before the first frame update
    
}
