using System.Collections;
using UnityEngine;

namespace Office.Props {
    public class DestroyObject : MonoBehaviour {

        [Tooltip("Wait for seconds before destroy object")]
        public float delay = 1f;

        private IEnumerator Start() {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }

    }
}
