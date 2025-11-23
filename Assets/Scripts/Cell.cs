using System;
using UnityEngine;
using UnityEngine.Rendering;

public enum CellNavigationType
{
    None,
    Start,
    Path,
    Finish,
}

public class Cell : MonoBehaviour
{
    public GameObject spawnedObjectPrefab;

    public Vector2Int gridPosition;
    public CellNavigationType navigationType;
    private MeshRenderer meshRenderer;
    public Material path;
    public Material start;
    public Material finish;
    public Material defaultMaterial;

    private GameObject spawnedObject;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {

    }

    public void SetNavigationType(CellNavigationType type)
    {
        if(meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        navigationType = type;

        if (type == CellNavigationType.Start)
        {
            meshRenderer.material = start;
        }
        else if (type == CellNavigationType.Path)
        {
            meshRenderer.material = path;
        }
        else if (type == CellNavigationType.Finish)
        {
            meshRenderer.material = finish;
        }
        else
        {
            meshRenderer.material = defaultMaterial;
        }
    }

    public void Toggle()
    {
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(spawnedObjectPrefab);
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

    public bool HasNavigationType()
    { 
        bool hasNavigationType = navigationType != CellNavigationType.None; 
        return hasNavigationType;
    }

    public override string ToString()
    {
        return $"{gridPosition.x},{gridPosition.y},{navigationType}";
    }
}
