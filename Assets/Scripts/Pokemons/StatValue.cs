using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatValue
{
    // base stat values
    int hp = 0;
    int attack = 0;
    int defense = 0;
    int spAttack = 0;
    int spDefense = 0;
    int speed = 0;

    // Max individual number for stat
    int maxNumber = 252;

    public StatValue()
    {
        hp = 0;
        attack = 0;
        defense = 0;
        spAttack = 0;
        spDefense = 0;
        speed = 0;
    }

    public StatValue(StatValue statValue)
    {
        hp = statValue.hp;
        attack = statValue.attack;
        defense = statValue.defense;
        spAttack = statValue.spAttack;
        spDefense = statValue.spDefense;
        speed = statValue.speed;
    }

    // Expose properties
    public int HP => hp;
    public int Attack => attack;
    public int Defense => defense;
    public int SpAttack => spAttack;
    public int SpDefense => spDefense;
    public int Speed => speed;

    /// <summary>
    /// Setup IV fields from randomize between 0 and 31
    /// </summary>
    public void SetupIV()
    {
        //Debug.Log($"This runs from the statvalue class!");
        hp = Mathf.RoundToInt(Random.Range(0, 32));
        attack = Mathf.RoundToInt(Random.Range(0, 32));
        defense = Mathf.RoundToInt(Random.Range(0, 32));
        spAttack = Mathf.RoundToInt(Random.Range(0, 32));
        spDefense = Mathf.RoundToInt(Random.Range(0, 32));
        speed = Mathf.RoundToInt(Random.Range(0, 32));
    }

    public void ModHP(int value)
    {
        hp = (hp < maxNumber) ? hp += value : hp;
    }

    public void ModStat(Stat stat, int value)
    {
        switch (stat.ToString())
        {
            case "Attack":
                attack = (attack < maxNumber) ? attack += value : attack;
                break;
            case "Defense":
                defense = (defense < maxNumber) ? defense += value : defense;
                break;
            case "SpAttack":
                spAttack = (spAttack < maxNumber) ? spAttack += value : spAttack;
                break;
            case "SpDefense":
                spDefense = (spDefense < maxNumber) ? spDefense += value : spDefense;
                break;
            case "Speed":
                speed = (speed < maxNumber) ? speed += value : speed;
                break;
        }
    }
}