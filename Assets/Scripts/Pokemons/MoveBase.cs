using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to create a new move for a Pokemon.
/// </summary>
[CreateAssetMenu(fileName = "Move", menuName = "Pokemon/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] new string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PokemonType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] bool alwaysHits;
    [SerializeField] int pp;
    [SerializeField] int priority;

    [SerializeField] MoveCategory category;
    [SerializeField] MoveEffects effects;
    [SerializeField] List<SecondaryEffects> secondaries;
    [SerializeField] MoveTarget target;

    public string Name => name;
    public string Description => description;
    public PokemonType Type => type;
    public int Power => power;
    public int Accuracy => accuracy;
    public bool AlwaysHits => alwaysHits;
    public int PP => pp;
    public int Priority => priority;
    public MoveCategory Category => category;
    public MoveEffects Effects => effects;
    public List<SecondaryEffects> Secondaries => secondaries;
    public MoveTarget Target => target;
}

/// <summary>
/// This class is used to store the effects of a move.
/// </summary>
[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;
    [SerializeField] ConditionID status;
    [SerializeField] ConditionID volatileStatus;

    public List<StatBoost> Boosts => boosts;
    public ConditionID Status => status;
    public ConditionID VolatileStatus => volatileStatus;
}

/// <summary>
/// Represents the SecondaryEffects class which inherits from the MoveEffects class.
/// </summary>
[System.Serializable]
public class SecondaryEffects : MoveEffects
{
    [SerializeField] int chance;
    [SerializeField] MoveTarget target;

    public int Chance => chance;
    public MoveTarget Target => target;
}

/// <summary>
/// This class is used to store information about a stat boost.
/// </summary>
[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}

/// <summary>
/// Enum representing the different categories of moves in a Pokemon game.
/// </summary>
public enum MoveCategory
{
    Physical, Special, Status
}

/// <summary>
/// Enum to represent the target of a move.
/// </summary>
public enum MoveTarget
{
    Foe, Self
}