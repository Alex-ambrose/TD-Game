using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public ButtonItem purchaseButton;

    private UpgradeSO upgrade;

    public void Setup(UpgradeSO upgrade)
    {
        this.upgrade = upgrade;
        titleText.text = upgrade.statAffected.ToString();

        var cost = UpgradeManager.Instance.GetUpgradeCost(upgrade.statAffected);
        purchaseButton.Setup(cost.ToString(), () => TryPurchase());
        RefreshState();
    }

    public void TryPurchase()
    {
        UpgradeManager.Instance.PurchaseUpgrade(upgrade.statAffected);
        RefreshState();
    }

    public void RefreshState()
    {
        var cost = UpgradeManager.Instance.GetUpgradeCost(upgrade.statAffected);
        purchaseButton.buttonText.text = cost.ToString();
        purchaseButton.button.interactable = GameManager.Instance.gameState.Gold >= cost;

        descriptionText.text = GetNextTierDescription();
    }

    private string GetNextTierDescription()
    {
        var managedUpgrade = UpgradeManager.Instance.upgrades.Find(u => u.statAffected == upgrade.statAffected);

        if(managedUpgrade.tiers.Length <= managedUpgrade.currentTierIndex + 1)
        {
            return "Maxxed";
        }
        var nextTier = managedUpgrade.tiers[managedUpgrade.currentTierIndex + 1];
        return $"Increases {upgrade.statAffected.ToString()} by {nextTier.value}";
    }
}
