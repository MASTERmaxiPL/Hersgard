using System;
using System.Collections;
using UnityEngine;

public class Player : Fighter, IData
{
    private PlayerControls playerControls;
    public float waitForInventorySpeed = 3f, waitForRollSpeed = 6f;
    public float speed = 5f;
    public Rigidbody2D rb;
    private static Animator[] anims;
    public Vector2 movement { get; private set; } = Vector2.zero;
    private Vector3 slideDir;
    Vector2 lastMoveDir;

    [SerializeField]
    private LayerMask rollLayerMask;

    private float rollSpeed;
    private float dodgeSpeed;
    public bool isRunning { get; private set; } = false;
    public bool choice { get; private set; } = false;
    public bool skipDialogue { get; private set; } = false;
    public bool deleteItem { get; private set; } = false;
    public bool selectItem { get; private set; } = false;
    public bool useItem { get; private set; } = false;
    public bool confirm { get; private set; } = false;
    public bool equip { get; private set; } = false;
    public bool doRoll { get; private set; } = false;
    public bool doDodge { get; private set; } = false;

    private GameObject closestObject;

    private State state;
    private enum State
    {
        Normal,
        Roll,
        Dodge
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerControls();
        state = State.Normal;
    }

    public void LoadData(GameData data)
    {
        transform.position = new Vector3(data.xPosition, data.yPosition, 0);
    }
    public void SaveData(GameData data)
    {
        if(this.gameObject != null)
        {
            data.xPosition = gameObject.transform.position.x;
            data.yPosition = gameObject.transform.position.y;
        }
        else
        {
            data.xPosition = 0;
            data.yPosition = 0;
        }

    }


    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    private static Player instance;

    public static Player GetInstance()
    {
        return instance;
    }


    private void Start()
    {
        instance = this;
        anims = PlayerBodyManager.GetAnims();

        playerControls.Movement.OpenInv.performed += _ => OpenInv();
        playerControls.Movement.OpemMenu.performed += _ => OpenMenu();
        playerControls.Movement.OpenMap.performed += _ => OpenMap();
        playerControls.Movement.OpenQuests.performed += _ => OpenQuests();
        playerControls.Movement.OpenSkills.performed += _ => OpenSkills();
        playerControls.Movement.OpenBestiary.performed += _ => OpenBestiary();

        playerControls.UI.ScrollUp.performed += _ => MapScrollUp();
        playerControls.UI.ScrollDown.performed += _ => MapScrollDown();

        playerControls.Movement.Interact.performed += _ => Interact();
        playerControls.Movement.Roll.performed += _ => Roll();
        playerControls.Movement.Roll.canceled += _ => StopRoll();
        playerControls.Movement.Dodge.performed += _ => Dodge();
        playerControls.Movement.Dodge.canceled += _ => StopDodge();
        playerControls.Movement.RunStart.performed += _ => RunStart();
        playerControls.Movement.RunStop.performed += _ => RunStop();

        playerControls.UI.Choose.performed += _ => MakeChoice();
        playerControls.UI.Choose.canceled += _ => CancelMakeChoice();
        playerControls.UI.SkipDialogue.performed += _ => SkipDialogue();
        playerControls.UI.SkipDialogue.canceled += _ => StopSkipDialogue();
        playerControls.UI.DeleteItem.performed += _ => DeleteItem();
        playerControls.UI.DeleteItem.canceled += _ => StopDeleteItem();
        playerControls.UI.Select.performed += _ => SelectItem();
        playerControls.UI.Select.canceled += _ => StopSelectItem();
        playerControls.UI.UseItem.performed += _ => UseItem();
        playerControls.UI.UseItem.canceled += _ => StopUseItem();
        playerControls.UI.Confirm.performed += _ => Confirm();
        playerControls.UI.Confirm.canceled += _ => StopConfirm();
        playerControls.UI.EquipItem.performed += _ => EquipItem();
        playerControls.UI.EquipItem.canceled += _ => StopEquipItem();
    }

