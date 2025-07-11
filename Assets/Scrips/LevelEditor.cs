using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    [Header("Editor References")]
    public TMP_InputField levelNameInput;
    public Button saveButton;
    public TMP_InputField gridWidthInput;
    public TMP_InputField gridHeightInput;
    public FreeCam FreeCam;

    [Header("Grid Settings")]
    public Grid grid;
    public float interactDistance;
    public LayerMask layerMask;

    [Header("Runtime variables")]
    public CellNavigationType currentNavigationType;
    private Camera camera;
    private GridPath path;

    private void Awake()
    {
        gridWidthInput.text = grid.width.ToString();
        gridHeightInput.text = grid.height.ToString();
    }

    void Start()
    {
        path = new GridPath();
        SetCameraPosition();
        saveButton.onClick.AddListener(Save);
        // dont let the save button be interactable until a level name is entered
        saveButton.interactable = false;
        levelNameInput.onEndEdit.AddListener(OnLevelNameEdited);
        levelNameInput.onSelect.AddListener((x)=>ToggleFreeCam(false));
        levelNameInput.onDeselect.AddListener((x) => ToggleFreeCam(true));
        gridWidthInput.onEndEdit.AddListener(OnGridWidthEdited);
        gridHeightInput.onEndEdit.AddListener(OnGridHeightEdited);

        grid.GenerateGrid();
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        var mousePosition = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out var hit, interactDistance, layerMask))
        {
            return;
        }

        var cell = hit.transform.gameObject.GetComponent<Cell>();
        Debug.Log(hit.transform.gameObject.name);
        Debug.DrawLine(ray.origin, ray.direction * interactDistance, Color.blue, 5);
        
        if (cell == null)
        {
            return;
        }

        var clickedPosition = cell.gridPosition;
        Node lastNode = null;
        var newNode = new Node(){
            pathOrder = path.nodes.Count,
            gridPosition = new Vec2Int(){
                x = cell.gridPosition.x, 
                y = cell.gridPosition.y
            }
        };

        if(path.nodes.Count > 0){
            lastNode = path.nodes[path.nodes.Count - 1];
        }
        else{
            grid.RecalculatePath(null, newNode, path);
            path.nodes.Add(newNode);
            return;
        }
        var lastPosition = lastNode.gridPosition;

        // remove the last node if you click on it
        if(clickedPosition.x == lastPosition.x && clickedPosition.y == lastPosition.y){
            var lastIndex = path.nodes.Count - 1;
            path.nodes.RemoveAt(lastIndex);
            grid.RecalculatePath(path.nodes[lastIndex], path.nodes[lastIndex], path);
            return;
        }

        var horizontalMove = Mathf.Abs(clickedPosition.x - lastPosition.x);
        var verticalMove = Mathf.Abs(clickedPosition.y - lastPosition.y);
        var isAdjacentMove = (horizontalMove == 1 && verticalMove == 0) || (horizontalMove == 0 && verticalMove == 1);
        var isClearBlock = cell.navigationType == CellNavigationType.None;
        
        // only allow adding adjacent blocks, ignore diagonals
        if(!isAdjacentMove || !isClearBlock){
            return;
        }

        grid.RecalculatePath(lastNode, newNode, path);
        path.nodes.Add(newNode);
    }
    
    void Save()
    {
        if(path == null || path.nodes.Count == 0)
        {
            Debug.LogWarning("No path set, cannot save level.");
            return;
        }

        //get a list of all spawned objects (position, type)
        var SavedLevel = new SavedLevel()
        {
            gridPath = path,
            gridSettings = new GridSettings()
            {
                width = grid.width,
                height = grid.height,
                cellSize = Vec3.FromVector3(grid.cellSize)
            }
        };
        //write it to a file
        var fileName = Path.Combine(Application.persistentDataPath, levelNameInput.text);
        var fileData = JsonConvert.SerializeObject(SavedLevel);
        File.WriteAllText(fileName, fileData);

        path = new GridPath();
        grid.GenerateGrid();
    }

    void OnLevelNameEdited(string newText)
    {
        saveButton.interactable = !string.IsNullOrEmpty(levelNameInput.text);
    }

    void OnGridWidthEdited(string newText)
    {
        if (int.TryParse(newText, out int width))
        {
            grid.width = width;
            grid.GenerateGrid();
        }
    }

    void OnGridHeightEdited(string newText)
    {
        if (int.TryParse(newText, out int height))
        {
            grid.height = height;
            grid.GenerateGrid();
        }
    }
    void SetCameraPosition()
    {
        if(camera ==null)
        {
            camera = Camera.main;
        }

        float x = grid.TotalGridWidth / 2;
        float y = grid.TotalGridHeight / 2;
        camera.transform.position = new Vector3(x,10,y);
        camera.transform.rotation = Quaternion.Euler(90,0,0);

    }

    void ToggleFreeCam(bool CanMove)
    {

        FreeCam.CanMove = CanMove;
    }
}