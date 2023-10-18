using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PokemonBase))]
public class PokemonBaseEditor : Editor
{
    SerializedProperty name;
    SerializedProperty description;
    SerializedProperty frontSprite;
    SerializedProperty backSprite;
    SerializedProperty smallSprite;
    SerializedProperty type1;
    SerializedProperty type2;

    // Base Stats
    SerializedProperty maxHp;
    SerializedProperty attack;
    SerializedProperty defense;
    SerializedProperty spAttack;
    SerializedProperty spDefense;
    SerializedProperty speed;

    // Maximum Stats
    SerializedProperty maximumHp;
    SerializedProperty maximumAttack;
    SerializedProperty maximumDefense;
    SerializedProperty maximumSpAttack;
    SerializedProperty maximumSpDefense;
    SerializedProperty maximumSpeed;

    // Battle info
    SerializedProperty expYield;
    SerializedProperty growthRate;
    SerializedProperty catchRate;

    // Move lists
    SerializedProperty learnableMoves;
    SerializedProperty learnableByItems;
    // Evolution list
    SerializedProperty evolutions;

    bool showBaseStats, showMaxStats, showLevelUpInfo = false;

    private void OnEnable()
    {
        name = serializedObject.FindProperty("name");
        description = serializedObject.FindProperty("description");
        frontSprite = serializedObject.FindProperty("frontSprite");
        backSprite = serializedObject.FindProperty("backSprite");
        smallSprite = serializedObject.FindProperty("smallSprite");
        type1 = serializedObject.FindProperty("type1");
        type2 = serializedObject.FindProperty("type2");

        // base stat
        maxHp = serializedObject.FindProperty("maxHp");
        attack = serializedObject.FindProperty("attack");
        defense = serializedObject.FindProperty("defense");
        spAttack = serializedObject.FindProperty("spAttack");
        spDefense = serializedObject.FindProperty("spDefense");
        speed = serializedObject.FindProperty("speed");

        // maximum stat
        maximumHp = serializedObject.FindProperty("maximumHp");
        maximumAttack = serializedObject.FindProperty("maximumAttack");
        maximumDefense = serializedObject.FindProperty("maximumDefense");
        maximumSpAttack = serializedObject.FindProperty("maximumSpAttack");
        maximumSpDefense = serializedObject.FindProperty("maximumSpDefense");
        maximumSpeed = serializedObject.FindProperty("maximumSpeed");

        // levelup info
        expYield = serializedObject.FindProperty("expYield");
        growthRate = serializedObject.FindProperty("growthRate");
        catchRate = serializedObject.FindProperty("catchRate");

        learnableMoves = serializedObject.FindProperty("learnableMoves");
        learnableByItems = serializedObject.FindProperty("learnableByItems");

        evolutions = serializedObject.FindProperty("evolutions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(name);
        EditorGUILayout.PropertyField(description);
        EditorGUILayout.PropertyField(frontSprite);
        EditorGUILayout.PropertyField(backSprite);
        EditorGUILayout.PropertyField(smallSprite);
        EditorGUILayout.PropertyField(type1);
        EditorGUILayout.PropertyField(type2);

        showBaseStats = EditorGUILayout.BeginFoldoutHeaderGroup(showBaseStats, "Base Stats");
        if (showBaseStats)
        {
            EditorGUILayout.PropertyField(maxHp);
            EditorGUILayout.PropertyField(attack);
            EditorGUILayout.PropertyField(defense);
            EditorGUILayout.PropertyField(spAttack);
            EditorGUILayout.PropertyField(spDefense);
            EditorGUILayout.PropertyField(speed);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        showMaxStats = EditorGUILayout.BeginFoldoutHeaderGroup(showMaxStats, "Max Stats");
        if (showMaxStats)
        {
            EditorGUILayout.PropertyField(maximumHp);
            EditorGUILayout.PropertyField(maximumAttack);
            EditorGUILayout.PropertyField(maximumDefense);
            EditorGUILayout.PropertyField(maximumSpAttack);
            EditorGUILayout.PropertyField(maximumSpDefense);
            EditorGUILayout.PropertyField(maximumSpeed);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        showLevelUpInfo = EditorGUILayout.BeginFoldoutHeaderGroup(showLevelUpInfo, "Level Up");
        if (showLevelUpInfo)
        {
            EditorGUILayout.PropertyField(expYield);
            EditorGUILayout.PropertyField(growthRate);
            EditorGUILayout.PropertyField(catchRate);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.PropertyField(learnableMoves);
        EditorGUILayout.PropertyField(learnableByItems);
        EditorGUILayout.PropertyField(evolutions);

        serializedObject.ApplyModifiedProperties();
    }
}
