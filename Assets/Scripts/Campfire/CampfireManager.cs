using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireManager : MonoBehaviour
{
    #region instance 
    private static CampfireManager instance;
    private void Awake()
    {
        instance = this;
    }
    public static CampfireManager GetInstance(){
        return instance;
    }
    #endregion

    [SerializeField] private TextAsset inkJSON;
    public void ShowCampfireOptions()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }
}
