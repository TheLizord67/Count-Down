using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    [SerializeField] public static Vector2 movement;

    [SerializeField] private PlayerInput input;

    [SerializeField] private InputAction moveAction;

    [SerializeField] private InputAction dashAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        input = GetComponent<PlayerInput>();

        moveAction = input.actions["Move"];

        dashAction = input.actions["Dash"];
    }

    // Update is called once per frame
    void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
    }
}
