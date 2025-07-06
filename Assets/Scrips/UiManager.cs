using System.IO;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameManager gameManager;
    public ButtonItem levelSelectButtonPrefab;
    public Transform levelSelectParent;

    void Start()
    {
       var Files = Directory.EnumerateFiles(Application.persistentDataPath);
        foreach (var file in Files)
        {
            var buttonItem = Instantiate(levelSelectButtonPrefab);
            buttonItem.button.onClick.AddListener(() => LoadLevel(file));
            buttonItem.text.text = Path.GetFileName(file);
            buttonItem.transform.parent = levelSelectParent;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            levelSelectParent.gameObject.SetActive(!levelSelectParent.gameObject.activeSelf);
        }
    }

    void LoadLevel(string file)
    {
        gameManager.LoadFromSave(file);
        levelSelectParent.gameObject.SetActive(false);
    }
}
