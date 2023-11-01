using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nature", menuName = "Pokemon/Create new nature")]
public class NatureBase : ScriptableObject
{
    [SerializeField] new string name;

    [HeaderAttribute("1 is neutral, 1.1 is positive, 0.9 is negative")]
    [SerializeField] float attack = 1f;
    [SerializeField] float defense = 1f;
    [SerializeField] float spAttack = 1f;
    [SerializeField] float spDefense = 1f;
    [SerializeField] float speed = 1f;

    public string Name => name;
    public float Attack => attack;
    public float Defense => defense;
    public float SpAttack => spAttack;
    public float SpDefense => spDefense;
    public float Speed => speed;
}
