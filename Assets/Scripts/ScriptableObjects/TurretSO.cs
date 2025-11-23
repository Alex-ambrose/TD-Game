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
