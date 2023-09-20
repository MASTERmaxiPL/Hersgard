using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class ScrollViewItem : MonoBehaviour, IPointerClickHandler
{
    public Item item;

    public bool isMarked = false;

    public int chosenAmount = 1;

    public Image mark;

    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private float waitTime = 5f;

    public void AddItem(Item newItem)
    {
        item = newItem;
        image.sprite = item.icon;
        if (item.canStack)
        {
            text.text = newItem.amount.ToString();
        }
        else
        {
            text.text = string.Empty;
        }
    }
    public void TextRemove(Item newItem) 
    {
        text.text = string.Empty;
    }

    public void TextRefreshEquipment(Item newItem)
    {
        text.text = newItem.amount.ToString();
    }
    public void TextRefresh(ScrollViewItem newItem)
    {
        if (newItem.GetComponentInParent<InventoryUI>()) 
        {
            text.text = newItem.item.amount.ToString();
        }
        else
        {
            text.text = newItem.item.inChestAmount.ToString();
        }
        if (newItem.GetComponentInParent<ChestInvUI>())
        {

        }
    }
    public void TextChestRefresh(ScrollViewItem newItem)
    {
        text.text = newItem.item.inChestAmount.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && isMarked)
        {
            DropItemMenu.GetInstance().UnmakrItem(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            DropItemMenu.GetInstance().MarkNewSlotItem(this);
        }
        if (isMarked)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (ChestInvManager.GetInstance().isPlayerChestOpened)
                {
                    if (gameObject.transform.parent.name == "OwnedItems")
                    {
                        if (item.amount > 1)
                        {
                            InventoryUI.GetInstance().OnRemoveButton(item);
                        }
                        else
                        {
                            MoveSingleItemToChest();
                        }
                    }
                    else
                    {
                        if (item.inChestAmount > 1)
                        {
                            ChestInvUI.GetInstance().OnRemoveButton(item);
                        }
                        else
                        {
                            MoveSingleItemToInventory();
                        }
                    }
                }
                else
                {
                    EquipItem();
                }
            }
        }
    }

    private void MoveSingleItemToChest()
    {
        if (item.canStack)
        {
            item.inChestAmount += 1;
            item.amount = 0;
        }

        ChestInvManager.GetInstance().AddItemToPlayerChest(this.item);
        
        Inventory.instance.Remove(this);
        Destroy(gameObject);
        Inventory inv = Inventory.instance;
        inv.currWeight -= item.weight;
    }
    private void MoveSingleItemToInventory()
    {
        if (item.canStack)
        {
            item.amount += 1;
            item.inChestAmount = 0;
        }
        Inventory.instance.isItemFromChest = true;
        Inventory.instance.Add(item);
        ChestInvManager.GetInstance().Remove(this);
        Destroy(gameObject);
    }

    public void OnRemoveItemButton()
    {
        chosenAmount = item.amount;
        DropItemMenu.GetInstance().number.text = chosenAmount.ToString();

        InventoryUI.GetInstance().OnRemoveButton(item);
    }

    public void EquipButton()
    {
        if (item != null)
        {
            item.Use();
            if (item.canStack && item.amount > 1)
            {
                item.amount -= 1;
                Debug.Log("Done once");
                TextRefreshEquipment(item);
            }
            else
            {
                Debug.Log("Done");
                Inventory.instance.Remove(this);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(waitTime);
    }

    public void UseItem()
    {
        if (isMarked)
        {
            if (item.canUse)
            {
                Debug.Log("Use Item --- WORK IN PROGRESS");
                mark.enabled = false;
                isMarked = false;
                UseButton();
                StartCoroutine(StartCooldown());
            }
        }
    }

    public void UseButton()
    {
        EquipButton();
        Inventory.instance.currWeight -= item.weight;
    }

    public void EquipItem() 
    {
        if (item.canEquip)
        {
            mark.enabled = false;
            isMarked = false;
            EquipButton();
            StartCoroutine(StartCooldown());
        }
    }
}
