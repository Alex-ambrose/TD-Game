using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudController : Singleton<HudController>
{
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public ButtonItem nextWaveButton;

    void Start()
    {
        scoreText.text = "Gold : 0";
        nextWaveButton.buttonText.text = "Next Wave";
        nextWaveButton.button.onClick.AddListener(() => {
            SpawnNextWave();
        });

    }

    void SpawnNextWave()
    {
        GameManager.Instance.SpawnNextWave();
    }

    public void OnWaveStateChanged(WaveStatus waveStatus)
    {
        nextWaveButton.button.interactable = waveStatus == WaveStatus.Completed;
    }

    public void UpdateLives(int remaningLives)
    {
        livesText.text = $"Lives: {remaningLives}";
    }
    public void UpdateGold(int Gold)
    {
        scoreText.text = $"Gold: {Gold}";
    }

    void Update()
    {
        
    }
}
