using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ChestInvManager : MonoBehaviour, IData
{
    [SerializeField] private GameObject playerChestPanel;
    [SerializeField] private GameObject playerInv;
    public bool stackable = false;
    public bool isPlayerChestOpened = false;
    private bool isInventoryVisualsOn = true;
    [SerializeField]
    private Animator isInventoryVisualsOnBtn;
    [SerializeField]
    public AnimatorOverrideController NoVisualsAnim;
    [SerializeField]
    public Equipment[] ChestEquipment;

    [SerializeField]
    private GameObject[] InChestVisuals;
    [SerializeField]
    private Sprite noVisualSprite;


    public List<Item> items = new List<Item>();

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    #region singleton
    private static ChestInvManager instance;
    public static ChestInvManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;

        ChestEquipment = new Equipment[4];
    }
    private void Start()
    {
        playerChestPanel.SetActive(false);
    }
    public void LoadData(GameData data)
    {
        isInventoryVisualsOn = !data.isInventoryVisualOn;

        foreach (Item item in data.chestItems.ToList())
        {
            AddItemToPlayerChest(item);
        }
        foreach(Equipment item in data.chestEquipment.ToList())
        {
            if (item != null)
            {
                VisualsUiManager.GetInstance().EquipToSlot(item);
            }
        }
        ChangeVisuals();
    }
    public void SaveData(GameData data)
    {
        data.chestItems = this.items;
        data.chestEquipment = this.ChestEquipment.ToList();
        data.isInventoryVisualOn = isInventoryVisualsOn;
    }

    #endregion

    public void OpenChest()
    {
        isPlayerChestOpened = true;
        Cursor.visible= true;
        StartCoroutine(WaitAndPrint(0.05f));
        playerChestPanel.SetActive(true);
        playerInv.SetActive(true);
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        VisualsUiManager.GetInstance().SetVisuals();
        yield break;
    }


    public void CloseChest()
    {
        playerChestPanel.SetActive(false);
        playerInv.SetActive(false);
        isPlayerChestOpened = false;
        Cursor.visible = false;
        Player.GetInstance().EnableMovement();
    }

    public void AddItemToPlayerChest(Item item)
    {
        bool check = false;
        if (item.canStack)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].name == item.name)
                {
                    stackable = true;
                    if (onItemChangedCallback != null)
                        onItemChangedCallback.Invoke();
                    stackable = false;
                    check = true;                 
                }
            }
        }
        if (!check)
        {
            items.Add(item);
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }
    }

    public void Remove(ScrollViewItem item)
    {
        //items.Remove(item);
        items.Remove(item.item);
    }

    public void ChangeVisuals()
    {
        if (isInventoryVisualsOn)
        {
            isInventoryVisualsOn = false;
            for (int i = 0; i <= 3; i++)
            {
                if (ChestEquipment[i] != null)
                {
                    if (i == 0)
                    {
                        if (EquipmentManager.instance.isHelmetOn)
                        {
                            PlayerBodyManager.GetInstance().UpdatePlayerVisuals(i+5, ChestEquipment[i].animController);
                            InChestVisuals[i].GetComponent<Image>().sprite = InChestVisuals[i].GetComponent<SpriteRenderer>().sprite;
                        }
                    }
                    else
                    {
                        PlayerBodyManager.GetInstance().UpdatePlayerVisuals(i+5, ChestEquipment[i].animController);
                        InChestVisuals[i].GetComponent<Image>().sprite = InChestVisuals[i].GetComponent<SpriteRenderer>().sprite;
                    }
                }
                else
                {
                    PlayerBodyManager.GetInstance().UpdatePlayerVisuals(i + 5, NoVisualsAnim);
                    InChestVisuals[i].GetComponent<Image>().sprite = noVisualSprite;
                }
            }
        }
        else
        {
            isInventoryVisualsOn = true;
            for(int i = 0; i < 4; i++)
            {
                if (EquipmentManager.instance.currEquipment[i] != null)
                {
                    if (i == 0)
                    {
                        if (EquipmentManager.instance.isHelmetOn)
                        {
                            PlayerBodyManager.GetInstance().UpdatePlayerVisuals(i+5, EquipmentManager.instance.currEquipment[i].animController);
                        }
                    }
                    else
                    {
                        PlayerBodyManager.GetInstance().UpdatePlayerVisuals(i+5, EquipmentManager.instance.currEquipment[i].animController);
                    }
                }
                else
                {
                    PlayerBodyManager.GetInstance().UpdatePlayerVisuals(i + 5, NoVisualsAnim); 
                }
            }
        }
        SetVisualsBtn();
    }
    public bool GetisInventoryVisualsOn()
    {
        return isInventoryVisualsOn;
    }
    public void SetVisualsBtn()
    {
        isInventoryVisualsOnBtn.SetBool("IsInventoryVisualsOn", isInventoryVisualsOn);
    }
}
