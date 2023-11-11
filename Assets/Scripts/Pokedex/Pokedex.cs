using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pokedex : MonoBehaviour, ISavable
{
    public List<PokedexObject> pokeDex;

    public static Pokedex i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    public void SetupPokedex()
    {
        List<PokemonBase> pokedexBase = new List<PokemonBase>();
        foreach (var pokemon in PokemonDB.objects.Values)
        {
            pokedexBase.Add(pokemon);
        }
        PokeDex = new List<PokedexObject>();

        foreach (var pokemon in pokedexBase)
        {
            PokeDex.Add(new PokedexObject(pokemon));
        }
        PokeDex.Sort((p, q) => p.ID.CompareTo(q.ID));
    }

    public EncounterStatus FindEncounterStatus(PokemonBase pokemon)
    {
        return PokeDex.Find((x) => x.Name == pokemon.Name).Status;
    }

    public void ChangePokemonStatus(Pokemon pokemon, EncounterStatus status)
    {
        PokeDex.Find((x)=>x.Name == pokemon.Base.Name).Status = status;
    }

    public object CaptureState()
    {
        var saveData = new PokedexSaveData()
        {
            pokedex = PokeDex.Select(p => p.GetSaveData()).ToList(),
        };

        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = (PokedexSaveData)state;

        PokeDex = saveData.pokedex.Select(p => new PokedexObject(p)).ToList();
    }

    public List<PokedexObject> PokeDex { get => pokeDex; set => pokeDex = value; }
}

[System.Serializable]
public class PokedexSaveData
{
    public List<PokedexObjectSaveData> pokedex;
}