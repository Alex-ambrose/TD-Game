using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button Playbutton;
    public Button leveleditbutton;

    // Start is called before the first frame update
    void Start()
    {
        Playbutton.onClick.AddListener(Play);
        leveleditbutton.onClick.AddListener(Edit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Play()
    {
        SceneManager.LoadScene("Play");
    }
    void Edit()
    {
        SceneManager.LoadScene("Level edit");
    }
}
