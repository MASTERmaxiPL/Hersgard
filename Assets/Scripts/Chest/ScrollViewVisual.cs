using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class ScrollViewVisual : MonoBehaviour, IPointerClickHandler
{
    public Equipment item;

    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI text;

    public void AddItem(Equipment newItem)
    {
        item = newItem;
        image.sprite = item.icon;
        text.text = string.Empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        VisualsUiManager.GetInstance().HideItemsToEquip();
        VisualsUiManager.GetInstance().EquipToSlot(item);
    }
}
