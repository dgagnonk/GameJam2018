using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//Custom inspector for personSpawner
[CustomEditor(typeof(PersonSpawner))]
public class PersonSpawnerEditor : Editor
{
    bool setupFoldout;
    public override void OnInspectorGUI()
    {
        PersonSpawner personSpawnerTarget = (PersonSpawner) target;

        personSpawnerTarget.randomSpawn = EditorGUILayout.Toggle("Random Spawn", personSpawnerTarget.randomSpawn);

        if(personSpawnerTarget.randomSpawn)
        {
            personSpawnerTarget.numPeople = EditorGUILayout.IntField("Number of NPCs", personSpawnerTarget.numPeople);
        }
        else
        {
            personSpawnerTarget.columns = EditorGUILayout.IntField("NPC Columns", personSpawnerTarget.columns);
            personSpawnerTarget.rows = EditorGUILayout.IntField("NPC Rows", personSpawnerTarget.rows);
        }


        setupFoldout = EditorGUILayout.Foldout(setupFoldout, "Additional Settings", true);
        if(setupFoldout)
        {
            personSpawnerTarget.boundaries = (SpawnBoundaries)EditorGUILayout.ObjectField("Boundaries Script", personSpawnerTarget.boundaries, typeof(SpawnBoundaries), true);

            personSpawnerTarget.peopleParent = (Transform)EditorGUILayout.ObjectField("People Parent", personSpawnerTarget.peopleParent, typeof(Transform), true);

            personSpawnerTarget.personPrefab = (GameObject)EditorGUILayout.ObjectField("Person Prefab", personSpawnerTarget.personPrefab, typeof(GameObject), true);
        }

    }
}
