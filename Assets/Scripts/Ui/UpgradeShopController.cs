using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShopController : MonoBehaviour
{
    public UpgradeItem upgradeButtonPrefab;
    public Transform upgradeButtonContainer;
    private List<UpgradeItem> upgradeItemInstances;

    void Start()
    {
        PopulateButtons();
    }

    private void OnEnable()
    {
        Refresh();
    }

    void PopulateButtons()
    {
        upgradeItemInstances = new List<UpgradeItem>();
        foreach (var upgrade in UpgradeManager.Instance.upgrades)
        {
            var upgradeItemInstance = Instantiate(upgradeButtonPrefab, upgradeButtonContainer);
            upgradeItemInstance.Setup(upgrade);
            upgradeItemInstances.Add(upgradeItemInstance);
        }
    }

    void Refresh()
    {
        foreach(var upgradeItem in upgradeItemInstances)
        {
            upgradeItem.RefreshState();
        }
    }
}
