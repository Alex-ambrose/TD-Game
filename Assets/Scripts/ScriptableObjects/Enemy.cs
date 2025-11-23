using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class Enemy : ScriptableObject
{
    public EnemyStats stats;
    public int availableAtWave;
    public int cost;
    public Color Color;
}
[Serializable]
public class EnemyStats
{
    public float maxHealth;
    public float currentHealth;
    public float speed;
    public float healthPerSecond;
}