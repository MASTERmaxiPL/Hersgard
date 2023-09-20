using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int price;
    public float weight;
    public int amount=1;
    public int inChestAmount=1;
    public bool canStack;
    public bool canUse;
    public bool canEquip;

    public virtual void Use()
    {

    }
    public string GetName()
    {
        return itemName;
    }
}
