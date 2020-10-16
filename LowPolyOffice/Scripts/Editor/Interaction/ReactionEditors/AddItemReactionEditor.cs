using Office.Interaction;
using UnityEditor;

[CustomEditor(typeof(AddItemReaction))]
public class AddItemReactionEditor : ReactionEditor
{
    protected override string GetFoldoutLabel() {
        return "Add Item Reaction";
    }
}
