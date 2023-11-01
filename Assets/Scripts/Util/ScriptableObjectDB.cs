using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObjectDB is a generic MonoBehaviour class that provides a database of ScriptableObjects of type T.
/// </summary>
public class ScriptableObjectDB<T> : MonoBehaviour where T : ScriptableObject
{
    /// <summary>
    /// Represents a static dictionary of objects with string keys and generic type values.
    /// Made public for ease of access.
    /// </summary>
    public static Dictionary<string, T> objects;

    /// <summary>
    /// Initializes the objects dictionary by loading all objects of type T from the Resources folder. 
    /// If two objects have the same name, an error is logged.
    /// </summary>
    public static void Init()
    {
        objects = new Dictionary<string, T>();

        var objectArray = Resources.LoadAll<T>("");
        foreach (var obj in objectArray)
        {
            if (objects.ContainsKey(obj.name))
            {
                Debug.LogError($"There are two objects with the name {obj.name}");
                continue;
            }

            objects[obj.name] = obj;
        }

        //Debug.Log($"DICT of {typeof(T)} with length: {objects.Count}");
    }

    /// <summary>
    /// Retrieves an object from the database by its name.
    /// </summary>
    /// <param name="name">The name of the object to retrieve.</param>
    /// <returns>The object with the specified name, or null if not found.</returns>
    public static T GetObjectByName(string name)
    {
        if (!objects.ContainsKey(name))
        {
            Debug.LogError($"Object with name {name} not found in the database");
            return null;
        }

        return objects[name];
    }
}
