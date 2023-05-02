using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class provides a saving system for the MonoBehaviour.
/// </summary>
public class SavingSystem : MonoBehaviour
{
    public static SavingSystem i { get; private set; }

    /// <summary>
    /// Sets the value of the static variable 'i' to the current instance of the class. 
    /// </summary>
    private void Awake()
    {
        i = this;
    }

    Dictionary<string, object> gameState = new Dictionary<string, object>();

    /// <summary>
    /// Captures the state of a list of savable entities and stores them in the game state.
    /// </summary>
    public void CaptureEntityStates(List<SavableEntity> savableEntities)
    {
        foreach (SavableEntity savable in savableEntities)
        {
            gameState[savable.UniqueId] = savable.CaptureState();
        }
    }

    /// <summary>
    /// Restores the state of the given list of savable entities.
    /// </summary>
    /// <param name="savableEntities">The list of savable entities to restore.</param>
    public void RestoreEntityStates(List<SavableEntity> savableEntities)
    {
        foreach (SavableEntity savable in savableEntities)
        {
            string id = savable.UniqueId;
            if (gameState.ContainsKey(id))
                savable.RestoreState(gameState[id]);
        }
    }

    /// <summary>
    /// Captures the current game state and saves it to the specified file.
    /// </summary>
    /// <param name="saveFile">The file to save the game state to.</param>
    public void Save(string saveFile)
    {
        CaptureState(gameState);
        SaveFile(saveFile, gameState);
    }

    /// <summary>
    /// Loads a save file and restores the game state.
    /// </summary>
    /// <param name="saveFile">The save file to load.</param>
    public void Load(string saveFile)
    {
        gameState = LoadFile(saveFile);
        RestoreState(gameState);
    }

    /// <summary>
    /// Deletes the specified file from the application's save directory.
    /// </summary>
    /// <param name="saveFile">The name of the file to delete.</param>
    public void Delete(string saveFile)
    {
        File.Delete(GetPath(saveFile));
    }

    /// <summary>
    /// Captures the state of all SavableEntities in the scene and stores them in a Dictionary.
    /// </summary>
    private void CaptureState(Dictionary<string, object> state)
    {
        foreach (SavableEntity savable in FindObjectsOfType<SavableEntity>())
        {
            state[savable.UniqueId] = savable.CaptureState();
        }
    }

    /// <summary>
    /// Restores the state of all SavableEntities in the scene from the given state dictionary.
    /// </summary>
    /// <param name="state">The state dictionary to restore from.</param>
    private void RestoreState(Dictionary<string, object> state)
    {
        foreach (SavableEntity savable in FindObjectsOfType<SavableEntity>())
        {
            string id = savable.UniqueId;
            if (state.ContainsKey(id))
                savable.RestoreState(state[id]);
        }
    }

    /// <summary>
    /// Saves the given state to the specified file.
    /// </summary>
    /// <param name="saveFile">The file to save the state to.</param>
    /// <param name="state">The state to save.</param>
    void SaveFile(string saveFile, Dictionary<string, object> state)
    {
        string path = GetPath(saveFile);
        print($"saving to {path}");

        using (FileStream fs = File.Open(path, FileMode.Create))
        {
            // Serialize our object
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs, state);
        }
    }

    /// <summary>
    /// Loads a file from the specified path and deserializes it into a Dictionary of strings and objects.
    /// </summary>
    /// <param name="saveFile">The path of the file to load.</param>
    /// <returns>A Dictionary of strings and objects.</returns>
    Dictionary<string, object> LoadFile(string saveFile)
    {
        string path = GetPath(saveFile);
        if (!File.Exists(path))
            return new Dictionary<string, object>();

        using (FileStream fs = File.Open(path, FileMode.Open))
        {
            // Deserialize our object
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return (Dictionary<string, object>)binaryFormatter.Deserialize(fs);
        }
    }

    /// <summary>
    /// Gets the path of the specified save file.
    /// </summary>
    /// <param name="saveFile">The name of the save file.</param>
    /// <returns>The path of the specified save file.</returns>
    private string GetPath(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile);
    }
}
