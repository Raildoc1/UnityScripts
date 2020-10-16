using Office.Interaction;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TriggerDialogReaction))]
public class TriggerDialogReactionEditor : ReactionEditor {
    protected override string GetFoldoutLabel() {
        return "Trigger Dialog Reaction";
    }
}
