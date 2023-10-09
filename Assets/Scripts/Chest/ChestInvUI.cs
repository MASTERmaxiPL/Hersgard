using System.Collections.Generic;
using System.Linq;
using Ink.Parsed;
using UnityEngine;
using UnityEngine.UI;

public class ChestInvUI : MonoBehaviour
{
    private ChestInvManager inventory;

    private static ChestInvUI instance;

    private DropItemMenu dropItemMenu;

    public static ChestInvUI GetInstance()
    {
        return instance;
    }

    public bool isDropMenuOpened = false;

    public bool isMarked = false;

    public ScrollViewItem[] slots;

    [SerializeField]
    private Transform scrollViewContent;

    [SerializeField]
    private GameObject inventorySlot;

    private void Awake()
    {
        instance = this;
        inventory = ChestInvManager.GetInstance();
        inventory.onItemChangedCallback += UpdateUI;
        slots = GetComponentsInChildren<ScrollViewItem>();
    }


    public void UpdateUI()
    {
        Debug.Log("UpdatingChestUI");
        if (slots.Length == 0)
        {
            InitiateSlots();
            slots[0].AddItem(ChestInvManager.GetInstance().items[0]);
            if (!slots[0].item.canStack)
            {
                slots[0].TextRemove(slots[0].item);
            }
            else
            {
                slots[0].TextRefresh(slots[0].GetComponentInParent<ScrollViewItem>());
            }
        }
        else if (ChestInvManager.GetInstance().stackable)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].TextRefresh(slots[i].GetComponentInParent<ScrollViewItem>());
            }
        }
        else
        {
            InitiateSlots();
            AddItemsToSlots();
        }
    }
    private void InitiateSlots()
    {
        GameObject newInventorySlot = Instantiate(inventorySlot, scrollViewContent);
        slots = GetComponentsInChildren<ScrollViewItem>();
    }
    private void AddItemsToSlots()
    {
        int num = slots.Length-1;
        slots[num].AddItem(inventory.items[num]);
        if (!slots[num].item.canStack)
        {
            slots[num].TextRemove(slots[num].item);
        }
        else
        {
            slots[num].TextRefresh(slots[num].GetComponentInParent<ScrollViewItem>());
        }
    }

    public void OnRemoveButton(Item item)
    {
        dropItemMenu = DropItemMenu.GetInstance();

        dropItemMenu.isDropMenuOpened = true;
        dropItemMenu.Panel.SetActive(true);
        isDropMenuOpened = true;
        //
        dropItemMenu.itemToInventory = true;
        //
        if (item.canStack && item.inChestAmount > 1)
        {
            dropItemMenu.text.text = "How many items you want to drop?";
            dropItemMenu.slider.gameObject.SetActive(true);
            dropItemMenu.slider.maxValue = item.inChestAmount;
            dropItemMenu.slider.value = dropItemMenu.slider.maxValue;
        }
        else
        {
            dropItemMenu.text.text = "Are you sure you want to delete this item?";
            dropItemMenu.slider.gameObject.SetActive(false);
            dropItemMenu.number.text = null;
        }
    }

    public bool TryRemoveSlot(Item slot)
    {
        if (gameObject.transform != null)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.GetComponent<ScrollViewItem>().item == slot)
                {
                    Destroy(child.gameObject);
                    return true;
                }
            }
        }
        return false;
    }
}

