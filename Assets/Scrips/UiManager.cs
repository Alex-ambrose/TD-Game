using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    public GameManager gameManager;
    public ButtonItem levelselectbuttonprefab;
    public Transform levelselectParent;
    // Start is called before the first frame update
    void Start()
    {
       var Files = Directory.EnumerateFiles(Application.persistentDataPath);
        foreach (var file in Files)
        {
            var button = Instantiate(levelselectbuttonprefab);
            button.button.onClick.AddListener(() => loadlevel(file));
            button.text.text = Path.GetFileName(file);
            button.transform.parent = levelselectParent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void loadlevel(string file)
    {
        gameManager.init(file);
        levelselectParent.gameObject.SetActive(false);
    }
}
