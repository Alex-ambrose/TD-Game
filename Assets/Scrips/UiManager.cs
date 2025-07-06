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
            var button = Instantiate(levelSelectButtonPrefab);
            button.button.onClick.AddListener(() => LoadLevel(file));
            button.text.text = Path.GetFileName(file);
            button.transform.parent = levelSelectParent;
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
        gameManager.init(file);
        levelSelectParent.gameObject.SetActive(false);
    }
}
