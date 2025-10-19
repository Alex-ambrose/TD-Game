using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    public List<Turret> Turrets;
    public Transform shop;
    public ShopItemController shopItemPrefab;
    void Start()
    {
        foreach (Turret t in Turrets)
        {
            var shopItem = Instantiate(shopItemPrefab,shop);
            shopItem.Setup(t);      
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
