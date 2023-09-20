using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public float xPosition;
    public float yPosition;
    //public SerializableDictionary<string, bool> itemsCollected;
    public bool isHelmetVisualOn;
    public bool isInventoryVisualOn;

    public List<Item> items;
    public List<Equipment> inventoryEquipment;
    public List<Item> chestItems;
    public List<Equipment> chestEquipment;

    public float currWeight;

    public int saveSlotLimit;

    public GameData()
    {
        this.xPosition = 0;
        this.yPosition = 0;
        //this.itemsCollected= new SerializableDictionary<string, bool>();
        this.isHelmetVisualOn = true;
        this.isInventoryVisualOn = true;

        this.items = new List<Item>();
        this.inventoryEquipment = new List<Equipment>();
        this.chestItems = new List<Item>();
        this.chestEquipment = new List<Equipment>();

        this.currWeight = 0;

        this.saveSlotLimit = 50;
    }
}
