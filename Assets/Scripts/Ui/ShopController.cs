using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ShopType
{
    Turret,
    Upgrade
}

public class ShopController : MonoBehaviour
{
    public GameObject turretShop;
    public GameObject upgradeShop;
    public ShopType currentShopType;
    public TabButton tabButtonPrefab;
    public Transform tabParent;

    // Start is called before the first frame update
    void Start()
    {
        SelectShop(ShopType.Turret);
        SetupTabs();
    }

    // Update is called once per frame
    void SetupTabs()
    {
        foreach (var value in Enum.GetValues(typeof(ShopType)).Cast<ShopType>())
        {
            var shopButton = Instantiate(tabButtonPrefab, tabParent);
            shopButton.Setup(value.ToString(), () => SelectShop(value));
        }
    }

    public void SelectShop(ShopType shopType)
    {
        currentShopType = shopType;
        turretShop.SetActive(currentShopType == ShopType.Turret);
        upgradeShop.SetActive(currentShopType == ShopType.Upgrade);
    }
}
