using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour, IData
{

    #region Singleton
    public static EquipmentManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public Equipment[] currEquipment;
    public bool isHelmetOn = true;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    [SerializeField]
    private Animator helmetBtn;
    [SerializeField]
    private Animator helmetAnimator;
    [SerializeField]
    private RuntimeAnimatorController helmetOff;
    [SerializeField]
    private GameObject inventoryHelmet;
    [SerializeField]
    private GameObject inventoryChest;
    [SerializeField]
    private GameObject inventoryLegs;
    [SerializeField]
    private GameObject inventoryBoots;
    [SerializeField]
    private Sprite helmetOffSprite;

    Inventory inventory;

    private void Start()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentType)).Length;
        currEquipment = new Equipment[numSlots];
    }

    public void LoadData(GameData data)
    {
        isHelmetOn = data.isHelmetVisualOn;

        foreach (Equipment item in data.inventoryEquipment)
        {
            if (item != null)
            {
                Equip(item);
            }
        }
    }
    public void SaveData(GameData data)
    {
        data.isHelmetVisualOn = isHelmetOn;

        data.inventoryEquipment = this.currEquipment.ToList();
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = null;

        if (currEquipment[slotIndex] != null)
        {
            oldItem = currEquipment[slotIndex];
            inventory.Add(oldItem);
            inventory.currWeight -= oldItem.weight;
        }

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
            Debug.Log(oldItem.itemName);
        }
        currEquipment[slotIndex] = newItem;

        EquipToSlot(newItem);
    }

    public void UnequipAll()
    {
        for(int i = 0; i < currEquipment.Length; i++)
        {
            Unequip(i);
        }
    }

    public void Unequip(int slotIndex)
    {
        Equipment oldItem = null;
        if (currEquipment[slotIndex] != null)
        {
            oldItem = currEquipment[slotIndex];
            inventory.Add(oldItem);
            inventory.currWeight -= oldItem.weight;
            UnequipFromSlot(oldItem);

            currEquipment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
                Debug.Log(oldItem);
            }
        }
    }

    private void EquipToSlot(Equipment newItem)
    {
        EquipmentSlots.GetInstance().EquipToSlot(newItem);
    }

    private void UnequipFromSlot(Equipment oldItem)
    {
        EquipmentSlots.GetInstance().UnquipToSlot(oldItem);
    }

    public void DeleteItem(int index)
    {
        currEquipment[index] = null;
    }
    public void IsHelmetOn()
    {
        if (isHelmetOn)
        {
            isHelmetOn= false;
            SetOffHelmetVisuals();
        }
        else
        {
            isHelmetOn= true;
            if (ChestInvManager.GetInstance().GetisInventoryVisualsOn())
            {
                if (currEquipment[0] != null)
                {
                    helmetAnimator.runtimeAnimatorController = currEquipment[0].animController;
                    inventoryHelmet.GetComponent<Image>().sprite = inventoryHelmet.GetComponent<SpriteRenderer>().sprite;
                }
            }
            else
            {
                if (ChestInvManager.GetInstance().ChestEquipment[0] != null)
                {
                    helmetAnimator.runtimeAnimatorController = ChestInvManager.GetInstance().ChestEquipment[0].animController;
                }
                if (currEquipment[0] != null)
                {
                    inventoryHelmet.GetComponent<Image>().sprite = inventoryHelmet.GetComponent<SpriteRenderer>().sprite;
                }    
            }

        }
        SetIsHelmetOn();
    }
    public void SetIsHelmetOn()
    {
        helmetBtn.SetBool("IsHelmetOn", isHelmetOn);
    }
    public void SetOffHelmetVisuals ()
    {
        helmetAnimator.runtimeAnimatorController = helmetOff;
        inventoryHelmet.GetComponent<Image>().sprite = helmetOffSprite;
    }

    public IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetVisuals();
        yield break;
    }

    private void SetVisuals()
    {
        Debug.Log("Set");
        inventoryHelmet.GetComponent<Image>().sprite = inventoryHelmet.GetComponent<SpriteRenderer>().sprite;
        inventoryChest.GetComponent<Image>().sprite = inventoryChest.GetComponent<SpriteRenderer>().sprite;
        inventoryLegs.GetComponent<Image>().sprite = inventoryLegs.GetComponent<SpriteRenderer>().sprite;
        inventoryBoots.GetComponent<Image>().sprite = inventoryBoots.GetComponent<SpriteRenderer>().sprite;
    }
}
