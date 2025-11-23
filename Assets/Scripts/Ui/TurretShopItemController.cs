using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretShopItemController : MonoBehaviour
{
    public Image Image;
    public Button Button;
    public TMP_Text ButtonText;

    private TurretSO turret;

    public void Setup(TurretSO t)
    {
        turret = t;
        Image.sprite = t.ShopImage;
        Button.onClick.AddListener(()=>TurretSpawnManager.Instance.SetCurrentTurret(t));
        Refresh();
    }

    public void Refresh()
    {
        ButtonText.text = $"{turret.Cost}";
        Button.interactable = GameManager.Instance.gameState.Gold >= turret.Cost;
    }
 }
