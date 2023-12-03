using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class represents a Pokemon object which can be serialized with properties such as name, type, and level.
/// </summary>
[System.Serializable]
public class Pokemon
{
    bool hasValue;
    [HideInInspector] public bool HasValue
    {
        get
        {
            if (_base != null)
            {
                hasValue = true;
            }
            else
            {
                hasValue = false;
            }
            return hasValue;
        }

        set
        {
            hasValue = value;
        }
    }

    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    public NatureBase? nature;
    public StatValue iv = new StatValue();
    public StatValue ev = new StatValue();

    /// <summary>
    /// Constructor for the Pokemon class, initializing the base and level of the Pokemon.
    /// </summary>
    /// <param name="pBase">The base of the Pokemon.</param>
    /// <param name="pLevel">The level of the Pokemon.</param>
    /// <returns>A new Pokemon instance.</returns>
    public Pokemon(PokemonBase pBase, int pLevel)
    {
        HasValue = true;

        _base = pBase;
        level = pLevel;

        Init();
    }

    /// <summary>
    /// None Pokemon constructor to hold nullable Pokemon objects.
    /// Should be fired on list creation
    /// </summary>
    public Pokemon()
    {
        HasValue = false;

        _base = null;
        
        level = 0;
        HP = 0;
        Exp = 0;

        Nature = null;
        IV = new StatValue();
        EV = new StatValue();
        Status = null;

        Moves = new List<Move>(4);
    }

    public PokemonBase Base { get { return _base; } }
    public int Level { get { return level; } }
    public int Exp { get; set; }
    public int HP { get; set; }

    public NatureBase Nature { get => nature; set => nature = value; }
    public StatValue IV { get => iv; set => iv = value; }
    public StatValue EV { get => ev; set => ev = value; }

    public List<Move> Moves { get; set; }
    public Move CurrentMove { get; set; }

    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public Condition Status { get; private set; }
    public int StatusTime { get; set; }
    public Condition VolatileStatus { get; private set; }
    public int VolatileStatusTime { get; set; }
    public Queue<string> StatusChanges { get; private set; }

    public int MaximumHp { get; private set; }
    public Dictionary<Stat, int> MaximumStats { get; private set; }

    public event System.Action OnStatusChanged;
    public event System.Action OnHPChanged;

    /// <summary>
    /// Initializes a Pokemon instance by generating its moves, calculating its stats, setting its HP to its max HP, creating a queue for status changes, resetting stat boosts, and setting its status and volatile status to null.
    /// </summary>
    public void Init()
    {
        HasValue = true;

        // Generate Moves
        Moves = new List<Move>();

        // Start at highest level move and work to lowest from learnable moves list
        var temp = Base.LearnableMoves;
        for (int i = temp.Count-1; i >= 0; i--)
        {
            if (temp[i].Level <= Level)
            {
                // if index in list is greater than 3 (Level has more than 4 moves), enable randomness
                if (i > 3)
                {
                    // Add randomness [0-2] / [3-9] = (3/10)
                    if (Random.Range(0, 10) > 2)
                        Moves.Add(new Move(temp[i].Base));
                }
                else
                {
                    Moves.Add(new Move(temp[i].Base));
                }
            }

            if (Moves.Count >= PokemonBase.MaxNumOfMoves)
                break;
        }

        /*
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= PokemonBase.MaxNumOfMoves)
                break;
        }
        */

        Exp = Base.GetExpForLevel(Level);

        // Enable IV and EV before Stat Calculation
        //Debug.Log("This line exists!");
        if (Nature == null)
        {
            GetRandomNature();
        }
        //Debug.Log($"{nature.Name}");
        IV = new StatValue();
        EV = new StatValue();
        iv.SetupIV();

        CalculateStats();
        HP = MaxHP;

        StatusChanges = new Queue<string>();
        ResetStatBoost();
        Status = null;
        VolatileStatus = null;

        CalculateMaxStats();
    }

    /// <summary>
    /// Constructor for the Pokemon class, taking a PokemonSaveData object as a parameter.
    /// </summary>
    /// <returns>
    /// A new Pokemon object.
    /// </returns>
    public Pokemon(PokemonSaveData saveData)
    {
        // Get HasValue
        HasValue = saveData.hasValue;
        Debug.Log($"({HasValue})");

        // if true
        if (saveData.hasValue)
        {
            HasValue = true;
            _base = PokemonDB.GetObjectByName(saveData.name);
            HP = saveData.hp;
            level = saveData.level;
            Exp = saveData.exp;

            nature = NatureDB.GetObjectByName(saveData.nature);
            iv = new StatValue(saveData.iv);
            ev = new StatValue(saveData.ev);

            if (saveData.statusId != null)
                Status = ConditionsDB.Conditions[saveData.statusId.Value];
            else
                Status = null;

            Moves = saveData.moves.Select(s => new Move(s)).ToList();

            CalculateStats();
            StatusChanges = new Queue<string>();
            ResetStatBoost();
            VolatileStatus = null;

            CalculateMaxStats();
        }
        else
        {
            new Pokemon();
        }
    }

