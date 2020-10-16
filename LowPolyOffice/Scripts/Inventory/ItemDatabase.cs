using System.Collections.Generic;
using UnityEngine;

namespace Office.Inventory {
    public class ItemDatabase : MonoBehaviour {

        #region Singleton
        public static ItemDatabase instance;

        private void Awake() {
            if (instance) Debug.LogWarning("More than one instance of ItemDatabase found!");
            instance = this;
        }
        #endregion

        private void Start() {
            LoadItems("EN");
        }

        private List<Item> items {
            get {
                if (itemCollection == null) LoadItems("EN");
                return itemCollection.items;
            }
        }

        public Item GetItemName(string uniqueItemName) {
            foreach (var item in items) {
                if (item.uniqueName.Equals(uniqueItemName)) return item;
            }
            return new Item(uniqueItemName, "NO_NAME", "NO_DESCRIPTION");
        }

        [SerializeField]
        private ItemList itemCollection;

        public void LoadItems(string language) {
            //Debug.Log($"Loading {language} items...");
            try {
                string json = Resources.Load<TextAsset>("Items/" + language).text;
                itemCollection = JsonUtility.FromJson<ItemList>(json);
                //Debug.Log($"Items loaded successfully! {items.Count} item{((items.Count == 1) ? "" : "s")} found!");
            } catch (System.NullReferenceException) {
                Debug.LogError($"Failed to load {language} items!");
            }
        }

        [System.Serializable]
        private class ItemList {
            public List<Item> items;
            public ItemList() {
                items = new List<Item>();
            }
        }

        [System.Serializable]
        public class Item {
            public string uniqueName;
            public string title;
            public string description;

            public Item(string uniqueName, string title, string description) {
                this.uniqueName = uniqueName;
                this.title = title;
                this.description = description;
            }
        }

    }
}