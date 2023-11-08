using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents the base of a Pokemon, containing all the necessary information to create a Pokemon.
/// </summary>
[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] new string name;

    [SerializeField] int dexId;
    [SerializeField] EncounterStatus status;
    [SerializeField] string species;
    [SerializeField] float height;
    [SerializeField] float weight;

    [TextAreaAttribute(minLines: 3, maxLines: 18)]
    [SerializeField] string description;

    [SerializeField] List<Sprite> frontSprite;
    [SerializeField] List<Sprite> backSprite;
    //[SerializeField] GameObject model; for a potential 3D model
    [SerializeField] List<Sprite> smallSprite;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    // Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    // Maximum Stats
    [SerializeField] int maximumHp;
    [SerializeField] int maximumAttack;
    [SerializeField] int maximumDefense;
    [SerializeField] int maximumSpAttack;
    [SerializeField] int maximumSpDefense;
    [SerializeField] int maximumSpeed;

    [SerializeField] List<EffortValues> evYield;

    // XP yield and Growth rate
    [SerializeField] int expYield;
    [SerializeField] GrowthRate growthRate;
    // Catch rate
    [SerializeField] int catchRate = 255;

    // Move lists
    [SerializeField] List<LearnableMove> learnableMoves;
    [SerializeField] List<MoveBase> learnableByItems;
    // Evolution list
    [SerializeField] List<Evolution> evolutions;

    public static int MaxNumOfMoves { get; set; } = 4;

    /// <summary>
    /// Calculates the experience points needed to reach a certain level based on the growth rate.
    /// </summary>
    /// <param name="level">The level to calculate the experience points for.</param>
    /// <returns>The experience points needed to reach the given level.</returns>
    public int GetExpForLevel(int level)
    {
        switch (growthRate)
        {
            case GrowthRate.Fast:
                return 4 * (level * level * level) / 5;
            case GrowthRate.MediumFast:
                return level * level * level;
            case GrowthRate.MediumSlow:
                return (6 * (level * level * level) / 5) - (15 * (level * level)) + (100 * level) - 140;
            case GrowthRate.Slow:
                return 5 * (level * level * level) / 4;
            case GrowthRate.Fluctuating:
                return GetFluctuating(level);
            case GrowthRate.Erratic:
                return GetErratic(level);
            default:
                break;
        }
        return -1;
    }

    /// <summary>
    /// Calculates a fluctuating value based on the given level.
    /// </summary>
    /// <param name="level">The level to use for the calculation.</param>
    /// <returns>The calculated fluctuating value.</returns>
    public int GetFluctuating(int level)
    {
        if (level < 15)
        {
            return Mathf.FloorToInt(Mathf.Pow(level, 3) * ((Mathf.Floor((level + 1) / 3) + 24) / 50));
        }
        else if ((level >= 15) && (level < 36))
        {
            return Mathf.FloorToInt(Mathf.Pow(level, 3) * ((level + 14) / 50));
        }
        else
        {
            return Mathf.FloorToInt(Mathf.Pow(level, 3) * ((Mathf.Floor(level / 2) + 32) / 50));
        }
    }

    /// <summary>
    /// Calculates the erratic value based on the given level.
    /// </summary>
    /// <param name="level">The level to calculate the erratic value for.</param>
    /// <returns>The calculated erratic value.</returns>
    public int GetErratic(int level)
    {
        if (level < 50)
        {
            return Mathf.FloorToInt(Mathf.Pow(level, 3) * (Mathf.Floor(100 - level) / 50));
        }
        else if ((level >= 50) && (level < 68))
        {
            return Mathf.FloorToInt(Mathf.Pow(level, 3) * (Mathf.Floor(150 - level) / 100));
        }
        else if ((level >= 68) && (level < 98))
        {
            return Mathf.FloorToInt(Mathf.Pow(level, 3) * (Mathf.Floor((1911 - (10 * level)) / 3) / 500));
        }
        else
        {
            return Mathf.FloorToInt(Mathf.Pow(level, 3) * (Mathf.Floor(160 - level) / 100));
        }
    }

    public string GetPokedexId() { return DexId.ToString("D4"); }
    public string GetSpecies() { return $"{Species} Pokemon"; }
    public string GetHeight() { return $"{MathF.Round(Height, 1)} m"; }
    public string GetWeight() { return $"{MathF.Round(Weight, 1)} kg"; }

    public string Name => name;
    public int DexId => dexId;
    public EncounterStatus Status { get; set; }
    public string Species => species;
    public float Height => height;
    public float Weight => weight;
    public string Description => description;
    
    public List<Sprite> FrontSprite => frontSprite;
    public List<Sprite> BackSprite => backSprite;
    public List<Sprite> SmallSprite => smallSprite;
    public PokemonType Type1 => type1;
    public PokemonType Type2 => type2;
    public int MaxHP => maxHp;
    public int Attack => attack;
    public int Defense => defense;
    public int SpAttack => spAttack;
    public int SpDefense => spDefense;
    public int Speed => speed;

    public int MaximumHP => maximumHp;
    public int MaximumAttack => maximumAttack;
    public int MaximumDefense => maximumDefense;
    public int MaximumSpAttack => maximumSpAttack;
    public int MaximumSpDefense => maximumSpDefense;
    public int MaximumSpeed => maximumSpeed;

    public List<EffortValues> EVYield => evYield;

    public int ExpYield => expYield;
    public GrowthRate GrowthRate => growthRate;
    public int CatchRate => catchRate;
    public List<LearnableMove> LearnableMoves => learnableMoves;
    public List<MoveBase> LearnableByItems => learnableByItems;
    public List<Evolution> Evolutions => evolutions;
}

/// <summary>
/// Represents a move that can be learned by a Pokemon at a specific level.
/// </summary>
[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base => moveBase;
    public int Level => level;
}

