using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData EnemyData { get; set; }
    public Action OnAttack;
    public Action OnAttacked;

    public void Attack()
    {
        // TODO: Pick a random hero from the battlefield and attack.
        OnAttack?.Invoke();
    }

    public void SetEnemyData(EnemyData data)
    {
        EnemyData = data;
    }

    public void SetEnemyHealth(int health)
    {
        EnemyData.Health = health;
    }

    public void SetEnemyAttackPower(int power)
    {
        EnemyData.AttackPower = power;
    }
}
