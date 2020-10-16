using UnityEngine;

namespace Office.Props.Abstract {
    public abstract class ResettableScriptableObject : ScriptableObject {
        public abstract void Reset();
    }
}