    private void OpenInv()
    {
        if (MenuManager.GetInstance().isDropMenuOpened)
        {
            return;
        }
        if (DialogueManager.GetInstance().dialogueIsPlaying || ChestInvManager.GetInstance().isPlayerChestOpened)
            return;
        MenuManager.GetInstance().OpenInventory();
        movement = Vector2.zero;
        setFloatToAnims("Horizontal", 0);
        setFloatToAnims("Vertical", 0);
        StartCoroutine(StartCooldown(waitForInventorySpeed));
    }
    private void OpenQuests()
    {
        if (MenuManager.GetInstance().isDropMenuOpened)
        {
            return;
        }
        if (DialogueManager.GetInstance().dialogueIsPlaying || ChestInvManager.GetInstance().isPlayerChestOpened)
            return;
        MenuManager.GetInstance().OpenQuests();
        movement = Vector2.zero;
        setFloatToAnims("Horizontal", 0);
        setFloatToAnims("Vertical", 0);
        StartCoroutine(StartCooldown(waitForInventorySpeed));
    }
    private void OpenSkills()
    {
        if (MenuManager.GetInstance().isDropMenuOpened)
        {
            return;
        }
        if (DialogueManager.GetInstance().dialogueIsPlaying || ChestInvManager.GetInstance().isPlayerChestOpened)
            return;
        MenuManager.GetInstance().OpenSkills();
        movement = Vector2.zero;
        setFloatToAnims("Horizontal", 0);
        setFloatToAnims("Vertical", 0);
        StartCoroutine(StartCooldown(waitForInventorySpeed));
    }
    private void OpenBestiary()
    {
        if (MenuManager.GetInstance().isDropMenuOpened)
        {
            return;
        }
        if (DialogueManager.GetInstance().dialogueIsPlaying || ChestInvManager.GetInstance().isPlayerChestOpened)
            return;
        MenuManager.GetInstance().OpenBestiary();
        movement = Vector2.zero;
        setFloatToAnims("Horizontal", 0);
        setFloatToAnims("Vertical", 0);
        StartCoroutine(StartCooldown(waitForInventorySpeed));
    }
    private void OpenMap()
    {
        if (MenuManager.GetInstance().isDropMenuOpened)
        {
            return;
        }
        if (DialogueManager.GetInstance().dialogueIsPlaying || ChestInvManager.GetInstance().isPlayerChestOpened)
            return;
        MenuManager.GetInstance().OpenMap();
        movement = Vector2.zero;
        setFloatToAnims("Horizontal", 0);
        setFloatToAnims("Vertical", 0);
        StartCoroutine(StartCooldown(waitForInventorySpeed));
    }
    private void MapScrollUp()
    {
        if (MenuManager.GetInstance().mapIsOpened)
        {
            CameraMapManager.GetInstance().ScrollUp();
        }
    }
    private void MapScrollDown()
    {
        if (MenuManager.GetInstance().mapIsOpened)
        {
            CameraMapManager.GetInstance().ScrollDown();
        }
    }

    private void OpenMenu()
    {
        if (MenuManager.GetInstance().confirmIsOpened)
        {
            MenuManager.GetInstance().CloseWithoutConfirm();
            return;
        }
        if (MenuManager.GetInstance().isDropMenuOpened)
        {
            DropItemMenu.GetInstance().CloseDropMenu();
            return;
        }
        if (DialogueManager.GetInstance().dialogueIsPlaying)
            return;
        if (VisualsUiManager.GetInstance().GetMarked() != null)
        {
            VisualsUiManager.GetInstance().HideItemsToEquip();
            return;
        }
        if (ChestInvManager.GetInstance().isPlayerChestOpened)
        {
            ChestInvManager.GetInstance().CloseChest();
            return;
        }
        else
        {
            MenuManager.GetInstance().OpenMenu();
            movement = Vector2.zero;
            setFloatToAnims("Horizontal", 0);
            setFloatToAnims("Vertical", 0);
            StartCoroutine(StartCooldown(waitForInventorySpeed));
        }
    }

