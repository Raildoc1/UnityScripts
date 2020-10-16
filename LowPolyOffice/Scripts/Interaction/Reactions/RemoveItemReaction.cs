using UnityEngine;

namespace Office.Interaction {
    public class RemoveItemReaction : Reaction {

        public string uniqueName;
        public int amount = 1;

        protected override void ImmediateReaction() {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ItemCollection>().RemoveItem(uniqueName, amount);
        }
    }
}
