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
    private HashSet<TreeInstance> tempTrees;
    private Terrain terrain;
    private List<TreeInstance> oriTrees;
    private void Start()
    {
        
        terrain = GetComponent<Terrain>();
        trees = terrain.terrainData.treeInstances;
        terrainData = terrain.terrainData;
        oriTrees = new List<TreeInstance>();
        foreach(TreeInstance t in trees)
        {
            oriTrees.Add(t);
        }

        //
        //MAKE A COPY, DELETE TREE OR REPLACE, THEN SET TERRAINDATA>TREEINSTANDCES AS COPY

    }
    public void checkTrees()
    {
        
        tempTrees = new HashSet<TreeInstance>();
        float min = float.MaxValue;
        
        foreach (TreeInstance t in trees)
        {

            if (t.prototypeIndex == 1 || t.prototypeIndex == 2 || t.prototypeIndex == 3)
            {
                var treeInstancePos = t.position;
                var localPos = new Vector3(treeInstancePos.x * terrainData.size.x, treeInstancePos.y * terrainData.size.y, treeInstancePos.z * terrainData.size.z);
                var worldPos = transform.TransformPoint(localPos);
                if (Vector3.Distance(player.transform.position, worldPos) > 3)
                {
                    tempTrees.Add(t);
                }
                else
                {
                    if (t.prototypeIndex == 3)
                    {
                        GameManager.Instance.addToInventory("Collectable", "plum");
                    }
                    else if (t.prototypeIndex == 2)
                    {
                        GameManager.Instance.addToInventory("Collectable", "pear");
                    }
                    if (t.prototypeIndex == 1)
                    {
                        GameManager.Instance.addToInventory("Collectable", "apple");
                    }
                }
            }
            else
            {
               tempTrees.Add(t);
            }
        }
        Debug.Log(tempTrees.Count);
        terrain.terrainData.treeInstances = tempTrees.ToArray();
        trees = terrain.terrainData.treeInstances;
    }
    
    
    // Start is called before the first frame update

    public void OnApplicationQuit()
    {
        terrain.terrainData.treeInstances = oriTrees.ToArray();
    }

}
