using UnityEngine;

public enum CellNavigationType
{
    Start = 0,
    Path = 1,
    Finish = 2,
    None = 3
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
        navigationType = CellNavigationType.None;
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
        return navigationType != CellNavigationType.None; 
    }
}
