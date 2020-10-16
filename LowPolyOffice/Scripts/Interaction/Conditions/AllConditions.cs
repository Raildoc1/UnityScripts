using UnityEngine;
using Office.Props.Abstract;

namespace Office.Interaction {
    public class AllConditions : ResettableScriptableObject {

        public Condition[] conditions;

        private static AllConditions _instance;

        private const string loadPath = "AllConditions";

        public static AllConditions instance {
            get {
                if (!_instance)
                    _instance = FindObjectOfType<AllConditions>();
                if (!_instance)
                    _instance = Resources.Load<AllConditions>(loadPath);
                if (!_instance)
                    Debug.LogError("AllConditions not found!");
                return _instance;
            }
            set { _instance = value; }
        }

        public override void Reset() {
            if (conditions == null) return;

            for (int i = 0; i < conditions.Length; i++) {
                conditions[i].isSatisfied = false;
            }
        }

        public static bool CheckCondition(Condition condition) {

            Condition[] allConditions = instance.conditions;
            Condition globalCondition = null;

            if ( (allConditions != null) && (allConditions[0] != null) ) {
                for (int i = 0; i < allConditions.Length; i++) {
                    if (allConditions[i].hash == condition.hash)
                        globalCondition = allConditions[i];
                }
            }

            if (!globalCondition) return false;

            return globalCondition.isSatisfied == condition.isSatisfied;
        }
    }
}
