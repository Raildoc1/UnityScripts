using UnityEngine;

namespace Office.Interaction {
    [CreateAssetMenu]
    public class Condition : ScriptableObject {
        public string description;
        public bool _isSatisfied = false;
        public virtual bool isSatisfied { 
            get {
                return _isSatisfied;
            } 
            set { 
                _isSatisfied = value; 
            } 
        }
        public int hash;
    }
}