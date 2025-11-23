using System.Collections.Generic;
using UnityEngine;

public class TurretShopController : MonoBehaviour
{
    public List<TurretSO> turretScriptableObjects;
    public Transform shopItemParent;
    public TurretShopItemController shopItemPrefab;

    private List<TurretShopItemController> turretShopItemControllers;

    void Start()
    {
        turretShopItemControllers = new List<TurretShopItemController>();
        foreach (TurretSO t in turretScriptableObjects)
        {
            var turretShopItem = Instantiate(shopItemPrefab,shopItemParent);
            turretShopItem.Setup(t);
            turretShopItemControllers.Add(turretShopItem);
        }
    }

    private void OnEnable()
    {
        Refresh();
    }

    // TODO: call this method when player's money changes, or set up an event
    void Refresh()
    {
        // Since this is also called on OnEnable, we need to check for null
        if (turretShopItemControllers == null)
        {
            return;
        }
        foreach (var shopItem in turretShopItemControllers)
        {
            shopItem.Refresh();
        }
    }
}
