using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyGun))]
public class EnemyGunEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyGun enemyGun = (EnemyGun)target;

        serializedObject.Update();

        DrawDefaultInspector();

        switch (enemyGun.fireMode)
        {
            case EnemyGun.FireMode.Burst:
                
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Burst Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("burstCount"));
                break;

            case EnemyGun.FireMode.Shotgun:
                
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Shotgun Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shotgunPellets"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shotgunSpreadAngle"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
