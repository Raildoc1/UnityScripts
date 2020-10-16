using Office.Interaction;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HaveItemCondition))]
public class HaveItemConditionEditor : ConditionEditor {
    protected override void AllConditionsAssetGUI() {

        HaveItemCondition condition = (HaveItemCondition)target;

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUI.indentLevel++;

        // Display the description of the Condition.
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField(condition.description);
        condition.uniqueName = EditorGUILayout.TextField(GUIContent.none, condition.uniqueName);
        condition.amount = EditorGUILayout.IntField(GUIContent.none, condition.amount);
        EditorGUILayout.EndVertical();

        // Display a button showing a '-' that if clicked removes this Condition from the AllConditions asset.
        if (GUILayout.Button("-", GUILayout.Width(conditionButtonWidth)))
            AllConditionsEditor.RemoveCondition(condition);

        EditorGUI.indentLevel--;
        EditorGUILayout.EndHorizontal();
    }

    public static Condition CreateItemCondition(string description, string uniqueName, int amount) {
        // Create a new instance of the Condition.
        HaveItemCondition newCondition = CreateInstance<HaveItemCondition>();

        // Set the description and the hash based on it.
        newCondition.description = description;
        newCondition.uniqueName = uniqueName;
        newCondition.amount = amount;
        SetHash(newCondition);
        return newCondition;
    }
}
