using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player instance;

    public Character Character;
    public KRB_CharacterController CharacterController;
    public PlayerInput PlayerInput;
    //public ExampleCharacterCamera CharacterCamera;
    public List<Character> AllControlledCharacters = new List<Character>();

    private Vector2 _movement = Vector2.zero;
    private bool _jumpPressed = false;
    private string _previousActionMap;
    public string PreviousActionMap
    {
        get
        {
            return _previousActionMap;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        // Tell camera to follow transform
        //CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

        //// Ignore the character's collider(s) for camera obstruction checks
        //CharacterCamera.IgnoredColliders.Clear();
        //CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
    }

    private void Update()
    {
        if (CharacterController) HandleCharacterInput();
    }

    private void LateUpdate()
    {
        // Handle rotating the camera along with physics movers
        //if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
        //{
        //    CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
        //    CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
        //}

        //HandleCameraInput();
    }

    private void HandleCameraInput()
    {
        // Create the look input vector for the camera
        //float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
        //float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
        //Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

        //// Prevent moving the camera while the cursor isn't locked
        //if (Cursor.lockState != CursorLockMode.Locked)
        //{
        //    lookInputVector = Vector3.zero;
        //}

        //// Input for zooming the camera (disabled in WebGL because it can cause problems)
        //float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

        // Apply inputs to the camera
        //CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

        // Handle toggling zoom level
        //if (Input.GetMouseButtonDown(1))
        //{
        //    CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
        //}
    }

    public void ChangeActionMap(string actionMapName)
    {
        _previousActionMap = PlayerInput.currentActionMap.name;
        PlayerInput.SwitchCurrentActionMap(actionMapName);
        Debug.Log("Player has switched to " + actionMapName + " action map!");
    }

    #region EXPLORATION
    public void OnMovementInput(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _jumpPressed = context.performed;
        HandleCharacterInput();
        _jumpPressed = false;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Character.InteractibleHandler.Interact();
        }
    }

    public void OnSwitchStateForward(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Character.CharacterExplorationStateHandler.SwitchStateForward();
        }
    }

    public void OnSwitchStateBackward(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Character.CharacterExplorationStateHandler.SwitchStateBackward();
        }
    }
    #endregion
    #region UI
    public void OnUINavigate(InputAction.CallbackContext context)
    {
        float up = context.ReadValue<Vector2>().y;
        if (up > .9f)
        {
            UIManager.instance.NavigateUp();
        }
        else if (up < -.9f)
        {
            UIManager.instance.NavigateDown();
        }
    }

    public void OnUISelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UIManager.instance.Select();
        }
    }

    public void OnUIReturn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UIManager.instance.Return();
        }
    }
    #endregion

    private void HandleCharacterInput()
    {
        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

        // Build the CharacterInputs struct
        characterInputs.MoveAxisForward = _movement.y;
        characterInputs.MoveAxisRight = _movement.x;
        characterInputs.CameraRotation = Camera.main.transform.rotation;
        characterInputs.JumpDown = _jumpPressed;

        // Apply inputs to character
        CharacterController.SetInputs(ref characterInputs);
    }

    public void AddGasCharacter()
    {
        foreach(Character character in AllControlledCharacters)
        {
            if (!character.CharacterExplorationStateHandler.PossibleStates.Contains(CharacterTypeState.Gas)) character.CharacterExplorationStateHandler.PossibleStates.Add(CharacterTypeState.Gas);
        }
    }
}
