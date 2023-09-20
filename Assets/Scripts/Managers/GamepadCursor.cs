using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamepadCursor : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private RectTransform cursorTransform;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject cursor;
    [SerializeField]
    private RectTransform canvasTransform;
    [SerializeField]
    private float cursorSpeed = 1000f;
    [SerializeField]
    private float padding = 35f;

    private Mouse virtualMouse;
    private bool prevMouseState;

    private Mouse currMouse;

    private string prevControlScheme = "";
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Mouse&Keyboard";


    private void OnEnable()
    {
        currMouse = Mouse.current;

        if(virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if(cursorTransform != null)
        {
            Vector2 position = cursorTransform.position;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        if(virtualMouse != null && virtualMouse.added) 
        {
            InputSystem.RemoveDevice(virtualMouse); 
        }
        
        InputSystem.onAfterUpdate -= UpdateMotion;
        playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null)
            return;
        if (!MenuManager.GetInstance().gamepadIsWorking)
            return;

        Vector2 stickValue = Gamepad.current.leftStick.ReadValue();
        stickValue *= cursorSpeed * Time.deltaTime;

        Vector2 currPos = virtualMouse.position.ReadValue();
        Vector2 newPos = currPos + stickValue;

        newPos.x = Mathf.Clamp(newPos.x, padding, Screen.width - padding);
        newPos.y = Mathf.Clamp(newPos.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPos);
        InputState.Change(virtualMouse.delta, stickValue);

        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();
        if (prevMouseState != aButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            prevMouseState = aButtonIsPressed;
        }

        AnchorCursor(newPos);
    }

    private void AnchorCursor(Vector2 newPos)
    {
        if (!MenuManager.GetInstance().mapIsOpened)
        {
            Vector2 anchoredPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, newPos, null, out anchoredPosition);
            cursorTransform.anchoredPosition = anchoredPosition;
        }
    }
    
    private void OnControlsChanged(PlayerInput input)
    {
        if (playerInput.currentControlScheme == mouseScheme)
        {
            if (MenuManager.GetInstance().inventoryIsOpened)
            {
                Cursor.visible = true;
                currMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
                cursorTransform.gameObject.SetActive(false);
                prevControlScheme = mouseScheme;
                MenuManager.GetInstance().gamepadIsWorking = false;
            }
        }
        else if(playerInput.currentControlScheme == gamepadScheme)
        {
            if (MenuManager.GetInstance().inventoryIsOpened)
            {
                cursor.SetActive(true);
                Cursor.visible = false;
                InputState.Change(virtualMouse.position, currMouse.position.ReadValue());
                AnchorCursor(currMouse.position.ReadValue());
            }
            prevControlScheme = gamepadScheme;
            MenuManager.GetInstance().gamepadIsWorking = true;
        }
    }
    private void Update()
    {
        if(prevControlScheme != playerInput.currentControlScheme)
        {
            OnControlsChanged(playerInput);
        }
        prevControlScheme= playerInput.currentControlScheme;
    }
}