    private void Interact()
    {
        if (MenuManager.GetInstance().inventoryIsOpened || MenuManager.GetInstance().menuIsOpened)
        {
            return;
        }
        if (ChestInvManager.GetInstance().isPlayerChestOpened)
        {
            ChestInvManager.GetInstance().CloseChest();
            return;
        }
        closestObject = CheckInteractible.GetInstance().closestObject;
        if (closestObject == null)
        {
            return;
        }
        if(closestObject.transform.GetChild(0).GetComponent<ItemPickup>())
        {
            closestObject.transform.GetChild(0).GetComponent<ItemPickup>().Interact();
        }
        else if (closestObject.transform.GetChild(0).GetComponent<DialogueTrigger>())
        {
            DisableMovement();
            playerControls.Movement.Interact.Disable();
            closestObject.transform.GetChild(0).GetComponent<DialogueTrigger>().Interact();
        }
        else if(closestObject.transform.GetChild(0).GetComponent<PlayerChestTrigger>())
        {
            DisableMovement();
            closestObject.transform.GetChild(0).GetComponent<PlayerChestTrigger>().Interact();
            ChestInvManager.GetInstance().SetVisualsBtn();
        }
        else if (closestObject.transform.GetChild(0).GetComponent<CampfireTrigger>())
        {
            DisableMovement();
            playerControls.Movement.Interact.Disable();
            closestObject.transform.GetChild(0).GetComponent<CampfireTrigger>().Interact();
        }
    }

    public void EnableMovement()
    {
        playerControls.Movement.Move.Enable();
        playerControls.Movement.Interact.Enable();
        playerControls.Movement.Roll.Enable();
        playerControls.Movement.Dodge.Enable();
    }
    private void DisableMovement()
    {
        playerControls.Movement.Move.Disable();
        playerControls.Movement.Roll.Disable();
        playerControls.Movement.Dodge.Disable();
    }

    private void Roll()
    {
        doRoll = true;
    }
    private void StopRoll()
    {
        doRoll = false;
    }

    private void Dodge()
    {
            doDodge = true;
    }

    private void StopDodge()
    {
        doDodge = false;
        StartCoroutine(StartCooldown(40f));
    }

    private void RunStart()
    {
        speed = 7f;
        setFloatToAnims("isRunning", 1);
    }
    private void RunStop()
    {
        speed= 5.0f;
        setFloatToAnims("isRunning", 0);
    }


    private void MakeChoice()
    {
        choice = true;
    }
    private void CancelMakeChoice()
    {
        choice = false;
    }
    private void SkipDialogue()
    {
        skipDialogue= true;
    }
    public void StopSkipDialogue()
    {
        skipDialogue= false;
    }

    private void DeleteItem()
    {
        if (MenuManager.GetInstance().inventoryIsOpened)
        {
            deleteItem = true;
        }

    }
    private void StopDeleteItem()
    {
        deleteItem = false;
    }
    private void SelectItem()
    {
        if (MenuManager.GetInstance().inventoryIsOpened)
        {
            selectItem = true;
        }
        
    }
    private void StopSelectItem()
    {
        selectItem = false;
    }
    private void UseItem()
    {
        if (MenuManager.GetInstance().inventoryIsOpened)
        {
            useItem = true;
        }
        
    }
    private void StopUseItem()
    {
        useItem= false;
    }
    private void Confirm()
    {
        if (MenuManager.GetInstance().inventoryIsOpened || ChestInvManager.GetInstance().isPlayerChestOpened)
        {
            confirm = true;
            StartCooldown(10f);
        }
    }
    private void StopConfirm()
    {
        confirm = false;
    }
    private void EquipItem()
    {
        equip = true;
    }
    private void StopEquipItem()
    {
        equip = false;
    }

