using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatValue
{
    // base stat values
    private int hp = 0;
    private int attack = 0;
    private int defense = 0;
    private int spAttack = 0;
    private int spDefense = 0;
    private int speed = 0;

    // Max individual number for stat
    int maxNumber = 252;

    public StatValue()
    {
        HP = 0;
        Attack = 0;
        Defense = 0;
        SpAttack = 0;
        SpDefense = 0;
        Speed = 0;
    }

    public StatValue(StatValue statValue)
    {
        HP = statValue.HP;
        Attack = statValue.Attack;
        Defense = statValue.Defense;
        SpAttack = statValue.SpAttack;
        SpDefense = statValue.SpDefense;
        Speed = statValue.Speed;
    }

    // Expose properties
    public int HP { get => hp; set => hp = value; }
    public int Attack { get => attack; set => attack = value; }
    public int Defense { get => defense; set => defense = value; }
    public int SpAttack { get => spAttack; set => spAttack = value; }
    public int SpDefense { get => spDefense; set => spDefense = value; }
    public int Speed { get => speed; set => speed = value; }

    /// <summary>
    /// Setup IV fields from randomize between 0 and 31
    /// </summary>
    public void SetupIV()
    {
        //Debug.Log($"This runs from the statvalue class!");
        HP = Mathf.RoundToInt(Random.Range(0, 32));
        Attack = Mathf.RoundToInt(Random.Range(0, 32));
        Defense = Mathf.RoundToInt(Random.Range(0, 32));
        SpAttack = Mathf.RoundToInt(Random.Range(0, 32));
        SpDefense = Mathf.RoundToInt(Random.Range(0, 32));
        Speed = Mathf.RoundToInt(Random.Range(0, 32));
    }

    /// <summary>
    /// Increase the EV value for a given stat based on ev yield value.
    /// EV cannot increase beyond maxNumber value
    /// </summary>
    /// <param name="evYield">List of EffortValue class define within the PokemonBase class</param>
    public void IncreaseEVFromEVYield(List<EffortValues> evYield)
    {
        foreach (var ev in evYield)
        {
            switch (ev.Stat)
            {
                case EVStats.HP:
                    HP = (HP < maxNumber) ? HP += ev.Value : HP;
                    break;
                case EVStats.Attack:
                    Attack = (Attack < maxNumber) ? Attack += ev.Value : Attack;
                    break;
                case EVStats.Defense:
                    Defense = (Defense < maxNumber) ? Defense += ev.Value : Defense;
                    break;
                case EVStats.SpAttack:
                    SpAttack = (SpAttack < maxNumber) ? SpAttack += ev.Value : SpAttack;
                    break;
                case EVStats.SpDefense:
                    SpDefense = (SpDefense < maxNumber) ? SpDefense += ev.Value : SpDefense;
                    break;
                case EVStats.Speed:
                    Speed = (Speed < maxNumber) ? Speed += ev.Value : Speed;
                    break;
            }
        }
    }
}