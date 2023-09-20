using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;

    private static InventoryUI instance;

    private DropItemMenu dropItemMenu;

    public static InventoryUI GetInstance()
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
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        slots = GetComponentsInChildren<ScrollViewItem>();
    }


    private void UpdateUI()
    {
        Debug.Log("Updating UI");
        if (slots.Length == 0)
        {
            InitiateSlots();
            slots[0].AddItem(inventory.items[0]);
        }
        else if (inventory.stackable)
        {
            Debug.Log("a");
            AddItemsToSlots();
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
        int num = slots.Length - 1;
        slots[num].AddItem(inventory.items[num]);
    }


    public void OnRemoveButton(Item item)
    {
        dropItemMenu = DropItemMenu.GetInstance();

        MenuManager.GetInstance().isDropMenuOpened = true;
        dropItemMenu.Panel.SetActive(true);
        isDropMenuOpened = true;

        if (item.canStack && item.amount > 1)
        {
            dropItemMenu.text.text = "How many items you want to drop?";
            dropItemMenu.slider.gameObject.SetActive(true);
            dropItemMenu.slider.maxValue = item.amount;
            dropItemMenu.slider.value = dropItemMenu.slider.maxValue;
        }
        else
        {
            dropItemMenu.text.text = "Are you sure you want to drop this item?";
            dropItemMenu.slider.gameObject.SetActive(false);
            dropItemMenu.number.text = null;
        }
    }

    public void SetIsDropMenuOpen()
    {
        isDropMenuOpened = false;
        DropItemMenu.GetInstance().CloseDropMenu();
    }

    public bool TryRemoveSlot(Item slot)
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.GetComponent<ScrollViewItem>().item == slot)
            {
                Destroy(child.gameObject);
                return true;
            }
        }
        return false;
    }
}
