using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// Keep all gameplay bindings in one place while the menu still uses IMGUI.
static class RiftorynInput
{
#if ENABLE_INPUT_SYSTEM
    static Keyboard Keys => Keyboard.current;
    static Mouse Pointer => Mouse.current;
    public static bool PointerPressed => Pointer != null && Pointer.leftButton.wasPressedThisFrame;
    public static Vector2 PointerPosition => Pointer != null ? Pointer.position.ReadValue() : Vector2.zero;
    // The legacy Mouse X/Y axes used sensitivity 0.1.
    public static Vector2 Look => Pointer != null ? Pointer.delta.ReadValue() * .1f : Vector2.zero;
    public static bool Escape => Keys != null && Keys.escapeKey.wasPressedThisFrame;
    public static bool Ability => Keys != null && Keys.qKey.wasPressedThisFrame;
    public static bool Roll => Keys != null && Keys.spaceKey.wasPressedThisFrame;
    public static bool Attack => Keys != null && Keys.eKey.wasPressedThisFrame;
    public static Vector2 Movement
    {
        get
        {
            Vector2 value=Vector2.zero;
            var keyboard=Keys;
            if(keyboard!=null)
            {
                if(keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) value.x++;
                if(keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) value.x--;
                if(keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) value.y++;
                if(keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) value.y--;
            }
            var gamepad=Gamepad.current;
            if(gamepad!=null) value+=gamepad.leftStick.ReadValue();
            return Vector2.ClampMagnitude(value,1f);
        }
    }
#else
    public static bool PointerPressed => Input.GetMouseButtonDown(0);
    public static Vector2 PointerPosition => Input.mousePosition;
    public static Vector2 Look => new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
    public static bool Escape => Input.GetKeyDown(KeyCode.Escape);
    public static bool Ability => Input.GetKeyDown(KeyCode.Q);
    public static bool Roll => Input.GetKeyDown(KeyCode.Space);
    public static bool Attack => Input.GetKeyDown(KeyCode.E);
    public static Vector2 Movement => new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
#endif
}
