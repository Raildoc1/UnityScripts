using Office.Interaction;
using UnityEditor;

[CustomEditor(typeof(FadeInOutReaction))]
public class FadeInOutReactionEditor : ReactionEditor {
    protected override string GetFoldoutLabel() {
        return "Fade In Out Text Reaction";
    }
}
