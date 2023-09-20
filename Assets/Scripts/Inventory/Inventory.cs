using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour, IData
{
    #region Singleton
    public static Inventory instance;
    public bool stackable =false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found");
            return;
        }

        instance = this;
    }
    #endregion

    public void LoadData(GameData data)
    {
        foreach(Item item in data.items.ToList()){
            Add(item);
        }

        currWeight = data.currWeight;
    }
    public void SaveData(GameData data)
    {
        data.items = this.items;
        data.currWeight = this.currWeight;
    }

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public List<Item> items = new List<Item>();
    public float maxWeight = 20;
    public float currWeight = 0;

    public bool isItemFromChest = false;

    public bool Add (Item item)
    {
        if (currWeight + item.weight <= maxWeight * 0.1 + maxWeight)
        {
            currWeight += item.weight;
            if (item.canStack)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].name == item.name)
                    {
                        if (!isItemFromChest)
                        {
                            items[i].amount++;
                        }
                        isItemFromChest = false;
                        stackable = true;
                        if (onItemChangedCallback != null)
                            onItemChangedCallback.Invoke();
                        stackable = false;
                        return true;
                    }
                }
            }
            if(item.amount == 0)
                item.amount = 1;
            items.Add(item);

            if(onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
            return true;
        }
        Debug.Log("You can't take any more items");
        return false;
    }
    public void Remove (ScrollViewItem item)
    {
        items.Remove(item.item);
    }
}
