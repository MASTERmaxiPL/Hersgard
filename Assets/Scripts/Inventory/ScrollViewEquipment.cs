using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ScrollViewEquipment : MonoBehaviour, IPointerClickHandler
{
    public Item item;

    public Image mark;

    public Image image;
    public Sprite helper;

    [SerializeField]
    public TextMeshProUGUI text;

    public bool isMarked = false;

    public int chosenAmount;

    [SerializeField]
    private float waitTime = 5f;

    public CharacterEquipment charSlot;

    public GameObject icon;
    public GameObject itemTypeHelp;

    public void SetItem(Item newItem)
    {
        item = newItem;
        if (item != null)
        {
            {
                if (item.canStack)
                {
                    text.text = item.amount.ToString();
                }
                else
                {
                    text.text = null;
                }
            }
            
        }
    }

    public void TextRefresh(Item newItem)
    {
        text.text = newItem.amount.ToString();
    }

    public void OnRemoveButton()
    {
        InventoryUI.GetInstance().isDropMenuOpened = true;
        chosenAmount = item.amount;
        DropItemMenu.GetInstance().number.text = chosenAmount.ToString();

        InventoryUI.GetInstance().OnRemoveButton(item);
    }

    

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && isMarked)
        {
            DropItemMenu.GetInstance().UnmakrEquipment(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            DropItemMenu.GetInstance().MarkNewSlotEquipment(this);
        }
        if (isMarked)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                UnequipItem();
            }
        }
    }

    public void UnequipItem()
    {
        DropItemMenu.GetInstance().UnequipItem();
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
    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(waitTime);
    }
}
