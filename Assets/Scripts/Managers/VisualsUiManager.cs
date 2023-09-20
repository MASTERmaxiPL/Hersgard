using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualsUiManager : MonoBehaviour
{
    private static VisualsUiManager instance;

    [SerializeField] private Animator helmetVisuals;
    [SerializeField] private Animator chestVisuals;
    [SerializeField] private Animator legsVisuals;
    [SerializeField] private Animator bootsVisuals;

    [SerializeField] private GameObject ChestHelmetVisuals;
    [SerializeField] private GameObject ChestChestVisuals;
    [SerializeField] private GameObject ChestLegsVisuals;
    [SerializeField] private GameObject ChestBootsVisuals;

    [SerializeField] private GameObject HelmetSlot;
    [SerializeField] private GameObject ChestSlot;
    [SerializeField] private GameObject LegsSlot;
    [SerializeField] private GameObject BootsSlot;
    public static VisualsUiManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;

        UnsetChestVis(ChestHelmetVisuals);
        UnsetChestVis(ChestChestVisuals);
        UnsetChestVis(ChestLegsVisuals);
        UnsetChestVis(ChestBootsVisuals);
    }

    private GameObject MarkedSlot = null;

    [SerializeField]
    private GameObject VisualsViewport;

    [SerializeField]
    private GameObject ItemGrid;

    [SerializeField]
    private GameObject inventorySlot;

    private string EquipmentType;

    public void ShowItemsToEquip(GameObject slotToVisualize)
    {
        string NameOfSlot = slotToVisualize.name;
        switch (NameOfSlot)
        {
            case "helmetSlot":
                print("helmet");
                SearchForEquipment("Helmet", slotToVisualize);
                break;
            case "chestSlot":
                print("chest");
                SearchForEquipment("Chestplate", slotToVisualize);
                break;
            case "legsSlot":
                print("legs");
                SearchForEquipment("Leggins", slotToVisualize);
                break;
            case "bootsSlot":
                print("boots");
                SearchForEquipment("Boots", slotToVisualize);
                break;
            default:
                print("something is wrong with visual equipment slot");
                break;
        }
    }
    private void SearchForEquipment(string slot, GameObject slotToVisualize)
    {
        DeleteSlots();
        Equipment eqItem;
        List<Equipment> slotList = new List<Equipment>(); ;
        foreach (Item item in ChestInvManager.GetInstance().items)
        {
            if (item.canEquip)
            {
                eqItem = (Equipment)item;
                if (eqItem.equipSlot.ToString() == slot)
                {
                    slotList.Add(eqItem);
                }
            }
        }
        foreach (Item item in Inventory.instance.items)
        {
            if (item.canEquip)
            {
                eqItem = (Equipment)item;
                if (eqItem.equipSlot.ToString() == slot)
                {
                    slotList.Add(eqItem);
                }
            }
        }
        if(slotList.Count > 0)
        {
            SetMarked(slotToVisualize);
            CreateSlots(slotList);
            TransformToSlot(slotToVisualize);
        }
        else
        {
            HideItemsToEquip();
        }
    }
    private void CreateSlots(List<Equipment> slotList)
    {
        foreach (Equipment x in slotList)
        {
            inventorySlot.GetComponent<ScrollViewVisual>().AddItem(x);
            GameObject newInventorySlot = Instantiate(inventorySlot, ItemGrid.transform);
            print(x.name);
        }
    }
    private void DeleteSlots()
    {
        foreach(Transform child in ItemGrid.transform)
        {
            Destroy(child.gameObject);
        }
    }
    private void TransformToSlot(GameObject slotToVisualize)
    {
        Vector2 newPosition = new Vector2(slotToVisualize.GetComponent<RectTransform>().anchoredPosition.x + 210, slotToVisualize.GetComponent<RectTransform>().anchoredPosition.y - 130);
        VisualsViewport.GetComponent<RectTransform>().anchoredPosition = newPosition;
    }
    public void HideItemsToEquip()
    {
        VisualsViewport.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000, 0);
        DeleteSlots();
        MarkedSlot = null;
    }

    public void SetMarked(GameObject slot)
    {
        MarkedSlot = slot;
        Image imgComponent = ItemGrid.gameObject.GetComponent<Image>();
        imgComponent.enabled = true;
    }
    public void UnsetMarked()
    {
        MarkedSlot = null;
        Image imgComponent = gameObject.GetComponent<Image>();
        imgComponent.enabled = false;
    }
    public string GetMarked()
    {
        if(MarkedSlot != null)
        {
            return MarkedSlot.name;
        }
        return null;
    }

    public void EquipToSlot(Equipment item)
    {
        EquipmentType = item.equipSlot.ToString();
        switch (EquipmentType)
        {
            case "Helmet":
                MarkedSlot = HelmetSlot;
                EquipStructure(item, helmetVisuals, 0, ChestHelmetVisuals);
                break;
            case "Chestplate":
                MarkedSlot = ChestSlot;
                EquipStructure(item, chestVisuals, 1, ChestChestVisuals);
                break;
            case "Leggins":
                MarkedSlot = LegsSlot;
                EquipStructure(item, legsVisuals, 2, ChestLegsVisuals);
                break;
            case "Boots":
                MarkedSlot = BootsSlot;
                EquipStructure(item, bootsVisuals, 3, ChestBootsVisuals);
                break;
            default: 
                print("something went wrong");
                break;
        }
        DropSlot(item);
        MarkedSlot= null;
    }

    private void EquipStructure(Equipment item, Animator visuals, int num, GameObject chestVisuals)
    {
        if(MarkedSlot != null)
        {

        }
        UnequipItem(MarkedSlot);
        MarkedSlot.GetComponentInChildren<CharacterVisualSlot>().SetItem(item);
        ChangePlayersVisuals(visuals, item);
        ChestInvManager.GetInstance().ChestEquipment[num] = item;
        SetChestVis(item.animController, chestVisuals);
    }

    private void UnequipItem(GameObject slot)
    {
        Equipment item = slot.GetComponent<CharacterVisualSlot>().item;
        slot.GetComponent<CharacterVisualSlot>().item = null;
        if (item != null)
        {
            Inventory.instance.Add(item);
        }
    }

    private void ChangePlayersVisuals(Animator anim, Equipment item)
    {
        if (!ChestInvManager.GetInstance().GetisInventoryVisualsOn())
        {
            anim.runtimeAnimatorController = item.animController;
        }
    }
    private void SetChestVis(RuntimeAnimatorController animContrloler, GameObject obj)
    {
        obj.GetComponent<Animator>().runtimeAnimatorController = animContrloler;
        StartCoroutine(MenuManager.GetInstance().WaitAndPrint(0.3f, obj));
    }
    private void UnsetChestVis(GameObject obj)
    {
        var image = obj.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        obj.GetComponent<Image>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
    }
    public void TurnOffPlayerVis(GameObject obj)
    {
    string NameOfSlot = obj.name;
        switch (NameOfSlot)
        {
            case "helmetSlot":
                print("helmet");
                helmetVisuals.GetComponent<Animator>().runtimeAnimatorController = ChestInvManager.GetInstance().NoVisualsAnim;
                ChestInvManager.GetInstance().ChestEquipment[0] = null;
                UnsetChestVis(ChestHelmetVisuals);
                break;
            case "chestSlot":
                chestVisuals.GetComponent<Animator>().runtimeAnimatorController = ChestInvManager.GetInstance().NoVisualsAnim;
                ChestInvManager.GetInstance().ChestEquipment[1] = null;
                UnsetChestVis(ChestChestVisuals);
                break;
            case "legsSlot":
                legsVisuals.GetComponent<Animator>().runtimeAnimatorController = ChestInvManager.GetInstance().NoVisualsAnim;
                ChestInvManager.GetInstance().ChestEquipment[2] = null;
                UnsetChestVis(ChestLegsVisuals);
                break;
            case "bootsSlot":
                bootsVisuals.GetComponent<Animator>().runtimeAnimatorController = ChestInvManager.GetInstance().NoVisualsAnim;
                ChestInvManager.GetInstance().ChestEquipment[3] = null;
                UnsetChestVis(ChestBootsVisuals);
                break;
            default:
                print("something is wrong with visual equipment slot");
                break;
            }
    }

    private void DropSlot(Item slot)
    {
        bool done = ChestInvUI.GetInstance().TryRemoveSlot(slot);
        if (!done)
        {
            InventoryUI.GetInstance().TryRemoveSlot(slot);
            Inventory.instance.items.Remove(slot);
        }
        else
        {
            ChestInvManager.GetInstance().items.Remove(slot);
        }
    }

    public void SetVisuals()
    {
        Debug.Log("Set");
        ChestHelmetVisuals.GetComponent<Image>().sprite = ChestHelmetVisuals.GetComponent<SpriteRenderer>().sprite;
        ChestChestVisuals.GetComponent<Image>().sprite = ChestChestVisuals.GetComponent<SpriteRenderer>().sprite;
        ChestLegsVisuals.GetComponent<Image>().sprite = ChestLegsVisuals.GetComponent<SpriteRenderer>().sprite;
        ChestBootsVisuals.GetComponent<Image>().sprite = ChestBootsVisuals.GetComponent<SpriteRenderer>().sprite;
    }
}
