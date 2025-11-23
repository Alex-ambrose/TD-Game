using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    public List<TurretSO> turretScriptableObjects;
    public Transform shopItemParent;
    public ShopItemController shopItemPrefab;

    void Start()
    {
        foreach (TurretSO t in turretScriptableObjects)
        {
            var shopItem = Instantiate(shopItemPrefab,shopItemParent);
            shopItem.Setup(t);      
        }
    }
}
