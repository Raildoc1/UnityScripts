using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour {

    [SerializeField]
    private List<Slot> items = new List<Slot>();

    public void AddItem(string uniqueName, int amount = 1) {
        foreach (var i in items) {
            if (i.uniqueName.Equals(uniqueName)) {
                i.amount += amount;
                return;
            }
        }
        Slot newSlot = new Slot(uniqueName, amount);
        items.Add(newSlot);
    }

    public void RemoveItem(string uniqueName, int amount = 1) {
        foreach (var i in items) {
            if (i.uniqueName.Equals(uniqueName)) {
                i.amount -= amount;
                if (i.amount < 1) items.Remove(i);
                return;
            }
        }
        Debug.LogError("No {uniqueName} found in Player's inventory!");
    }

    public bool HaveItems(string uniqueName, int amount = 1) {
        foreach (var i in items) {
            if (i.uniqueName.Equals(uniqueName)) {
                if (i.amount >= amount) return true;
                else return false;
            }
        }
        return false;
    }

    [System.Serializable]
    public class Slot {
        public string uniqueName;
        public int amount;
        public Slot(string uniqueName, int amount = 1) {
            this.uniqueName = uniqueName;
            this.amount = amount;
        }
    }

}
