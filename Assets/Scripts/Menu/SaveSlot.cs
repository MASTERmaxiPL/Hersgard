using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{ 
    [SerializeField]private GameObject SaveName;
    [SerializeField]private GameObject SaveTime;

    private Button btn;

    private void Awake()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(LoadSaveSlot);
    }

    public void SetData(string name, string time)
    {
        SaveName.GetComponent<TextMeshProUGUI>().text = name;
        SaveTime.GetComponent<TextMeshProUGUI>().text = time;
    }

    private void LoadSaveSlot()
    {
        btn.onClick.RemoveListener(LoadSaveSlot);
        Debug.Log("LoadSaveSlot");
        DataManager.GetInstance().setDataHandler(this.SaveName.GetComponent<TextMeshProUGUI>().text);
        MainMenu.GetInstance().Continue();
    }
}
