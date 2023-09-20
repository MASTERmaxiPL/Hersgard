using System.Collections;
using System.Linq;
using Ink.Parsed;
using UnityEditor.Animations;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EquipmentSlots : MonoBehaviour
{
    [SerializeField] private CharacterEquipment helmetSlot;
    [SerializeField] private CharacterEquipment chestSlot;
    [SerializeField] private CharacterEquipment legsSlot;
    [SerializeField] private CharacterEquipment bootsSlot;

    [SerializeField] private ScrollViewEquipment helmetSlotItem;
    [SerializeField] private ScrollViewEquipment chestSlotItem;
    [SerializeField] private ScrollViewEquipment legsSlotItem;
    [SerializeField] private ScrollViewEquipment bootsSlotItem;


    [SerializeField] private AnimatorController helmetAnimController;
    [SerializeField] private AnimatorController chestAnimController;
    [SerializeField] private AnimatorController legsAnimController;
    [SerializeField] private AnimatorController bootsAnimController;

    protected bool isMarked = false;

    public ScrollViewEquipment[] slots;

    #region Singleton
    private static EquipmentSlots instance;

    [SerializeField] private Animator helmetVisuals;
    [SerializeField] private Animator chestVisuals;
    [SerializeField] private Animator legsVisuals;
    [SerializeField] private Animator bootsVisuals;

    private void Awake()
    {
        slots = GetComponentsInChildren<ScrollViewEquipment>();

        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found");
            return;
        }
        instance = this;
    }
    private void Start()
    {
        MenuManager.GetInstance().CloseInventoryPages();
    }
    #endregion

    public static EquipmentSlots GetInstance()
    {
        return instance;
    }

    public void EquipToSlot(Equipment newItem)
    {
        Debug.Log("a");
        switch (newItem.equipSlot.ToString())
        {
            case "Helmet":
                Debug.Log("helmet");
                ChangeIcon(helmetSlot, newItem);
                SetItemToSlot(helmetSlotItem, newItem);
                SetVisuals(helmetVisuals,newItem,helmetAnimController);
                break;
            case "Chestplate":
                ChangeIcon(chestSlot, newItem);
                SetItemToSlot(chestSlotItem, newItem);
                SetVisuals(chestVisuals, newItem, chestAnimController);
                break;
            case "Leggins":
                ChangeIcon(legsSlot, newItem);
                SetItemToSlot(legsSlotItem, newItem);
                SetVisuals(legsVisuals, newItem, legsAnimController);
                break;
            case "Boots":
                ChangeIcon(bootsSlot, newItem);
                SetItemToSlot(bootsSlotItem, newItem);
                SetVisuals(bootsVisuals, newItem, bootsAnimController);
                break;
            case "Gloves":
                ChangeIcon(helmetSlot, newItem);
                break;
            case "LeftHandedWeapon":
                ChangeIcon(helmetSlot, newItem);
                break;
            case "RightHandedWeapon":
                ChangeIcon(helmetSlot, newItem);
                break;
            case "BothHandWeapon":
                ChangeIcon(helmetSlot, newItem);
                break;
            case "Potion":
                ChangeIcon(helmetSlot, newItem);
                //Potion
                break;
            case "Eatable":
                ChangeIcon(helmetSlot, newItem);
                break;
        }
    }

    public void UnquipToSlot(Equipment oldItem)
    {
        Debug.Log(oldItem);
        switch (oldItem.equipSlot.ToString())
        {
            case "Helmet":
                ResetIcon(helmetSlot);
                UnsetItemToSlot(helmetSlotItem);
                UnsetVisuals(helmetVisuals,oldItem,helmetAnimController);
                break;
            case "Chestplate":
                ResetIcon(chestSlot);
                UnsetItemToSlot(chestSlotItem);
                UnsetVisuals(chestVisuals, oldItem, chestAnimController);
                break;
            case "Leggins":
                ResetIcon(legsSlot);
                UnsetItemToSlot(legsSlotItem);
                UnsetVisuals(legsVisuals, oldItem, legsAnimController);
                break;
            case "Boots":
                ResetIcon(bootsSlot);
                UnsetItemToSlot(bootsSlotItem);
                UnsetVisuals(bootsVisuals, oldItem, bootsAnimController);
                break;
            case "Gloves":
                ResetIcon(chestSlot);
                break;
            case "LeftHandedWeapon":
                ResetIcon(chestSlot);
                break;
            case "RightHandedWeapon":
                ResetIcon(chestSlot);
                break;
            case "BothHandWeapon":
                ResetIcon(chestSlot);
                break;
            case "Potion":
                ResetIcon(chestSlot);
                //Potion
                break;
            case "Eatable":
                ResetIcon(chestSlot);
                break;
        }
    }

    private void ChangeIcon(CharacterEquipment slot, Item newItem)
    {
        slot.itemTypeHelp.gameObject.SetActive(false);
        slot.image.sprite = newItem.icon;
        slot.image.gameObject.SetActive(true);
    }

    public void ResetIcon(CharacterEquipment slot)
    {
        slot.image.gameObject.SetActive(false);
        slot.itemTypeHelp.gameObject.SetActive(true);
    }

    public void SetItemToSlot(ScrollViewEquipment slot, Item item)
    {
        slot.SetItem(item);
    }

    public void UnsetItemToSlot(ScrollViewEquipment slot)
    {
        slot.SetItem(null);
    }
    private void SetVisuals(Animator visuals,Equipment item, AnimatorController controller)
    {
        visuals.runtimeAnimatorController = item.animController;
        StartCoroutine(WaitAndPrint(0.3f));
        MenuManager.GetInstance().SetInvVis(visuals.runtimeAnimatorController, item.equipSlot.ToString());
        if(!ChestInvManager.GetInstance().GetisInventoryVisualsOn())
        {
            visuals.runtimeAnimatorController = controller;
        }
    }
    private void UnsetVisuals(Animator visuals, Equipment item, AnimatorController controller)
    {
        if (ChestInvManager.GetInstance().GetisInventoryVisualsOn())
            visuals.runtimeAnimatorController = controller;
        MenuManager.GetInstance().UnsetInvVis(visuals.runtimeAnimatorController, item.equipSlot.ToString());
    }
    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
