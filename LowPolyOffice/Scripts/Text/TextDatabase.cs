using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Office.Text {
    public class TextDatabase : MonoBehaviour {

        #region Singleton
        public static TextDatabase instance;

        private void Awake() {
            if (instance) Debug.LogWarning("More than one instance of ItemDatabase found!");
            instance = this;
            LoadNames("EN");
        }
        #endregion

        private TextCollection textCollection;

        private List<TextEntity> texts {
            get {
                return textCollection.texts;
            }
        }

        public string GetText(string uniqueName) {
            foreach (var text in texts) {
                if (text.uniqueName.Equals(uniqueName)) {
                    return text.text;
                }
            }
            return "NO_TEXT";
        }

        public void LoadNames(string language) {
            //Debug.Log($"Loading {language} names...");
            try {
                string json = Resources.Load<TextAsset>("Texts/" + language).text;
                textCollection = JsonUtility.FromJson<TextCollection>(json);
                //Debug.Log($"Items loaded successfully! {texts.Count} name{((texts.Count == 1) ? "" : "s")} found!");
            } catch (System.NullReferenceException) {
                Debug.LogError($"Failed to load {language} names!");
            }
        }

        [System.Serializable]
        private class TextCollection {
            public List<TextEntity> texts;
            TextCollection() {
                texts = new List<TextEntity>();
            }
        }

        [System.Serializable]
        private class TextEntity {
            public string uniqueName;
            public string text;
            public TextEntity(string uniqueName, string text) {
                this.uniqueName = uniqueName;
                this.text = text;
            }
        }

    }
}
