using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade", order = 1)]
public class UpgradeSO : ScriptableObject
{
    public UpgradeStats statAffected;
    public UpgradeType upgradeType;
    public UpgradeTier[] tiers;
    // TODO: move this to its own class for saving
    public int currentTierIndex = 0;
}

[Serializable]
public class UpgradeTier
{
    public float value;
    public int cost;
}

