using UnityEngine;

public class ItemPickup : Interactible
{
    [SerializeField] private Item item;
    /*[SerializeField] private string id;

    private bool collected = false;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    
    public void LoadData(GameData data)
    {
        data.itemsCollected.TryGetValue(id, out collected);
        if (collected)
        {
            gameObject.SetActive(false);
        }
    }
    public void SaveData(ref GameData data) 
    {
        if (data.itemsCollected.ContainsKey(id))
        {
            data.itemsCollected.Remove(id);
        }
        data.itemsCollected.Add(id, collected);
    }*/

    private void PickUp()
    {
        bool wasPickedUp = Inventory.instance.Add(item);
        if(wasPickedUp)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public override void Interact()
    {
        PickUp();
    }
}
