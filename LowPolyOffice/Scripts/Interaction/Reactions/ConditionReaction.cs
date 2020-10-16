namespace Office.Interaction {
    public class ConditionReaction : Reaction {
        public Condition condition;     // The Condition to be changed.
        public bool satisfied;          // The satisfied state the Condition will be changed to.


        protected override void ImmediateReaction() {
            condition.isSatisfied = satisfied;
        }
    }
}
