using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    [Header("Menu UI")]
    [SerializeField] private GameObject rightInventoryPage;
    [SerializeField] private GameObject leftInventoryPage;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private GameObject questsPanel;
    [SerializeField] private GameObject skillsPanel;
    [SerializeField] private GameObject bestiaryPanel;
    [SerializeField] private GameObject mainBackgroundPanel;

    [SerializeField] private Camera MapCamera;

    [SerializeField] private Button settings;
    [SerializeField] private Button goToMainMenu;
    [SerializeField] private Button exitGame;
    [SerializeField] private Button back;

    //ConfirmPanel - Exiting game
    [SerializeField] private GameObject ConfirmPanel;
    [SerializeField] private GameObject YesBtn;
    [SerializeField] private GameObject NoBtn;

    private bool GoToMenuClicked = false;

    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button settingsBack;

    //ButtonsPanel
    [SerializeField]
    private GameObject InvBtn, SkillsBtn, QuestBtn, MapBtn, SomeBtn, BestiaryBtn;
    [SerializeField] private GameObject Buttons;

    [SerializeField]
    private GameObject menuFirstButton, settingsFirstButton;
    [SerializeField]
    private GameObject volumeFirstButton, keybindsFirstButton;
    private GameObject lastButtonInPreviousSection;

    //Main Settings buttons
    [SerializeField] private Button soundSettingsButton;
    [SerializeField] private Button keybindsSettingsButton;

    [SerializeField] private Button backKeybindsSettings;
    [SerializeField] private Button backSoundSettings;

    //Settings title
    [SerializeField] private TextMeshProUGUI title;

    public bool menuIsOpened { get; private set; } = false;
    public bool confirmIsOpened { get; private set; } = false;
    public bool mapIsOpened { get; private set; } = false;
    public bool inventoryIsOpened { get; private set; } = false;
    public bool questsIsOpened { get; private set; } = false;
    public bool skillsIsOpened { get; private set; } = false;
    public bool bestiaryIsOpened { get; private set; } = false;
    public bool listIsOpened { get; private set; } = false;
    public bool gamepadIsWorking { get; set; } = false;

    [SerializeField] private GameObject helemtVisuals;
    [SerializeField] private GameObject chestVisuals;
    [SerializeField] private GameObject legsVisuals;
    [SerializeField] private GameObject bootsVisuals;

    private int num;

    public bool isDropMenuOpened = false;

    private static MenuManager instance;
    public static MenuManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Menu Manager in the scene");
        }
        instance = this;
    }

    private void Start()
    {
        InputSystem.EnableDevice(Mouse.current);
        menuPanel.SetActive(false);
        ConfirmPanel.SetActive(false);

        questsPanel.SetActive(false);
        skillsPanel.SetActive(false);
        bestiaryPanel.SetActive(false);
        mainBackgroundPanel.SetActive(false);
        mapPanel.SetActive(false);
        MapCamera.enabled = false;

        cursor.SetActive(false);
        Cursor.visible = false;

        Buttons.GetComponent<Animator>().Play("IdleBtns");

        InvBtn.GetComponent<Button>().onClick.AddListener(OpenInventoryByBtn);
        SkillsBtn.GetComponent<Button>().onClick.AddListener(OpenSkillsByBtn);
        QuestBtn.GetComponent<Button>().onClick.AddListener(OpenQuestsByBtn);
        MapBtn.GetComponent<Button>().onClick.AddListener(OpenMapByBtn);
        SomeBtn.GetComponent<Button>().onClick.AddListener(OpenMapByBtn);
        BestiaryBtn.GetComponent<Button>().onClick.AddListener(OpenBestiaryByBtn);
    }
    private void SetBtnsListeners()
    {
        Buttons.GetComponent<Animator>().Play("ShowBtns");
        mainBackgroundPanel.SetActive(true);
    }
    private void UnsetBtnsListeners()
    {
        Buttons.GetComponent<Animator>().Play("IdleBtns");
        mainBackgroundPanel.SetActive(false);
    }

    public void CloseInventoryPages()
    {
        rightInventoryPage.SetActive(false);
        leftInventoryPage.SetActive(false);
    }
    public void OpenInventory()
    {
        if (!menuIsOpened)
        {
            if (mapIsOpened)
                CloseMap();
            if (inventoryIsOpened)
            {
                CloseInventory();
                UnsetBtnsListeners();
                return;
            }
            if (questsIsOpened)
                CloseQuests();
            if (skillsIsOpened)
                CloseSkills();
            if (bestiaryIsOpened)
                CloseBestiary();
            SetBtnsListeners();
            inventoryIsOpened = true;
            DropItemMenu.GetInstance().ActiveDropItem();
            StartCoroutine(EquipmentManager.instance.WaitAndPrint(0.1f));
            EquipmentManager.instance.SetIsHelmetOn();
            leftInventoryPage.SetActive(true);
            rightInventoryPage.SetActive(true);
            if (gamepadIsWorking)
            {
                cursor.SetActive(true);
            }
            else
            {
                Cursor.visible = true;
            }
        }
    }
    private void OpenInventoryByBtn() { if (!inventoryIsOpened) OpenInventory(); }
    public void CloseInventory()
    {
        DropItemMenu.GetInstance().UnactiveDropItem();
        inventoryIsOpened = false;
        leftInventoryPage.SetActive(false);
        rightInventoryPage.SetActive(false);
        cursor.SetActive(false);
        Cursor.visible = false;
    }

    public void OpenMenu()
    {
        UnsetBtnsListeners();
        if (inventoryIsOpened)
        {
            CloseInventory();
            return;
        }
        if (mapIsOpened)
        {
            CloseMap();
            return;
        }
        if (questsIsOpened)
        {
            CloseQuests();
            return;
        }
        if (skillsIsOpened)
        {
            CloseSkills();
            return;
        }
        if (bestiaryIsOpened)
        {
            CloseBestiary();
            return;
        }
        if (menuIsOpened)
        {
            CloseMenu();
            return;
        }

        InputSystem.DisableDevice(Mouse.current);
        menuIsOpened = true;
        menuPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirstButton);

        settings.onClick.AddListener(OpenSettings);
        goToMainMenu.onClick.AddListener(GoToMainMenu);
        exitGame.onClick.AddListener(ExitGame);
        back.onClick.AddListener(Back);

        soundSettingsButton.onClick.AddListener(GoToSoundSettings);
        keybindsSettingsButton.onClick.AddListener(GoToKeybindsSettings);

        backSoundSettings.onClick.AddListener(GoBackFromSoundSett);
        backKeybindsSettings.onClick.AddListener(GoBackFromKeySett);

        settingsBack.onClick.AddListener(CloseSettings);
    }
    public void CloseMenu()
    {
        InputSystem.EnableDevice(Mouse.current);
        menuIsOpened = false;
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);

        settings.onClick.RemoveListener(OpenSettings);
        goToMainMenu.onClick.RemoveListener(GoToMainMenu);
        exitGame.onClick.RemoveListener(ExitGame);
        back.onClick.RemoveListener(Back);

        soundSettingsButton.onClick.RemoveListener(GoToSoundSettings);
        keybindsSettingsButton.onClick.RemoveListener(GoToKeybindsSettings);

        backSoundSettings.onClick.RemoveListener(GoBackFromSoundSett);
        backKeybindsSettings.onClick.RemoveListener(GoBackFromKeySett);

        settingsBack.onClick.RemoveListener(CloseSettings);


    }
    public void OpenMap()
    {
        SetBtnsListeners();
        MapCamera.enabled = true;
        if (!menuIsOpened)
        {
            if (mapIsOpened)
            {
                CloseMap();
                UnsetBtnsListeners();
                return;
            }
            if (inventoryIsOpened)
                CloseInventory();
            if (questsIsOpened)
                CloseQuests();
            if (skillsIsOpened)
                CloseSkills();
            if (bestiaryIsOpened)
                CloseBestiary();
            CameraMapManager.GetInstance().TurnOnMap();
            mapIsOpened = true;
            mapPanel.SetActive(true);
            if (gamepadIsWorking)
            {
                Cursor.lockState = CursorLockMode.Locked;
                cursor.transform.localPosition = Vector3.zero;
                cursor.SetActive(true);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    private void OpenMapByBtn() { if (!mapIsOpened) OpenMap(); }
    public void CloseMap()
    {
        Cursor.lockState = CursorLockMode.None;
        MapCamera.enabled = false;
        Cursor.visible = false;
        cursor.SetActive(false);
        mapIsOpened = false;
        mapPanel.SetActive(false);
        CameraMapManager.GetInstance().TurnOffMap();
    }
    private void OpenSettings()
    {
        settingsPanel.SetActive(true);
        settingsPanel.GetComponent<Animator>().Play("SlideSettingsPanel");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }
    private void GoToMainMenu()
    {
        confirmIsOpened = true;

        ConfirmPanel.SetActive(true);

        YesBtn.GetComponent<Button>().onClick.AddListener(YesBtnConfirm);
        NoBtn.GetComponent<Button>().onClick.AddListener(NoBtnConfirm);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(NoBtn);

        GoToMenuClicked = true;
    }
    private void ExitGame()
    {
        confirmIsOpened = true;

        ConfirmPanel.SetActive(true);

        YesBtn.GetComponent<Button>().onClick.AddListener(YesBtnConfirm);
        NoBtn.GetComponent<Button>().onClick.AddListener(NoBtnConfirm);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(NoBtn);

        GoToMenuClicked = false;
    }
    private void YesBtnConfirm()
    {
        confirmIsOpened = false;

        YesBtn.GetComponent<Button>().onClick.RemoveListener(YesBtnConfirm);
        NoBtn.GetComponent<Button>().onClick.RemoveListener(NoBtnConfirm);

        ConfirmPanel.SetActive(false);

        if (GoToMenuClicked)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            Application.Quit();
        }
    }
    private void NoBtnConfirm()
    {
        confirmIsOpened = false;

        YesBtn.GetComponent<Button>().onClick.RemoveListener(YesBtnConfirm);
        NoBtn.GetComponent<Button>().onClick.RemoveListener(NoBtnConfirm);

        ConfirmPanel.SetActive(false);
        if (GoToMenuClicked)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(goToMainMenu.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(exitGame.gameObject);
        }
    }
    public void CloseWithoutConfirm()
    {
        confirmIsOpened = false;
        ConfirmPanel.SetActive(false);

        YesBtn.GetComponent<Button>().onClick.RemoveListener(YesBtnConfirm);
        NoBtn.GetComponent<Button>().onClick.RemoveListener(NoBtnConfirm);

        if (GoToMenuClicked)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(goToMainMenu.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(exitGame.gameObject);
        }
    }


    private void Back()
    {
        CloseMenu();
        cursor.SetActive(false);
        Cursor.visible = false;
    }

    private void GoToSoundSettings()
    {
        title.text = "SOUNDS";
        settingsPanel.GetComponent<Animator>().Play("PauseSettingsShowSound");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(volumeFirstButton);
        lastButtonInPreviousSection = soundSettingsButton.gameObject;
    }
    private void GoToKeybindsSettings()
    {
        title.text = "KEYBINDS";
        settingsPanel.GetComponent<Animator>().Play("PauseSettingsShowKey");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(keybindsFirstButton);
        lastButtonInPreviousSection = keybindsSettingsButton.gameObject;
    }
    private void GoBackFromSoundSett()
    {
        title.text = "SETTINGS";
        settingsPanel.GetComponent<Animator>().Play("PauseSettingsHideSounds");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(lastButtonInPreviousSection);
    }
    private void GoBackFromKeySett()
    {
        title.text = "SETTINGS";
        settingsPanel.GetComponent<Animator>().Play("PauseSettingsHideKey");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(lastButtonInPreviousSection);
    }
    private void CloseSettings()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirstButton);

        settingsPanel.GetComponent<Animator>().Play("PauseSettingsHide");
    }

    public void OpenQuests()
    {
        if (!menuIsOpened)
        {
            if (mapIsOpened)
                CloseMap();
            if (inventoryIsOpened)
                CloseInventory();
            if (questsIsOpened)
            {
                CloseQuests();
                UnsetBtnsListeners();
                return;
            }
            if (skillsIsOpened)
                CloseSkills();
            if (bestiaryIsOpened)
                CloseBestiary();
            SetBtnsListeners();
            questsIsOpened = true;
            questsPanel.SetActive(true);
            if (gamepadIsWorking)
            {
                cursor.SetActive(true);
            }
            else
            {
                Cursor.visible = true;
            }
        }
    }
    private void OpenQuestsByBtn() { if (!questsIsOpened) OpenQuests(); }
    public void CloseQuests()
    {
        questsIsOpened = false;
        questsPanel.SetActive(false);
        cursor.SetActive(false);
        Cursor.visible = false;
    }

    public void OpenSkills()
    {
        if (!menuIsOpened)
        {
            if (mapIsOpened)
                CloseMap();
            if (inventoryIsOpened)
                CloseInventory();
            if (questsIsOpened)
                CloseQuests();
            if (skillsIsOpened)
            {
                CloseSkills();
                UnsetBtnsListeners();
                return;
            }
            if (bestiaryIsOpened)
                CloseBestiary();
            SetBtnsListeners();
            skillsIsOpened = true;
            skillsPanel.SetActive(true);
            if (gamepadIsWorking)
            {
                cursor.SetActive(true);
            }
            else
            {
                Cursor.visible = true;
            }
        }
    }
    private void OpenSkillsByBtn() { if (!skillsIsOpened) OpenSkills(); }
    public void CloseSkills()
    {
        skillsIsOpened = false;
        skillsPanel.SetActive(false);
        cursor.SetActive(false);
        Cursor.visible = false;
    }

    public void OpenBestiary()
    {
        if (!menuIsOpened)
        {
            if (mapIsOpened)
                CloseMap();
            if (inventoryIsOpened)
                CloseInventory();
            if (questsIsOpened)
                CloseQuests();
            if (skillsIsOpened)
                CloseSkills();
            if (bestiaryIsOpened)
            {
                CloseBestiary();
                UnsetBtnsListeners();
                return;
            }
            SetBtnsListeners();
            bestiaryIsOpened = true;
            bestiaryPanel.SetActive(true);
            if (gamepadIsWorking)
            {
                cursor.SetActive(true);
            }
            else
            {
                Cursor.visible = true;
            }
        }
    }
    private void OpenBestiaryByBtn() { if (!bestiaryIsOpened) OpenBestiary(); }
    public void CloseBestiary()
    {
        bestiaryIsOpened = false;
        bestiaryPanel.SetActive(false);
        cursor.SetActive(false);
        Cursor.visible = false;
    }
    public void SetInvVis(RuntimeAnimatorController animContrloler, string tag)
    {
        switch (tag)
        {
            case "Helmet":
                helemtVisuals.GetComponent<Animator>().runtimeAnimatorController = animContrloler;
                StartCoroutine(WaitAndPrint(0.3f, helemtVisuals));
                break;
            case "Chestplate":
                chestVisuals.GetComponent<Animator>().runtimeAnimatorController = animContrloler;
                StartCoroutine(WaitAndPrint(0.3f, chestVisuals));
                break;
            case "Leggins":
                legsVisuals.GetComponent<Animator>().runtimeAnimatorController = animContrloler;
                StartCoroutine(WaitAndPrint(0.3f, legsVisuals));
                break;
            case "Boots":
                bootsVisuals.GetComponent<Animator>().runtimeAnimatorController = animContrloler;
                StartCoroutine(WaitAndPrint(0.3f, bootsVisuals));
                break;
        }
    }
    public IEnumerator WaitAndPrint(float waitTime, GameObject obj)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            obj.GetComponent<Image>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
            yield return new WaitForSeconds(waitTime);
            var image = obj.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            Debug.Log(image.color);
            if (!EquipmentManager.instance.isHelmetOn)
            {
                EquipmentManager.instance.SetOffHelmetVisuals();
            }
            yield break;
        }
    }

    private Image image;

    public void UnsetInvVis(RuntimeAnimatorController animContrloler, string tag)
    {
        switch (tag)
        {
            case "Helmet":
                image = helemtVisuals.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
                helemtVisuals.GetComponent<Image>().sprite = null;
                break;
            case "Chestplate":
                image = chestVisuals.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
                chestVisuals.GetComponent<Image>().sprite = null;
                break;
            case "Leggins":
                image = legsVisuals.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
                legsVisuals.GetComponent<Image>().sprite = null;
                break;
            case "Boots":
                image = bootsVisuals.GetComponent<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
                bootsVisuals.GetComponent<Image>().sprite = null;
                break;
        }
    }
}
