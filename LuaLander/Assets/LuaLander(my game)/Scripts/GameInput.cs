using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { private set; get; }

    public event EventHandler OnMenuButtonPressed;

    private InputActions inputActions;

    private void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        // inputActions.Player.LanderLeft.performed //fires once the moment the button is pressed.
        // inputActions.Player.LanderLeft.IsPressed() // returns true every frame the button is held down

        // public class InputActions : IInputActionCollection2, IDisposable
        // No MonoBehaviour in the inheritance chain → no Unity lifecycle methods like Awake, Start, Update.
        // MonoBehaviours can't be instantiated with new - new InputActions() - normal C# class
        inputActions.Enable();

        inputActions.Player.Menu.performed += Menu_performed;
    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMenuButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsUpActionPressed()
    {
        return inputActions.Player.LanderUp.IsPressed();
    }
    public bool IsLeftActionPressed()
    {
        return inputActions.Player.LanderLeft.IsPressed();
    }
    public bool IsRightActionPressed()
    {
        return inputActions.Player.LanderRight.IsPressed();
    }

    // THIS IS NOT CORRECT WAY. MAKE EVENTHANDLER MENU_PERFORMED INSTEAD
    // public bool IsPauseActionPressed()
    // {
    //     // return inputActions.Player.Pause.IsPressed();
    //     return inputActions.Player.Menu.WasPerformedThisFrame();
    // }

    private void OnDestroy()
    {
        inputActions.Disable();
    }
}


// How IEnumerable works. It is inside InputActions maybe...
// public class Inventory : IEnumerable<string>
// {
//     private string[] items = { "sword", "shield", "potion" };

//     public IEnumerator<string> GetEnumerator()
//     {
//         return ((IEnumerable<string>)items).GetEnumerator();
//     }

//     IEnumerator IEnumerable.GetEnumerator() // the non-generic version (boilerplate)
//     {
//         return GetEnumerator();
//     }
// }

// // Now foreach works:
// var inventory = new Inventory();
// foreach (string item in inventory)
// {
//     Debug.Log(item);
// }