using System.IO;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameManager gameManager;
    public ButtonItem levelSelectButtonPrefab;
    public Transform levelButtonContainer;
    public GameObject levelSelectPanel;

    void Start()
    {
        levelSelectPanel.SetActive(true);
        
        var Files = FileUtils.GetFilesInAppDataFolder("Levels");
        foreach (var file in Files)
        {
            var buttonItem = Instantiate(levelSelectButtonPrefab);
            buttonItem.button.onClick.AddListener(() => LoadLevel(file));
            buttonItem.text.text = Path.GetFileName(file);
            buttonItem.transform.parent = levelButtonContainer;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            levelSelectPanel.gameObject.SetActive(!levelSelectPanel.gameObject.activeSelf);
        }
    }

    void LoadLevel(string file)
    {
        var fileName = Path.GetFileName(file);
        gameManager.LoadFromSave(fileName);
        levelSelectPanel.gameObject.SetActive(false);
    }
}
