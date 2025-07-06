using Newtonsoft.Json;
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
    public TMP_Dropdown blockTypeDropdown;
    public TMP_InputField gridWidthInput;
    public TMP_InputField gridHeightInput;

    [Header("Grid Settings")]
    public Grid grid;
    public float interactDistance;
    public LayerMask layerMask;

    [Header("Runtime variables")]
    public CellNavigationType currentNavigationType;

    private void Awake()
    {
        gridWidthInput.text = grid.width.ToString();
        gridHeightInput.text = grid.height.ToString();
    }

    void Start()
    {
        saveButton.onClick.AddListener(Save);
        // dont let the save button be interactable until a level name is entered
        saveButton.interactable = false;
        levelNameInput.onEndEdit.AddListener(OnLevelNameEdited);
        blockTypeDropdown.onValueChanged.AddListener(BlockTypeChanged);
        gridWidthInput.onEndEdit.AddListener(OnGridWidthEdited);
        gridHeightInput.onEndEdit.AddListener(OnGridHeightEdited);

        grid.GenerateGrid();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out var hit, interactDistance, layerMask))
            {
                var cell = hit.transform.gameObject.GetComponent<Cell>();
                Debug.Log(hit.transform.gameObject.name);
                Debug.DrawLine(ray.origin, ray.direction * interactDistance, Color.blue, 5);
                if (cell != null)
                {
                    cell.SetNavigationType(currentNavigationType);
                }
            }
        }

        TrySetBlockType();
    }

    // Set nav type using hotkeys
    void TrySetBlockType()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentNavigationType = CellNavigationType.Path;
            blockTypeDropdown.value = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentNavigationType = CellNavigationType.Start;
            blockTypeDropdown.value = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentNavigationType = CellNavigationType.Finish;
            blockTypeDropdown.value = 2;
        }
    }
    
    void Save()
    {
        var gridPath = grid.GetGridPath();

        if(gridPath == null || gridPath.Count == 0)
        {
            Debug.LogWarning("No path set, cannot save level.");
            return;
        }

        if(gridPath.Where(node => node.cellNavigationType == CellNavigationType.Start).Count() > 1)
        {
            Debug.LogWarning("Multiple start nodes found, cannot save level.");
            return;
        }

        if (gridPath.Where(node => node.cellNavigationType == CellNavigationType.Finish).Count() > 1)
        {
            Debug.LogWarning("Multiple finish nodes found, cannot save level.");
            return;
        }

        //get a list of all spawned objects (position, type)
        var SavedLevel = new SavedLevel()
        {
            gridPath = gridPath,
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
    }

    void OnLevelNameEdited(string newText)
    {
        saveButton.interactable = !string.IsNullOrEmpty(levelNameInput.text);
    }

    void BlockTypeChanged(int value)
    {
        currentNavigationType = (CellNavigationType) value;
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
}
