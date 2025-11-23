using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public List<UpgradeSO> upgrades;
    private static readonly string upgradeFolderName = "upgrades";
    private static readonly string upgradeFileName = "upgradeData";

    void Start()
    {
        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Load()
    {
        var saveData = FileUtils.GetSaveFileData(upgradeFolderName, upgradeFileName);
        if (string.IsNullOrEmpty(saveData))
        {
            return;
        }

        upgrades = JsonConvert.DeserializeObject<List<UpgradeSO>>(saveData);

        if(upgrades == null || upgrades.Count == 0)
        {
            Debug.LogError("Failed to load upgrades from save data.");
        }
    }

    private void Save()
    {
        var folderPath = Path.Combine(Application.persistentDataPath, upgradeFolderName);

        if (upgrades == null || upgrades.Count == 0)
        {
            Debug.LogWarning("No upgrades loaded. Cannot save.");
            return;
        }
        FileUtils.TrySaveFile(folderPath, upgradeFileName, upgrades);
    }

    public int GetUpgradeCost(UpgradeStats statAffected)
    {
        var upgrade = upgrades.Find(u => u.statAffected == statAffected);
        if (upgrade == null)
        {
            Debug.LogError($"Upgrade for stat {statAffected} not found.");
            return int.MaxValue;
        }
        if (upgrade.currentTierIndex >= upgrade.tiers.Count() - 1)
        {
            Debug.LogWarning($"Upgrade for stat {statAffected} is already at max tier.");
            return int.MaxValue;
        }
        var nextUpgradeTier = upgrade.tiers[upgrade.currentTierIndex + 1];
        return nextUpgradeTier.cost;
    }

    public void PurchaseUpgrade(UpgradeStats statAffected)
    {
        var upgrade = upgrades.Find(u => u.statAffected == statAffected);
        if (upgrade == null)
        {
            Debug.LogError($"Upgrade for stat {statAffected} not found.");
            return;
        }
        if (upgrade.currentTierIndex >= upgrade.tiers.Count() - 1)
        {
            Debug.LogWarning($"Upgrade for stat {statAffected} is already at max tier.");
            return;
        }

        var nextUpgradeTier = upgrade.tiers[upgrade.currentTierIndex + 1];

        if (GameManager.Instance.gameState.Gold < nextUpgradeTier.cost)
        {
            Debug.LogWarning($"Cannot purchase upgrade, insufficient funds.");
            return;
        }

        GameManager.Instance.gameState.Gold -= nextUpgradeTier.cost;
        upgrade.currentTierIndex++;
    }


    public float GetUpgradeValue(UpgradeStats statAffected, float baseValue)
    {
        var upgrade = upgrades.Find(u => u.statAffected == statAffected);
        var currentTier = upgrade.tiers[upgrade.currentTierIndex];
        if (upgrade.upgradeType == UpgradeType.Additive)
        {
            return baseValue + currentTier.value;
        }
        return baseValue * currentTier.value;
    }
}
