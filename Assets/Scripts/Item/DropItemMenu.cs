using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DropItemMenu : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Slider slider;
    [SerializeField]
    private GameObject dropItemsPanel;
    public GameObject Panel;

    public TextMeshProUGUI number;

    private static DropItemMenu instance;

    public ScrollViewItem lastMarkedScrollViewItem;
    public ScrollViewEquipment lastMarkedScrollViewEquipment;

    private string lastMarked = "";

    private bool equipmentDeleted = false;

    public bool isDropMenuOpened = false;

    public bool itemToInventory = false;

    [SerializeField]
    private float waitTime = 5f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
        Panel.SetActive(false);

        dropItemsPanel.gameObject.transform.Find("Panel");
        slider.onValueChanged.AddListener((v) =>
        {
            number.text = v.ToString();
        });
    }

    public void ActiveDropItem()
    {
        gameObject.SetActive(true);
    }
    public void UnactiveDropItem()
    {
        gameObject.SetActive(false);
    }

    public static DropItemMenu GetInstance()
    {
        return instance;
    }


    private void MarkItem(ScrollViewItem item)
    {
        item.mark.enabled = true;
        item.isMarked = true;
        lastMarkedScrollViewItem = item;
    }
    private void MarkEquipment(ScrollViewEquipment equipment)
    {
        equipment.mark.enabled = true;
        equipment.isMarked = true;
        equipment.mark.sprite = null;
        lastMarkedScrollViewEquipment = equipment;
    }

    public void CloseDropMenu()
    {
        MenuManager.GetInstance().isDropMenuOpened = false;
        Panel.SetActive(false);
        InventoryUI.GetInstance().isDropMenuOpened = false;
    }


    public void MarkNewSlotItem(ScrollViewItem item)
    {

        CheckIfMarkedBefore();
        Debug.Log("MarkNewSlotItem");
        lastMarked = "Item";

        MarkItem(item);
    }
    public void MarkNewSlotEquipment(ScrollViewEquipment equipment)
    {
        CheckIfMarkedBefore();

        lastMarked = "Equipment";

        MarkEquipment(equipment);
    }

    public void UnmakrItem(ScrollViewItem item)
    {
        item.isMarked = false;
        item.mark.enabled = false;
        lastMarkedScrollViewItem = null;
    }
    public void UnmakrEquipment(ScrollViewEquipment equipment)
    {
        equipment.isMarked = false;
        equipment.mark.enabled = false;
        lastMarkedScrollViewEquipment = null;
    }

    private void CheckIfMarkedBefore()
    {
        if (lastMarkedScrollViewItem != null)
        {
            Debug.Log("Unmark Item");
            UnmakrItem(lastMarkedScrollViewItem);
        }
        if (lastMarkedScrollViewEquipment != null)
        {
            Debug.Log("Unmark Equipment");
            UnmakrEquipment(lastMarkedScrollViewEquipment);
        }
    }

    private void FixedUpdate()
    {
        if (Player.GetInstance().deleteItem)
        {
            DeleteItem();
        }

        if (Player.GetInstance().useItem)
        {
            UseItem();
        }

        if (Player.GetInstance().equip)
        {
            EquipItem();
        }

        if (Player.GetInstance().confirm && MenuManager.GetInstance().isDropMenuOpened)
        {
            Debug.Log("CONFIRM");
            if (lastMarked == "Item")
            {
                lastMarkedScrollViewItem.chosenAmount = 1;
                if (itemToInventory && lastMarkedScrollViewItem.isMarked && lastMarkedScrollViewItem.item != null)
                {
                    itemToInventory = false;
                    lastMarkedScrollViewItem.chosenAmount = (int)slider.value;

                    if (lastMarkedScrollViewItem.chosenAmount == lastMarkedScrollViewItem.item.inChestAmount)
                    {
                        lastMarkedScrollViewItem.item.amount += (int)slider.value;
                        Inventory.instance.isItemFromChest = true;
                        Inventory.instance.Add(lastMarkedScrollViewItem.item);
                        lastMarkedScrollViewItem.item.inChestAmount = 0;
                        ChestInvManager.GetInstance().Remove(lastMarkedScrollViewItem);
                        Destroy(lastMarkedScrollViewItem.gameObject);
                        InventoryUI.GetInstance().SetIsDropMenuOpen();
                    }
                    else
                    {
                        lastMarkedScrollViewItem.item.amount += (int)slider.value;
                        lastMarkedScrollViewItem.item.inChestAmount -= (int)slider.value;
                        Inventory.instance.isItemFromChest = true;
                        Inventory.instance.Add(lastMarkedScrollViewItem.item);
                        lastMarkedScrollViewItem.TextRefresh(lastMarkedScrollViewItem);
                        lastMarkedScrollViewItem.mark.enabled = false;
                        lastMarkedScrollViewItem.isMarked = false;
                        InventoryUI.GetInstance().SetIsDropMenuOpen();
                    }
                }
                else if (lastMarkedScrollViewItem.isMarked && lastMarkedScrollViewItem.item != null)
                {
                    if (lastMarkedScrollViewItem.item.canStack && lastMarkedScrollViewItem.item.amount > 1)
                    {
                        lastMarkedScrollViewItem.chosenAmount = (int)slider.value;

                        if (lastMarkedScrollViewItem.chosenAmount == lastMarkedScrollViewItem.item.amount)
                        {
                            if (ChestInvManager.GetInstance().isPlayerChestOpened)
                            {
                                lastMarkedScrollViewItem.item.inChestAmount += lastMarkedScrollViewItem.item.amount;
                                ChestInvManager.GetInstance().AddItemToPlayerChest(lastMarkedScrollViewItem.item);
                            }  
                            lastMarkedScrollViewItem.item.amount = 0;
                            Inventory.instance.Remove(lastMarkedScrollViewItem);
                            Destroy(lastMarkedScrollViewItem.gameObject);
                        }
                        else
                        {
                            lastMarkedScrollViewItem.item.amount -= (int)slider.value;
                            if (ChestInvManager.GetInstance().isPlayerChestOpened)
                            {
                                ScrollViewItem lastviewItem = lastMarkedScrollViewItem;
                                lastviewItem.item.inChestAmount += (int)slider.value;
                                lastMarkedScrollViewItem.TextRefresh(lastMarkedScrollViewItem);
                                ChestInvManager.GetInstance().AddItemToPlayerChest(lastviewItem.item);
                            }
                            lastMarkedScrollViewItem.TextRefresh(lastMarkedScrollViewItem);
                            lastMarkedScrollViewItem.mark.enabled = false;
                            lastMarkedScrollViewItem.isMarked = false;
                        }
                    }
                    else
                    {
                        if (ChestInvManager.GetInstance().isPlayerChestOpened)
                        {
                            if(lastMarkedScrollViewItem.item.canStack && lastMarkedScrollViewItem.item.amount == 1)
                            {
                                ScrollViewItem lastviewItem = lastMarkedScrollViewItem;
                                lastviewItem.item.inChestAmount += 1;
                                lastviewItem.item.amount -= 1;
                                ChestInvManager.GetInstance().AddItemToPlayerChest(lastviewItem.item);
                                lastMarkedScrollViewItem.TextRefresh(lastMarkedScrollViewItem);
                            }
                            else
                            {
                                ChestInvManager.GetInstance().AddItemToPlayerChest(lastMarkedScrollViewItem.item);
                            }
                        }
                        Inventory.instance.Remove(lastMarkedScrollViewItem);
                        Destroy(lastMarkedScrollViewItem.gameObject);
                    }

                    Inventory.instance.currWeight -= lastMarkedScrollViewItem.item.weight * lastMarkedScrollViewItem.chosenAmount;
                    lastMarkedScrollViewItem.isMarked = false;
                    Panel.SetActive(false);
                    InventoryUI.GetInstance().SetIsDropMenuOpen();
                    lastMarkedScrollViewItem.chosenAmount = 1;
                    lastMarkedScrollViewItem = null;
                }
            }
            else if (lastMarked == "Equipment")
            {
                if (lastMarkedScrollViewEquipment.isMarked && lastMarkedScrollViewEquipment.item != null)
                {
                    if (lastMarkedScrollViewEquipment.item.canStack && lastMarkedScrollViewEquipment.item.amount > 1)
                    {
                        lastMarkedScrollViewEquipment.chosenAmount = (int)slider.value;
                    }
                    if (lastMarkedScrollViewEquipment.chosenAmount == lastMarkedScrollViewEquipment.item.amount)
                    {
                        lastMarkedScrollViewEquipment.chosenAmount = 1;
                        equipmentDeleted = true;
                    }
                    else
                    {
                        lastMarkedScrollViewEquipment.item.amount = lastMarkedScrollViewEquipment.item.amount - lastMarkedScrollViewEquipment.chosenAmount;
                        lastMarkedScrollViewEquipment.TextRefresh(lastMarkedScrollViewEquipment.item);

                        lastMarkedScrollViewEquipment.mark.enabled = false;
                        lastMarkedScrollViewEquipment.isMarked = false;
                    }
                    Inventory.instance.currWeight -= lastMarkedScrollViewEquipment.item.weight * lastMarkedScrollViewEquipment.chosenAmount;
                    lastMarkedScrollViewEquipment.isMarked = false;
                    Panel.SetActive(false);
                    InventoryUI.GetInstance().SetIsDropMenuOpen();
                    lastMarkedScrollViewEquipment.image.sprite = lastMarkedScrollViewEquipment.helper;

                    if (equipmentDeleted)
                    {
                        Equipment[] slots2 = EquipmentManager.instance.currEquipment;
                        bool deleteFromSlot = false;
                        int i = 0;
                        while (!deleteFromSlot)
                        {
                            if (slots2[i] == lastMarkedScrollViewEquipment.item)
                            {
                                EquipmentManager.instance.DeleteItem(i);
                                StartCoroutine(StartCooldown());
                                deleteFromSlot= true;
                            }
                            i++;
                        }
                        EquipmentSlots.GetInstance().UnsetItemToSlot(lastMarkedScrollViewEquipment);

                        lastMarkedScrollViewEquipment.icon.SetActive(false);
                        lastMarkedScrollViewEquipment.itemTypeHelp.SetActive(true);
                        lastMarkedScrollViewEquipment.mark.enabled = false;
                        lastMarkedScrollViewEquipment.isMarked = false;
                        lastMarkedScrollViewEquipment.item = null;
                        lastMarkedScrollViewEquipment = null;
                        equipmentDeleted = false;
                        lastMarked = "";
                    }
                }
            }
        }
    }


    private void DeleteItem()
    {
        if(lastMarked == "Item")
        {
            Debug.Log("DeleteItem");
            if (lastMarkedScrollViewItem.isMarked && lastMarkedScrollViewItem.item != null)
            {
                Debug.Log("DeleteMarked");
                if (lastMarkedScrollViewItem.item.canStack)
                {
                    Debug.Log("Delete1");
                    lastMarkedScrollViewItem.OnRemoveItemButton();
                }
                else
                {
                    Debug.Log("Delete2");
                    lastMarkedScrollViewItem.item.amount = 1;
                    lastMarkedScrollViewItem.OnRemoveItemButton();
                }
            }
        }
        else if(lastMarked == "Equipment")
        {
            
            if (lastMarkedScrollViewEquipment.isMarked && lastMarkedScrollViewEquipment.item != null)
            {
                Debug.Log("DeleteEquipment");
                if (lastMarkedScrollViewEquipment.item.canStack)
                {
                    lastMarkedScrollViewEquipment.OnRemoveButton();
                }
                else
                {
                    lastMarkedScrollViewEquipment.item.amount = 1;
                    lastMarkedScrollViewEquipment.OnRemoveButton();
                }
            }
        }
        StartCoroutine(StartCooldown());
    }
    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(waitTime);
    }

    private void UseItem()
    {
        if (lastMarked == "Item")
        {         
            lastMarkedScrollViewItem.UseItem();
            
        }
        else if (lastMarked == "Equipment")
        {
            lastMarkedScrollViewEquipment.UseItem();           
        }
    }

    private void EquipItem()
    {
        if (lastMarked == "Item")
        {
            if (lastMarkedScrollViewItem.isMarked)
            {
                lastMarkedScrollViewItem.isMarked = false;
                lastMarkedScrollViewItem.EquipItem();
            }
        }
        else if (lastMarked == "Equipment")
        {
            if (lastMarkedScrollViewEquipment.isMarked)
            {
                lastMarkedScrollViewEquipment.isMarked = false;
                UnequipItem();
            }
        }
    }

    public void UnequipItem()
    {
        ScrollViewEquipment[] equipSlot = EquipmentSlots.GetInstance().slots;
        lastMarkedScrollViewEquipment.mark.enabled = false;
        lastMarkedScrollViewEquipment.isMarked = false;
        lastMarkedScrollViewEquipment.text.text = null;
        for (int i = 0; i < equipSlot.Length - 1; i++)
        {
            if (equipSlot[i] == lastMarkedScrollViewEquipment)
            {
                EquipmentManager.instance.Unequip(i);
                StartCoroutine(StartCooldown());
                return;
            }
        }
    }
}
