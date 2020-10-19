using Office.Text;
using UnityEngine;

namespace Office.Props {
    public class NameHolder : MonoBehaviour {
        [SerializeField]
        private string UniqueName = "";

        public string GetName() {
            return TextDatabase.instance.GetTextByName(UniqueName);
        }
    }
}
