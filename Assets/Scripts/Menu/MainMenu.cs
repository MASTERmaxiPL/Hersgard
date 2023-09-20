using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    #region singleton

    private static MainMenu instance;
    private void Awake()
    {
        instance = this;
        if(PlayerPrefs.GetInt("saveIndex") <= -1)
        {
            Debug.Log("test");
            PlayerPrefs.SetInt("saveIndex", -1);
        }
    }
    public static MainMenu GetInstance()
    {
        return instance;
    }

    #endregion
    #region variables
    //Main menu buttons
    [SerializeField]
    private Button cont;
    [SerializeField]
    private Button new_Game;
    [SerializeField]
    private Button load_Save;
    [SerializeField]
    private Button settings;
    [SerializeField]
    private Button credits;
    [SerializeField]
    private Button exit;

    //Credits and Settings Back button
    [SerializeField]
    private Button creditsBack;
    [SerializeField]
    private Button settingsBack;

    //Main Panels
    [SerializeField]
    private GameObject mainMenuPanel;
    [SerializeField]
    private GameObject loadSavePanel;
    [SerializeField]
    private GameObject creditsPanel;
    [SerializeField] 
    private GameObject settingsPanel;

    //Load Save Panel button
    [SerializeField]
    private Button savePanelBackBtn;

    //Main Settings buttons
    [SerializeField]
    private Button soundSettingsButton;
    [SerializeField]
    private Button keybindsSettingsButton;

    [SerializeField]
    private Button backKeybindsSettings;
    [SerializeField]
    private Button backSoundSettings;

    //Settings Panels
    [SerializeField]
    private GameObject mainSettings;
    [SerializeField]
    private GameObject soundSettings;
    [SerializeField]
    private GameObject keybindsSettings;
    //Settings title
    [SerializeField]
    private TextMeshProUGUI title;

    //Select Buttons
    [SerializeField]
    private GameObject menuFirstButton, settingsFirstButton, creditsFirstButton;
    [SerializeField]
    private GameObject volumeFirstButton, keybindsFirstButton;
    private GameObject lastButtonInMainMenu, lastButtonInPreviousSection;

    [SerializeField] private GameObject SaveDataContent;
    [SerializeField] private GameObject SaveDataSlot;
    [SerializeField] private Scrollbar Scrollbar;

    [SerializeField]private TextMeshProUGUI SaveLimitText;

    #endregion
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (!DataManager.GetInstance().CheckIfDataSaveExists())
        {
            cont.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(new_Game.gameObject);

            SetNavs();

            load_Save.gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            cont.onClick.AddListener(Continue);
            EventSystem.current.SetSelectedGameObject(cont.gameObject);
        }
    }

    private void SetNavs()
    {
        Navigation nav = new Navigation();
        nav.mode = Navigation.Mode.Explicit;
        nav.selectOnUp = exit;
        nav.selectOnDown = settings;
        new_Game.navigation = nav;

        Navigation nav2 = new Navigation();
        nav2.mode = Navigation.Mode.Explicit;
        nav2.selectOnUp = new_Game;
        nav2.selectOnDown = credits;
        settings.navigation = nav2;

        Navigation nav3 = new Navigation();
        nav3.mode = Navigation.Mode.Explicit;
        nav3.selectOnUp = credits;
        nav3.selectOnDown = new_Game;
        exit.navigation = nav3;
    }

    private void Start()
    {
        new_Game.onClick.AddListener(NewGame);
        load_Save.onClick.AddListener(LoadSaves);
        settings.onClick.AddListener(GoToSettings);
        credits.onClick.AddListener(GoToCredits);
        exit.onClick.AddListener(ExitGame);

        savePanelBackBtn.onClick.AddListener(CloseSavePanel);

        creditsBack.onClick.AddListener(CloseCredits);
        settingsBack.onClick.AddListener(CloseSettings);
        title.text = "SETTINGS";

        soundSettingsButton.onClick.AddListener(GoToSoundSettings);
        keybindsSettingsButton.onClick.AddListener(GoToKeybindsSettings);

        backSoundSettings.onClick.AddListener(GoBackFromSoundSett);
        backKeybindsSettings.onClick.AddListener(GoBackFromKeySett);

        loadSavePanel.SetActive(false);
        Cursor.visible = false;
        InputSystem.DisableDevice(Mouse.current);
    }

    public void Continue()
    {
        DataManager.GetInstance().OnSceneLoaded();
        RemoveEventListeners();
        Debug.Log("Load Save & play game");
        SceneManager.LoadSceneAsync("Game");
    }
    private void NewGame()
    {
        RemoveEventListeners();
        InputSystem.EnableDevice(Mouse.current);
        Cursor.visible = true;
        SceneManager.LoadSceneAsync("CharacterCreator");
    }

     
    private void LoadSaves()
    {
        loadSavePanel.SetActive(true);
        InputSystem.EnableDevice(Mouse.current);
        Cursor.visible = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(savePanelBackBtn.gameObject);

        if (SaveDataContent.transform.GetComponentInChildren<SaveSlot>() == null)
        {
            FileInfo[] files = DataManager.GetInstance().LoadExistingSaves();
            foreach (FileInfo file in files)
            {
                string[] text = file.Name.Split('.');
                string fileName = text[0];
                string fileTime = file.LastWriteTime.ToString();

                SaveDataSlot.GetComponent<SaveSlot>().SetData(fileName, fileTime);
                GameObject SaveSlot = Instantiate(SaveDataSlot, SaveDataContent.transform);

                Debug.Log(file.Name);
            }
        }

        Scrollbar.value = 1;
        Scrollbar.size = 0.3f;
    }
    private void CloseSavePanel()
    {
        loadSavePanel.SetActive(false);
        InputSystem.DisableDevice(Mouse.current);
        Cursor.visible = false;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(load_Save.gameObject);
    }

    private void GoToSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
        lastButtonInMainMenu = settings.gameObject;
    }
    private void GoToCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsFirstButton);
        lastButtonInMainMenu = credits.gameObject;
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    private void CloseCredits()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(lastButtonInMainMenu);
    }

    private void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(lastButtonInMainMenu);
    }
    private void GoBackFromSoundSett()
    {
        title.text = "SETTINGS";
        settingsPanel.GetComponent<Animator>().Play("SlideBackFromSoundSettings");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(lastButtonInPreviousSection);
    }
    private void GoBackFromKeySett()
    {
        title.text = "SETTINGS";
        settingsPanel.GetComponent<Animator>().Play("SlideBackFromKeybindSettings");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(lastButtonInPreviousSection);
    }


    private void GoToSoundSettings()
    {
        title.text = "SOUNDS";
        settingsPanel.GetComponent<Animator>().Play("SlideSoundSettings");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(volumeFirstButton);
        lastButtonInPreviousSection = soundSettingsButton.gameObject;
    }
    private void GoToKeybindsSettings()
    {
        title.text = "KEYBINDS";
        settingsPanel.GetComponent<Animator>().Play("SlideKeybindsSettings");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(keybindsFirstButton);
        lastButtonInPreviousSection = keybindsSettingsButton.gameObject;
    }

    private void RemoveEventListeners()
    {
        cont.onClick.RemoveListener(Continue);
        new_Game.onClick.RemoveListener(NewGame);
        load_Save.onClick.RemoveListener(LoadSaves);
        settings.onClick.RemoveListener(GoToSettings);
        credits.onClick.RemoveListener(GoToCredits);
        exit.onClick.RemoveListener(ExitGame);
        savePanelBackBtn.onClick.RemoveListener(CloseSavePanel);
        creditsBack.onClick.RemoveListener(CloseCredits);
        settingsBack.onClick.RemoveListener(CloseSettings);
        soundSettingsButton.onClick.RemoveListener(GoToSoundSettings);
        keybindsSettingsButton.onClick.RemoveListener(GoToKeybindsSettings);
        backKeybindsSettings.onClick.RemoveListener(GoBackFromKeySett);
        backSoundSettings.onClick.RemoveListener(GoBackFromSoundSett);
    }

    public void SetSaveSlotLimit()
    {
        int num; 
        int.TryParse(SaveLimitText.text, out num);
        DataManager.GetInstance().SetSaveLimit(num);
    }
}
