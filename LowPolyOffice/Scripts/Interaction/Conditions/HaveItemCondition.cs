using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Office.Interaction {
    public class HaveItemCondition : Condition {

        public string uniqueName; 
        public int amount = 1;

        public override bool isSatisfied {
            get {
                return GameObject.FindGameObjectWithTag("Player").GetComponent<ItemCollection>().HaveItems(uniqueName, amount);
            }
        }
    }
}