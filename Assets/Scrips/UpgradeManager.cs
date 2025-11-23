using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public List<Upgrade> upgrades;

    void Start()
    {
        
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

public enum UpgradeStats
{
    Damage,
    AttackSpeed,
    Range,
    Velocity
}

public enum UpgradeType
{
    Additive,
    Multiplicative
}

public class Upgrade
{
    public UpgradeStats statAffected;
    public UpgradeType upgradeType;
    public UpgradeTier[] tiers;
    public int currentTierIndex = 0;
}

public class UpgradeTier
{
    public int tier;
    public float value;
    public int cost;
}
