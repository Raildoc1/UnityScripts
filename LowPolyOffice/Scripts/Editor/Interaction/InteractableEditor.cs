// Script is taken from Unity Adventure Tutorial
// Some code was changed
// Changed or new lines marked as 'KG'

using UnityEngine;
using UnityEditor;
using Office.Interaction;

// This is the Editor for the Interactable MonoBehaviour.
// However, since the Interactable contains many sub-objects, 
// it requires many sub-editors to display them.
// For more details see the EditorWithSubEditors class.
[CustomEditor(typeof(Interactable))]
public class InteractableEditor : EditorWithSubEditors<ConditionCollectionEditor, ConditionCollection>
{
    private Interactable interactable;
    private SerializedProperty matchTargetTransformProperty;        // Represents the matchTargetTransform bool on the Interactable. KG
    private SerializedProperty collectionsProperty;                 // Represents the ConditionCollection array on the Interactable.
    private SerializedProperty defaultReactionCollectionProperty;   // Represents the ReactionCollection which is used if none of the ConditionCollections are.


    private const float collectionButtonWidth = 125f;
    private const string interactablePropMatchTargetTransformName = "matchTargetTransform"; // KG
    private const string interactablePropConditionCollectionsName = "conditionCollections";
    private const string interactablePropDefaultReactionCollectionName = "defaultReactionCollection";

    private void OnEnable ()
    {
        // Cache the target reference.
        interactable = (Interactable)target;

        // Cache the SerializedProperties.
        matchTargetTransformProperty = serializedObject.FindProperty(interactablePropMatchTargetTransformName);
        collectionsProperty = serializedObject.FindProperty(interactablePropConditionCollectionsName);
        defaultReactionCollectionProperty = serializedObject.FindProperty(interactablePropDefaultReactionCollectionName);
        
        // Create the necessary Editors for the ConditionCollections.
        CheckAndCreateSubEditors(interactable.conditionCollections);
    }


    private void OnDisable ()
    {
        // When the InteractableEditor is disabled, destroy all the ConditionCollection editors.
        CleanupEditors ();
    }


    // This is called when the ConditionCollection editors are created.
    protected override void SubEditorSetup(ConditionCollectionEditor editor)
    {
        // Give the ConditionCollection editor a reference to the array to which it belongs.
        editor.collectionsProperty = collectionsProperty;
    }


    public override void OnInspectorGUI ()
    {
        // Pull information from the target into the serializedObject.
        serializedObject.Update ();

        // Displays matchTargetTransform bool (KG)
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        matchTargetTransformProperty.boolValue = EditorGUILayout.Toggle(new GUIContent("Match Target Transform", "Will player match target position and rotation."), matchTargetTransformProperty.boolValue);
        EditorGUILayout.EndHorizontal();

        // If necessary, create editors for the ConditionCollections.
        CheckAndCreateSubEditors(interactable.conditionCollections);

        // Display all of the ConditionCollections.
        for (int i = 0; i < subEditors.Length; i++)
        {
            subEditors[i].OnInspectorGUI ();
            EditorGUILayout.Space ();
        }

        // Create a right-aligned button which when clicked, creates a new ConditionCollection in the ConditionCollections array.
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace ();
        if (GUILayout.Button("Add Collection", GUILayout.Width(collectionButtonWidth)))
        {
            ConditionCollection newCollection = ConditionCollectionEditor.CreateConditionCollection ();
            collectionsProperty.AddToObjectArray (newCollection);
        }
        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.Space ();

        // Use the default object field GUI for the defaultReaction.
        EditorGUILayout.PropertyField (defaultReactionCollectionProperty);

        // Push information back to the target from the serializedObject.
        serializedObject.ApplyModifiedProperties ();
    }
}