    private void HandleMovement()
    {
        if (MenuManager.GetInstance().menuIsOpened || MenuManager.GetInstance().inventoryIsOpened || MenuManager.GetInstance().mapIsOpened)
        {
            setFloatToAnims("isRunning", 0);
            return;
        }

        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        setFloatToAnims("Horizontal", movement.x);
        setFloatToAnims("Vertical", movement.y);
        setFloatToAnims("Speed", movement.sqrMagnitude);

        if (movement.x != 0 || movement.y != 0)
        {
            setFloatToAnims("lastMoveX", movement.x);
            setFloatToAnims("lastMoveY", movement.y);
            lastMoveDir = movement;
        }
    }
    
    private bool TryMove(Vector3 baseMoveDir, float distance)
    {
        Vector3 moveDir = baseMoveDir;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, moveDir, distance, rollLayerMask);
        CircleCollider2D checkInteractible = this.GetComponentInChildren<CircleCollider2D>();
        BoxCollider2D playerCollider = this.GetComponent<BoxCollider2D>();

        if(raycastHit2D.collider != null && raycastHit2D.collider != playerCollider && raycastHit2D.collider != checkInteractible)
        {
            moveDir = new Vector3(baseMoveDir.x, 0f).normalized;
            raycastHit2D = Physics2D.Raycast(transform.position, moveDir, distance, rollLayerMask);
            if (raycastHit2D.collider != null && raycastHit2D.collider != playerCollider && raycastHit2D.collider != checkInteractible)
            {
                moveDir = new Vector3(0f, baseMoveDir.y).normalized;
                raycastHit2D = Physics2D.Raycast(transform.position, moveDir, distance, rollLayerMask);
                if(raycastHit2D.collider != null && raycastHit2D.collider != playerCollider && raycastHit2D.collider != checkInteractible)
                {
                    moveDir = raycastHit2D.point;
                    Debug.Log(raycastHit2D.collider);
                }
            }
            return false;
        }
        else
        {
            lastMoveDir = moveDir;
            rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
            return true;
        }
    }


    private void HandleRollSliding()
    {
        if (TryMove(slideDir, rollSpeed * Time.deltaTime))
        {
            transform.position += slideDir * rollSpeed * Time.deltaTime;

            rollSpeed -= rollSpeed * 20f * Time.deltaTime;
            if (rollSpeed < 5f)
            {
                state = State.Normal;
            }
        }
        else
        {
            state = State.Normal;
        }

    }

    private void HandleDodgeSliding()
    {
        if(TryMove(slideDir, dodgeSpeed * Time.deltaTime))
        {
            transform.position -= slideDir * dodgeSpeed * Time.deltaTime;

            dodgeSpeed -= dodgeSpeed * 30f * Time.deltaTime;
            if (dodgeSpeed < 5f)
            {
                state = State.Normal;
            }
        }
        else
        {
            state = State.Normal;
        }
    }

    private void HandleRoll()
    {
        if (doRoll)
        {
            if (MenuManager.GetInstance().inventoryIsOpened || MenuManager.GetInstance().menuIsOpened || MenuManager.GetInstance().mapIsOpened)
            {
                return;
            }
            state = State.Roll;
            slideDir = lastMoveDir;
            rollSpeed = 120f;
            doRoll = false;

            // PLAY ANIM
        }
    }

    private void HandleDodge()
    {
        if (doDodge)
        {
            if (MenuManager.GetInstance().inventoryIsOpened || MenuManager.GetInstance().menuIsOpened || MenuManager.GetInstance().mapIsOpened)
            {
                return;
            }
            
            state = State.Dodge;
            slideDir = lastMoveDir;
            dodgeSpeed = 80f;
            
           
            //PLAY ANIM
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                HandleMovement();
                HandleRoll();
                HandleDodge();
                break;
            case State.Roll:
                HandleRollSliding();
                break;
            case State.Dodge:
                HandleDodgeSliding();
                break;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }

    private IEnumerator StartCooldown(float speed)
    {
        yield return new WaitForSeconds(speed);
    }

    private void setFloatToAnims(string keyword, float value)
    {
        if (anims != null)
        {
            foreach (Animator anim in anims)
            {
                if(anim!=null) 
                    anim.SetFloat(keyword, value);
            }
        }

    }
}