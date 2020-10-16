using UnityEngine;

namespace Office.Interaction {
    public class AddItemReaction : Reaction {

        public string uniqueName;
        public int amount = 1;

        protected override void ImmediateReaction() {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ItemCollection>().AddItem(uniqueName, amount);
        }
    }
}
