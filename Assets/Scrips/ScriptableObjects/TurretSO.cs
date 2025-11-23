using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "ScriptableObjects/Turret", order = 1)]
public class TurretSO : ScriptableObject
{
    public float Damage;
    public float AttackSpeed;
    public float Range;
    public float Velocity;
    public int Cost;
    public string Description;
    public TurretController prefab;
    public Sprite ShopImage;
}


public class TurretStats
{
    public TurretSO StatsSO;

    public float Damage => UpgradeManager.Instance.GetUpgradeValue(UpgradeStats.Damage, StatsSO.Damage);
    public float AttackInterval => UpgradeManager.Instance.GetUpgradeValue(UpgradeStats.AttackSpeed, 1 / StatsSO.AttackSpeed);
    public float Range => UpgradeManager.Instance.GetUpgradeValue(UpgradeStats.Range, StatsSO.Range);
    public float Velocity => UpgradeManager.Instance.GetUpgradeValue(UpgradeStats.Velocity, StatsSO.Velocity);

    public TurretStats(TurretSO so)
    {
        StatsSO = so;
    }
}