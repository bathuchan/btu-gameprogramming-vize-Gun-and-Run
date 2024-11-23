using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gun))]
public class EnemyGunEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Gun enemyGun = (Gun)target;

        serializedObject.Update();

        DrawDefaultInspector();

        switch (enemyGun.fireMode)
        {
            case Gun.FireMode.Burst:
                
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Burst Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("burstCount"));
                break;

            case Gun.FireMode.Shotgun:
                
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Shotgun Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shotgunPellets"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shotgunSpreadAngle"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