/// <summary>
/// Represents the evolution of a Pokemon, including the Pokemon it evolves into, the required level, and the required item.
/// </summary>
[System.Serializable]
public class Evolution
{
    [SerializeField] PokemonBase evolvesInto;
    [SerializeField] int requiredLevel;
    [SerializeField] EvolutionItem requiredItem;
    [SerializeField] DayNightCycle dayTimeCycle;

    public PokemonBase EvolvesInto => evolvesInto;
    public int RequiredLevel => requiredLevel;
    public EvolutionItem RequiredItem => requiredItem;
    public DayNightCycle DayTimeCycle => dayTimeCycle;

    public bool GetDayTimeCycle()
    {
        if (dayTimeCycle == DayNightCycle.None)
            return true;

        if (dayTimeCycle == DayNightCycle.Day)
            return IsDay();

        if (dayTimeCycle == DayNightCycle.Night)
            return IsNight();

        return false;
    }

    bool IsDay()
    {
        // get current local time
        int hour = DateTime.Now.Hour;
        // Debug.Log($"day; {hour}");

        // check
        if ((6f <= hour) || (hour <= 18))
            return true;

        return false;
    }

    bool IsNight()
    {
        // get current local time
        int hour = DateTime.Now.Hour;
        // Debug.Log($"night; {hour}");

        // check
        if (hour <= 6)
            return true;
        if (18 <= hour)
            return true;

        return false;
    }
}

[System.Serializable]
public class EffortValues
{
    [SerializeField] EVStats stat;
    [SerializeField] int value;

    public EVStats Stat => stat;
    public int Value => value;
}

public enum DayNightCycle
{
    None,
    Night,
    Day
}

/// <summary>
/// Enum containing all the different types of Pokemon.
/// </summary>
public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Dark,
    Steel,
    Fairy
}

/// <summary>
/// Enum representing the different growth rates of a Pokémon.
/// </summary>
public enum GrowthRate
{
    Erratic, Fast, MediumFast, MediumSlow, Slow, Fluctuating
}

public enum EncounterStatus
{
    None,
    Seen,
    Own
}

public enum EVStats
{
    HP,
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed
}

/// <summary>
/// Enum representing the different stats of a Pokemon.
/// </summary>
public enum Stat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed,

    // These 2 are not actual stats, they're used to boose the moveAccuracy
    Accuracy,
    Evasion
}

/// <summary>
/// This class provides a type chart for different types of data.
/// </summary>
public class TypeChart
{
    /// <summary>
    /// This is a chart of type effectiveness for the Pokemon game.
    /// </summary>
    static float[][] chart =
    {
        //    0 = no damage
        // 0.5f = not very effective
        //   1f = normal damage
        //   2f = super effective
        // 
        //Has to be same order as PokemonType class
        //                       Nor   Fir   Wat   Ele   Gra   Ice   Fig   Poi   Gro   Fly   Psy   Bug   Roc   Gho   Dra   Dar  Ste    Fai
        /*Normal*/  new float[] {1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 0,    1f,   1f,   0.5f, 1f},
        /*Fire*/    new float[] {1f,   0.5f, 0.5f, 1f,   2f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 1f,   2f,   1f},
        /*Water*/   new float[] {1f,   2f,   0.5f, 1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   1f,   1f},
        /*Electric*/new float[] {1f,   1f,   2f,   0.5f, 0.5f, 1f,   1f,   1f,   0f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f},
        /*Grass*/   new float[] {1f,   0.5f, 2f,   1f,   0.5f, 1f,   1f,   0.5f, 2f,   0.5f, 1f,   0.5f, 2f,   1f,   0.5f, 1f,   0.5f, 1f},
        /*Ice*/     new float[] {1f,   0.5f, 0.5f, 1f,   2f,   0.5f, 1f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f},
        /*Fighting*/new float[] {2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f, 0.5f, 0.5f, 2f,   0f,   1f,   2f,   2f,   0.5f},
        /*Poison*/  new float[] {1f,   1f,   1f,   1f,   2f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   0f,   2f},
        /*Ground*/  new float[] {1f,   2f,   1f,   2f,   0.5f, 1f,   1f,   2f,   1f,   0f,   1f,   0.5f, 2f,   1f,   1f,   1f,   2f,   1f},
        /*Flying*/  new float[] {1f,   1f,   1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 1f},
        /*Psychic*/ new float[] {1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   1f,   1f,   0.5f, 1f,   1f,   1f,   1f,   0f,   0.5f, 1f},
        /*Bug*/     new float[] {1f,   0.5f, 1f,   1f,   2f,   1f,   0.5f, 0.5f, 1f,   0.5f, 2f,   1f,   1f,   0.5f, 1f,   2f,   0.5f, 0.5f},
        /*Rock*/    new float[] {1f,   2f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f},
        /*Ghost*/   new float[] {0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   2f,   1f,   0.5f, 1f,   1f},
        /*Dragon*/  new float[] {1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 0f},
        /*Dark*/    new float[] {1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f},
        /*Steel*/   new float[] {1f,   0.5f, 0.5f, 0.5f, 1f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 2f},
        /*Fairy*/   new float[] {1f,   0.5f, 1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   0.5f, 1f},
    };

    /// <summary>
    /// Calculates the effectiveness of an attack based on the attack and defense types of the Pokemon.
    /// </summary>
    /// <param name="attackType">The type of the attacking Pokemon.</param>
    /// <param name="defenseType">The type of the defending Pokemon.</param>
    /// <returns>
    /// A float representing the effectiveness of the attack.
    /// </returns>
    static public float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        if (attackType == PokemonType.None || defenseType == PokemonType.None)
            return 1;

        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }
}