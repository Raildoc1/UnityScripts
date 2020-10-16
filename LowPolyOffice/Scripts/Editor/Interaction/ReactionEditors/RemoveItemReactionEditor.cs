using Office.Interaction;
using UnityEditor;

[CustomEditor(typeof(RemoveItemReaction))]
public class RemoveItemReactionEditor : ReactionEditor {
    protected override string GetFoldoutLabel() {
        return "Remove Item Reaction";
    }
}
