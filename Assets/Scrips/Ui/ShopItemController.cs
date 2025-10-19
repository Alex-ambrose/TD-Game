using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{
    public Image Image;
    public Button Button;
    public TMP_Text ButtonText;

    public void Setup(Turret t)
    {
        Image.sprite = t.ShopImage;
        ButtonText.text = $"{t.Cost}";
        Button.onClick.AddListener(()=>TurretSpawnManager.Instance.SetCurrentTurret(t));
    }
 }