    /// <summary>
    /// Gets the save data for the Pokemon.
    /// </summary>
    /// <returns>The Pokemon's save data.</returns>
    public PokemonSaveData GetSaveData()
    {
        var saveData = new PokemonSaveData
        {
            hasValue = HasValue
        };

        if (hasValue)
        {
            saveData.name = Base.name;
            saveData.hp = HP;
            saveData.level = Level;
            saveData.exp = Exp;
            saveData.nature = Nature.name;
            saveData.iv = IV;
            saveData.ev = EV;
            saveData.statusId = Status?.Id;
            saveData.moves = Moves.Select(m => m.GetSaveData()).ToList();
        }
        else
        {
            saveData.name = "null";
            saveData.hp = 0;
            saveData.level = 0;
            saveData.exp = 0;
            saveData.name = "null";
            saveData.iv = new StatValue();
            saveData.ev = new StatValue();
            saveData.statusId = 0;
            saveData.moves = new List<MoveSaveData>(4);
        }

        return saveData;
    }

    /// <summary>
    /// Calculates the stats of a Pokemon based on its base stats and level.
    /// </summary>
    void CalculateStats()
    {
        //Debug.Log($"NATURE; {nature.Name}");
        //Debug.Log($"IV; {iv.Attack}");
        //Debug.Log($"EV; {ev.Attack}");

        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt(((((2 * Base.Attack + IV.Attack + (EV.Attack / 4)) * Level) / 100f) + 5) * Nature.Attack));
        Stats.Add(Stat.Defense, Mathf.FloorToInt(((((2 * Base.Defense + IV.Defense + (EV.Defense / 4)) * Level) / 100f) + 5) * Nature.Defense));
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt(((((2 * Base.SpAttack + IV.SpAttack + (EV.SpAttack / 4)) * Level) / 100f) + 5) * Nature.SpAttack));
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt(((((2 * Base.SpDefense + IV.SpDefense + (EV.SpDefense / 4)) * Level) / 100f) + 5) * Nature.SpDefense));
        Stats.Add(Stat.Speed, Mathf.FloorToInt(((((2 * Base.Speed + IV.Speed + (EV.Speed / 4)) * Level) / 100f) + 5) * Nature.Speed));

        int oldMaxHP = MaxHP;
        MaxHP = Mathf.FloorToInt((((2 * Base.MaxHP + IV.HP + (EV.HP/4)) * Level) / 100f) + 10 + Level);

        if (oldMaxHP != 0)
            HP += MaxHP - oldMaxHP;
    }

    void CalculateMaxStats()
    {
        MaximumStats = new Dictionary<Stat, int>();
        MaximumStats.Add(Stat.Attack, Base.MaximumAttack);
        MaximumStats.Add(Stat.Defense, Base.MaximumDefense);
        MaximumStats.Add(Stat.SpAttack, Base.MaximumSpAttack);
        MaximumStats.Add(Stat.SpDefense, Base.MaximumSpDefense);
        MaximumStats.Add(Stat.Speed, Base.MaximumSpeed);

        MaximumHp = Base.MaximumHP;
    }

    /// <summary>
    /// Resets the StatBoosts dictionary to its default values.
    /// </summary>
    void ResetStatBoost()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0},
            {Stat.Defense, 0},
            {Stat.SpAttack, 0},
            {Stat.SpDefense, 0},
            {Stat.Speed, 0},
            {Stat.Accuracy, 0},
            {Stat.Evasion, 0}
        };
    }

    int GetCurentStat(Stat stat) { return Stats[stat];}
    int GetMaximumStat(Stat stat) { return MaximumStats[stat]; }
    int GetCurrentHP() { return MaxHP; }
    int GetMaximumHP() { return MaximumHp; }

    public float GetHPNormalized() { return (float)GetCurrentHP() / GetMaximumHP(); }
    public float GetStatNormalized(Stat stat) { return (float)GetCurentStat(stat) / GetMaximumStat(stat); }

    /// <summary>
    /// Gets the stat value after applying any stat boosts.
    /// </summary>
    /// <param name="stat">The stat to get the value of.</param>
    /// <returns>The stat value after applying any stat boosts.</returns>
    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        // Apply stat boost
        int boost = StatBoosts[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        else
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);

        return statVal;
    }

    /// <summary>
    /// Applies stat boosts to the character and enqueues status changes.
    /// </summary>
    /// <param name="statBoosts">The list of stat boosts to apply.</param>
    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach (var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

            // Apply sfx here for stat raise or fall
            if (boost > 0)
            {
                AudioManager.i.PlaySfx(AudioId.StatRose);
                StatusChanges.Enqueue($"{Base.Name}'s {stat} rose!");
            }
            else
            {
                AudioManager.i.PlaySfx(AudioId.StatFall);
                StatusChanges.Enqueue($"{Base.Name}'s {stat} fell!");
            }

            Debug.Log($"{stat} has been boosted to {StatBoosts[stat]}");
        }
    }

    /// <summary>
    /// Checks if the experience is greater than the required experience for the next level and updates the level and stats accordingly.
    /// </summary>
    /// <returns>Returns true if the level was updated, false otherwise.</returns>
    public bool CheckForLevelUp()
    {
        if (Exp > Base.GetExpForLevel(level + 1))
        {
            ++level;
            CalculateStats();
            return true;
        }

        return false;
    }

    public int CheckExpToNextLevel()
    {
        return (level < (GlobalSettings.i.MaximumPokemonLevel + 1)) ? (Base.GetExpForLevel(Level + 1) - Exp) : 0;
    }

    /// <summary>
    /// Gets the LearnableMove at the current level.
    /// </summary>
    /// <returns>The LearnableMove at the current level.</returns>
    public LearnableMove GetLearnableMoveAtCurrLevel()
    {
        return Base.LearnableMoves.Where(x => x.Level == level).FirstOrDefault();
    }

    /// <summary>
    /// Adds a new move to the list of moves of the Pokemon.
    /// </summary>
    /// <param name="moveToLearn">The move to be added.</param>
    public void LearnMove(MoveBase moveToLearn)
    {
        if (Moves.Count > PokemonBase.MaxNumOfMoves)
            return;

        Moves.Add(new Move(moveToLearn));
    }

    /// <summary>
    /// Checks if the list of Moves contains a MoveBase object.
    /// </summary>
    /// <param name="moveToCheck">The MoveBase object to check for.</param>
    /// <returns>True if the list of Moves contains the MoveBase object, false otherwise.</returns>
    public bool HasMove(MoveBase moveToCheck)
    {
        return Moves.Count(m => m.Base == moveToCheck) > 0;
    }

    /// <summary>
    /// Checks for an evolution based on the current level.
    /// </summary>
    /// <returns>The evolution that is required for the current level.</returns>
    public Evolution CheckForEvolution()
    {
        return Base.Evolutions.FirstOrDefault(e => e.RequiredLevel <= level && e.GetDayTimeCycle());
    }

    /// <summary>
    /// Checks if the given item is a required item for an evolution.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>The evolution that requires the given item, or null if none.</returns>
    public Evolution CheckForEvolution(ItemBase item)
    {
        return Base.Evolutions.FirstOrDefault(e => e.RequiredItem == item);
    }

    /// <summary>
    /// Evolves the current object into the specified evolution and calculates the stats.
    /// </summary>
    public void Evolve(Evolution evolution)
    {
        _base = evolution.EvolvesInto;
        CalculateStats();
    }

    /// <summary>
    /// Resets the HP to the maximum HP and invokes the OnHPChanged event.
    /// </summary>
    public void Heal()
    {
        HP = MaxHP;
        OnHPChanged?.Invoke();
        CureStatus();
    }

    public int Attack => GetStat(Stat.Attack);
    public int Defense => GetStat(Stat.Defense);
    public int SpAttack => GetStat(Stat.SpAttack);
    public int SpDefense => GetStat(Stat.SpDefense);
    public int Speed => GetStat(Stat.Speed);
    public int MaxHP { get; private set; }

    /// <summary>
    /// Calculates the damage taken by a Pokemon when attacked by a Move.
    /// </summary>
    /// <param name="move">The Move used to attack the Pokemon.</param>
    /// <param name="attacker">The Pokemon attacking the Pokemon.</param>
    /// <returns>A DamageDetails object containing the damage taken.</returns>
    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        float critical = 1f;
        if (!(move.Base.CriticalHitBehaviour == CriticalHitBehaviour.Never))
        {
            if (move.Base.CriticalHitBehaviour == CriticalHitBehaviour.Always)
            {
                critical = 1.5f;
            }
            else
            {
                // how to specify moves like razor leaf which have crit chance of 12.5?
                int criticalChance = 0 + ((move.Base.CriticalHitBehaviour == CriticalHitBehaviour.High) ? 1 : 0);
                float[] chances = new float[] { (4.167f), (12.5f), (50f), 100f };
                if (Random.value * 100f <= chances[Mathf.Clamp(criticalChance, 0, 3)])
                    critical = 1.5f;
            }
        }

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false,
            DamageDealt = 0
        };

        float attack = (move.Base.Category == MoveCategory.Special) ? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.Category == MoveCategory.Special) ? SpDefense : Defense;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        DecreaseHP(damage);

        damageDetails.DamageDealt = damage;

        return damageDetails;
    }

    public void TakeRecoilDamage(int damage)
    {
        if (damage < 1)
            damage = 1;
        DecreaseHP(damage);
        StatusChanges.Enqueue($"{Base.Name} was damaged by recoil!");
    }

    /// <summary>
    /// Increases the HP of the object by the given amount, clamped between 0 and MaxHP. Invokes the OnHPChanged event.
    /// </summary>
    public void IncreaseHP(int amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, MaxHP);
        OnHPChanged?.Invoke();
    }

    /// <summary>
    /// Decreases the HP of the object by the specified damage amount, clamped between 0 and MaxHP. Invokes the OnHPChanged event.
    /// </summary>
    public void DecreaseHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHP);
        OnHPChanged?.Invoke();
    }

    /// <summary>
    /// Sets the status of the object based on the given condition ID. Invokes the OnStart event, adds a start message to the status changes queue, and invokes the OnStatusChanged event.
    /// </summary>
    public void SetStatus(ConditionID conditionId)
    {
        if (Status != null) return;

        Status = ConditionsDB.Conditions[conditionId];
        Status?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Name} {Status.StartMessage}");
        OnStatusChanged?.Invoke();
    }

    /// <summary>
    /// Sets the Status to null and invokes the OnStatusChanged event.
    /// </summary>
    public void CureStatus()
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }

    /// <summary>
    /// Sets the volatile status of the character to the specified condition.
    /// </summary>
    /// <param name="conditionId">The ID of the condition to set.</param>
    public void SetVolatileStatus(ConditionID conditionId)
    {
        if (VolatileStatus != null) return;

        VolatileStatus = ConditionsDB.Conditions[conditionId];
        VolatileStatus?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.Name} {VolatileStatus.StartMessage}");
    }

    /// <summary>
    /// Resets the VolatileStatus to null.
    /// </summary>
    public void CureVolatileStatus()
    {
        VolatileStatus = null;
    }

    public void GetRandomNature()
    {
        //Debug.Log("Nature runs!");
        int size = NatureDB.objects.Count;
        List<string> natures = new List<string>();
        foreach (var i in NatureDB.objects)
        {
            //Debug.Log($"nature; k:{i.Key} v:{i.Value}");
            natures.Add(i.Key);
        }

        int index = Mathf.RoundToInt(Random.Range(0, size));
        //Debug.Log($"{natures[index].name}");
        nature = NatureDB.GetObjectByName(natures[index]);
    }

    /// <summary>
    /// Gets a random move from the list of moves with PP greater than 0.
    /// </summary>
    /// <returns>A random move from the list of moves with PP greater than 0.</returns>
    public Move GetRandomMove()
    {
        var movesWithPP = Moves.Where(x => x.PP > 0).ToList();

        int r = Random.Range(0, movesWithPP.Count);
        return movesWithPP[r];
    }

    /// <summary>
    /// Checks if the move can be performed by checking the Status and VolatileStatus objects.
    /// </summary>
    /// <returns>True if the move can be performed, false otherwise.</returns>
    public bool OnBeforeMove()
    {
        bool canPerformMove = true;
        if (Status?.OnBeforeMove != null)
        {
            if (!Status.OnBeforeMove(this))
                canPerformMove = false;
        }

        if (VolatileStatus?.OnBeforeMove != null)
        {
            if (!VolatileStatus.OnBeforeMove(this))
                canPerformMove = false;
        }

        return canPerformMove;
    }

    /// <summary>
    /// Invokes the OnAfterTurn methods of the Status and VolatileStatus objects.
    /// </summary>
    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
        VolatileStatus?.OnAfterTurn?.Invoke(this);
    }

    /// <summary>
    /// Resets the volatile status and stat boosts after a battle is over.
    /// </summary>
    public void OnBattleOver()
    {
        VolatileStatus = null;
        ResetStatBoost();
    }
}

/// <summary>
/// This class contains the details of the damage done to an object.
/// </summary>
public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
    public int DamageDealt { get; set; }
}

/// <summary>
/// This class is used to store the data of a Pokemon for saving and loading.
/// </summary>
[System.Serializable]
public class PokemonSaveData
{
    public bool hasValue;

    public string name;
    public int hp;
    public int level;
    public int exp;

    public string nature;
    public StatValue iv;
    public StatValue ev;

    public ConditionID? statusId;
    public List<MoveSaveData> moves;
}
