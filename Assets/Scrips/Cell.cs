using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject Prefab;
    private GameObject spawnedObject;
    public int BlockType;
    public Material path;
    public Material start;
    public Material finish;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setBlocktype(int type)
    {
        BlockType = type;
        var selectedmaterial = path;
        if (type == 0)
        {
            selectedmaterial = start;
        }
        else if (type == 1)
        {
            selectedmaterial = path;
        }
        else
        {
            selectedmaterial = finish;
        }
        spawnedObject.GetComponent<MeshRenderer>().material = selectedmaterial;
    }

    public void Toggle()
    {
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(Prefab);
            spawnedObject.transform.parent = transform;
            spawnedObject.transform.localPosition = new Vector3(0, 1, 0);
        }
        else if(spawnedObject.activeInHierarchy)
        {
            spawnedObject.SetActive(false);
        }
        else
        {
            spawnedObject.SetActive(true);
        }
        
    }
    public bool hasChild()
    { return spawnedObject != null && spawnedObject.activeInHierarchy; }


}
