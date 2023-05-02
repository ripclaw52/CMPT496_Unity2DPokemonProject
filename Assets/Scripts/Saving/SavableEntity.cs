using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This class provides a MonoBehaviour that can be saved and loaded.
/// </summary>
[ExecuteAlways]
public class SavableEntity : MonoBehaviour
{
    [SerializeField] string uniqueId = "";
    static Dictionary<string, SavableEntity> globalLookup = new Dictionary<string, SavableEntity>();

    public string UniqueId => uniqueId;

    /// <summary>
    /// Captures the state of the gameobject on which the savableEntity is attached.
    /// </summary>
    /// <returns>A dictionary containing the state of all components implementing the ISavable interface.</returns>
    public object CaptureState()
    {
        Dictionary<string, object> state = new Dictionary<string, object>();
        foreach (ISavable savable in GetComponents<ISavable>())
        {
            state[savable.GetType().ToString()] = savable.CaptureState();
        }
        return state;
    }

    /// <summary>
    /// Restores the state of the gameobject on which the savableEntity is attached.
    /// </summary>
    public void RestoreState(object state)
    {
        Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
        foreach (ISavable savable in GetComponents<ISavable>())
        {
            string id = savable.GetType().ToString();

            if (stateDict.ContainsKey(id))
                savable.RestoreState(stateDict[id]);
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Update method used for generating UUID of the SavableEntity
    /// Updates the unique ID of the game object, if it is not already set or is not unique. 
    /// Also adds the game object to the global lookup.
    /// </summary>
    private void Update()
    {
        // don't execute in playmode
        if (Application.IsPlaying(gameObject)) return;

        // don't generate Id for prefabs (prefab scene will have path as null)
        if (String.IsNullOrEmpty(gameObject.scene.path)) return;

        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty property = serializedObject.FindProperty("uniqueId");

        if (String.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
        {
            property.stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }

        globalLookup[property.stringValue] = this;
    }
#endif

    /// <summary>
    /// Checks if the given candidate is unique.
    /// </summary>
    /// <param name="candidate">The candidate to check.</param>
    /// <returns>True if the candidate is unique, false otherwise.</returns>
    private bool IsUnique(string candidate)
    {
        if (!globalLookup.ContainsKey(candidate)) return true;

        if (globalLookup[candidate] == this) return true;

        // Handle scene unloading cases
        if (globalLookup[candidate] == null)
        {
            globalLookup.Remove(candidate);
            return true;
        }

        // Handle edge cases like designer manually changing the UUID
        if (globalLookup[candidate].UniqueId != candidate)
        {
            globalLookup.Remove(candidate);
            return true;
        }

        return false;
    }
}