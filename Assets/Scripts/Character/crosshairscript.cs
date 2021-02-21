using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crosshairscript : MonoBehaviour
{
    public Vector2 MouseSensitivity = Vector2.zero;
    public bool Inverted = false;
    public Vector2 CurrentMousePosition { get; private set; }

    private Vector2 CrosshairStartingPosition;

    [SerializeField, Range(0.0f,1.0f)]
    private float HorizontalPercentageConstrain;
    [SerializeField, Range(0.0f, 1.0f)]
    private float VerticalPercentageConstrain;
    private float HorizontalConstrain;
    private float VerticalConstrain;


    private Vector2 CurrentLookDelta = Vector2.zero;

    private float MaxHorizontalConstrainValue;
    private float MaxVerticalConstrainValue;

    private float MinHorizontalConstrainValue;
    private float MinVerticalConstrainValue;

    private InputActions input_Actions;

    

    private void Awake()
    {
        input_Actions = new InputActions();
    }

    void Start()
    {
        if (GameManager.Instance.CursorActive)
            AppEvents.Invoke_MouseCursorEnable(false);

        CrosshairStartingPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);

        HorizontalConstrain = (Screen.width * HorizontalPercentageConstrain) / 2f;

        MinHorizontalConstrainValue = -(Screen.width / 2) + HorizontalConstrain;
        MaxHorizontalConstrainValue = (Screen.width / 2) - HorizontalConstrain;

        VerticalConstrain = (Screen.height * VerticalPercentageConstrain) * 2f;
        MinVerticalConstrainValue = -(Screen.width / 2) + VerticalConstrain;
        MaxVerticalConstrainValue = (Screen.width / 2) - VerticalConstrain;

    }

    void Update()
    {
        float crosshairXPosition = CrosshairStartingPosition.x + CurrentLookDelta.x;

        float crosshairYPosition = Inverted
            ? CrosshairStartingPosition.y - CurrentLookDelta.y
            : CrosshairStartingPosition.y + CurrentLookDelta.y;

        CurrentMousePosition = new Vector2(crosshairXPosition, crosshairYPosition);
        
        transform.position = CurrentMousePosition;

    }

    private void OnLook(UnityEngine.InputSystem.InputAction.CallbackContext delta)
    {
        Vector2 mouseDelta = delta.ReadValue<Vector2>();
        CurrentLookDelta.x += mouseDelta.x * MouseSensitivity.x;
        if (CurrentLookDelta.x >= MaxHorizontalConstrainValue || CurrentLookDelta.x <= MinHorizontalConstrainValue)
        {
            CurrentLookDelta.x -= mouseDelta.x * MouseSensitivity.x;
        }

        CurrentLookDelta.y += mouseDelta.y * MouseSensitivity.y;
        if (CurrentLookDelta.y >= MaxVerticalConstrainValue || CurrentLookDelta.y <= MinVerticalConstrainValue)
        {
            CurrentLookDelta.y -= mouseDelta.y * MouseSensitivity.y;
        }
    }


    private void OnEnable()
    {
        input_Actions.Enable();
        input_Actions.ThirdPerson.Look.performed += OnLook;

    }


    private void OnDisable()
    {
        input_Actions.Disable();
        input_Actions.ThirdPerson.Look.performed -= OnLook;

    }
}
