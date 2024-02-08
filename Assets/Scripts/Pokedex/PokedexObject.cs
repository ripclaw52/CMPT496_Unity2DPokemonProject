using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexObject
{
    public int ID { get; set; }
    public string Name { get; set; }
    public PokemonBase Base { get; private set; }
    public EncounterStatus Status { get; set; }

    public PokedexObject(PokemonBase pPokemon)
    {
        Base = pPokemon;
        ID = pPokemon.DexId;
        Name = pPokemon.Name;
        Status = pPokemon.Status;
    }

    public PokedexObject(PokedexObjectSaveData saveData)
    {
        ID = saveData.id;
        Status = saveData.status;
        Name = saveData.name;
        Base = PokemonDB.GetObjectByName(saveData.name);
    }

    public PokedexObjectSaveData GetSaveData()
    {
        var saveData = new PokedexObjectSaveData()
        {
            id = ID,
            name = Name,
            status = Status,
        };
        return saveData;
    }

    public string FormatID()
    {
        return $"No. {Base.GetPokedexId()}";
    }
}

[System.Serializable]
public class PokedexObjectSaveData
{
    public int id;
    public string name;
    public EncounterStatus status;
}