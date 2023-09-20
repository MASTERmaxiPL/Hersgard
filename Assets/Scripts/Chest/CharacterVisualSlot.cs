using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CharacterVisualSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject itemTypeHelp;

    public Equipment item;

    [SerializeField]
    private Image image;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (VisualsUiManager.GetInstance().GetMarked() == null)
            {
                UnsetItem();
            }
        }
        else if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (VisualsUiManager.GetInstance().GetMarked() == gameObject.name)
            {
                VisualsUiManager.GetInstance().HideItemsToEquip();
            }
            else
            {
                VisualsUiManager.GetInstance().ShowItemsToEquip(gameObject);
            }
        }
    }

    public void SetItem(Equipment newItem)
    {
        item = newItem;
        image.sprite = item.icon;
        image.gameObject.SetActive(true);
        itemTypeHelp.gameObject.SetActive(false);
    }
    public void UnsetItem()
    {

        if(item!= null)
        {
            image.gameObject.SetActive(false);
            itemTypeHelp.gameObject.SetActive(true);
            Inventory.instance.Add(item);
            item = null;

            if (!ChestInvManager.GetInstance().GetisInventoryVisualsOn())
            {
                VisualsUiManager.GetInstance().TurnOffPlayerVis(gameObject);
            }
        }
    }
}
