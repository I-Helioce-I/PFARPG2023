using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Default,
    Displacing,
}

public enum OrientationMethod
{
    TowardsCamera,
    TowardsMovement,
}

public enum DisplacementState
{
    None,
    Anchoring,
    Displacing,
    Deanchoring,
}

public struct PlayerCharacterInputs
{
    public float MoveAxisForward;
    public float MoveAxisRight;
    public Quaternion CameraRotation;
    public bool JumpDown;
    public bool CrouchDown;
    public bool CrouchUp;
}

public struct AICharacterInputs
{
    public Vector3 MoveVector;
    public Vector3 LookVector;
}

public enum BonusOrientationMethod
{
    None,
    TowardsGravity,
    TowardsGroundSlopeAndGravity,
}

public class KRB_CharacterController : MonoBehaviour, ICharacterController
{
    public KinematicCharacterMotor Motor;
    public CharacterStateHandler CharacterStateHandler;

    [Header("Character state")]
    [SerializeField][ReadOnlyInspector] private CharacterTypeState _currentCharacterState;

    [Header("Stable Movement")]
    public float MaxStableMoveSpeed = 10f;
    public float StableMovementSharpness = 15f;
    public float OrientationSharpness = 10f;
    public OrientationMethod OrientationMethod = OrientationMethod.TowardsCamera;

    [Header("Air Movement")]
    public float MaxAirMoveSpeed = 15f;
    public float AirAccelerationSpeed = 15f;
    public float Drag = 0.1f;
    public float GasStateGroundCheckDistance = .2f;

    [Header("Jumping")]
    public bool AllowJumpingWhenSliding = false;
    public float JumpUpSpeed = 10f;
    public float JumpScalableForwardSpeed = 10f;
    public float JumpPreGroundingGraceTime = 0f;
    public float JumpPostGroundingGraceTime = 0f;

    [Header("Pushing blocks")]
    [SerializeField] private bool _canPushBlocks = false;
    [SerializeField] private float _timeToStartPushing = .5f;
    [SerializeField] private float _moveInputVectorGrace = .8f;
    private float _pushingRequestTime = 0f;
    private bool _isTryingToPushBlock = false;
    private bool _isPushingBlock = false;
    private Vector3 _pushingDirection = Vector3.zero;
    public bool IsPushingBlock => _isPushingBlock;

    [Header("Displacement")]
    [SerializeField] private float _anchorTime = .5f;
    [HideInInspector] public Displacer CurrentDisplacer;
    private DisplacementState _internalDisplacementState;
    private DisplacementState _currentDisplacementState
    {
        get
        {
            return _internalDisplacementState;
        }
        set
        {
            _internalDisplacementState = value;
            _anchoringTimer = 0f;
            _anchoringStartPosition = Motor.TransientPosition;
            _anchoringStartRotation = Motor.TransientRotation;
        }
    }
    private float _anchoringTimer = 0f;
    private Vector3 _anchoringStartPosition = Vector3.zero;
    private Quaternion _anchoringStartRotation = Quaternion.identity;
    private Quaternion _rotationBeforeDisplacement = Quaternion.identity;
    private Quaternion _deanchoringStartRotation = Quaternion.identity;
    private Vector3 _deanchoringStartPosition = Vector3.zero;
    [HideInInspector] public Vector3 DisplacerTargetPosition;
    [HideInInspector] public Quaternion DisplacerTargetRotation;
    [HideInInspector] public Vector3 DisplacerDestinationPosition;
    [HideInInspector] public Quaternion DisplacerDestinationRotation;
    [HideInInspector] public float DisplacementTime;
    private float _displacementTimer = 0f;

    [Header("Misc")]
    public List<Collider> IgnoredColliders = new List<Collider>();
    public BonusOrientationMethod BonusOrientationMethod = BonusOrientationMethod.None;
    public float BonusOrientationSharpness = 10f;
    public Vector3 Gravity = new Vector3(0, -30f, 0);
    public Transform MeshRoot;
    public Transform CameraFollowPoint;
    public float CrouchedCapsuleHeight = 1f;

    public CharacterState CurrentCharacterState { get; private set; }

    private Collider[] _probedColliders = new Collider[8];
    private RaycastHit[] _probedHits = new RaycastHit[8];
    private Vector3 _moveInputVector;
    private Vector3 _lookInputVector;
    private bool _jumpRequested = false;
    private bool _jumpConsumed = false;
    private bool _jumpedThisFrame = false;
    private float _timeSinceJumpRequested = Mathf.Infinity;
    private float _timeSinceLastAbleToJump = 0f;
    private Vector3 _internalVelocityAdd = Vector3.zero;
    private bool _shouldBeCrouching = false;
    private bool _isCrouching = false;

    private Vector3 lastInnerNormal = Vector3.zero;
    private Vector3 lastOuterNormal = Vector3.zero;

    private void Awake()
    {
        // Handle initial state
        TransitionToState(CharacterState.Default);

        // Assign the characterController to the motor
        Motor.CharacterController = this;
    }

    private void OnEnable()
    {
        CharacterStateHandler.TransitionedFromTo -= OnCharacterTypeStateTransition;
        CharacterStateHandler.TransitionedFromTo += OnCharacterTypeStateTransition;
    }

    private void OnDisable()
    {
        CharacterStateHandler.TransitionedFromTo -= OnCharacterTypeStateTransition;
    }

    private void OnCharacterTypeStateTransition(CharacterTypeState from, CharacterTypeState to)
    {
        _currentCharacterState = to;
    }

    /// <summary>
    /// Handles movement state transitions and enter/exit callbacks
    /// </summary>
    public void TransitionToState(CharacterState newState)
    {
        CharacterState tmpInitialState = CurrentCharacterState;
        OnStateExit(tmpInitialState, newState);
        CurrentCharacterState = newState;
        OnStateEnter(newState, tmpInitialState);
    }

    /// <summary>
    /// Event when entering a state
    /// </summary>
    public void OnStateEnter(CharacterState state, CharacterState fromState)
    {
        switch (state)
        {
            case CharacterState.Default:
                {
                    break;
                }
            case CharacterState.Displacing:
                {
                    _rotationBeforeDisplacement = Motor.TransientRotation;

                    Motor.SetMovementCollisionsSolvingActivation(false);
                    Motor.SetGroundSolvingActivation(false);
                    _currentDisplacementState = DisplacementState.Anchoring;

                    _anchoringTimer = 0f;
                    _displacementTimer = 0f;

                    break;
                }
        }
    }

    /// <summary>
    /// Event when exiting a state
    /// </summary>
    public void OnStateExit(CharacterState state, CharacterState toState)
    {
        switch (state)
        {
            case CharacterState.Default:
                {
                    break;
                }
            case CharacterState.Displacing:
                {

                    Motor.SetMovementCollisionsSolvingActivation(true);
                    Motor.SetGroundSolvingActivation(true);
                    break;
                }
        }
    }

    /// <summary>
    /// This is called every frame by ExamplePlayer in order to tell the character what its inputs are
    /// </summary>
    public void SetInputs(ref PlayerCharacterInputs inputs)
    {
        // Clamp input
        Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

        // Calculate camera direction and rotation on the character plane
        Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
        if (cameraPlanarDirection.sqrMagnitude == 0f)
        {
            cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
        }
        Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

        switch (CurrentCharacterState)
        {
            case CharacterState.Default:
                {
                    // Move and look inputs
                    _moveInputVector = cameraPlanarRotation * moveInputVector;

                    switch (OrientationMethod)
                    {
                        case OrientationMethod.TowardsCamera:
                            _lookInputVector = cameraPlanarDirection;
                            break;
                        case OrientationMethod.TowardsMovement:
                            _lookInputVector = _moveInputVector.normalized;
                            break;
                    }

                    // Jumping input
                    if (inputs.JumpDown)
                    {
                        _timeSinceJumpRequested = 0f;
                        _jumpRequested = true;
                    }

                    // Crouching input
                    if (inputs.CrouchDown)
                    {
                        _shouldBeCrouching = true;

                        if (!_isCrouching)
                        {
                            _isCrouching = true;
                            Motor.SetCapsuleDimensions(0.5f, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
                            MeshRoot.localScale = new Vector3(1f, 0.5f, 1f);
                        }
                    }
                    else if (inputs.CrouchUp)
                    {
                        _shouldBeCrouching = false;
                    }

                    break;
                }
        }
    }

    /// <summary>
    /// This is called every frame by the AI script in order to tell the character what its inputs are
    /// </summary>
    public void SetInputs(ref AICharacterInputs inputs)
    {
        _moveInputVector = inputs.MoveVector;
        _lookInputVector = inputs.LookVector;
    }

    private Quaternion _tmpTransientRot;

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is called before the character begins its movement update
    /// </summary>
    public void BeforeCharacterUpdate(float deltaTime)
    {
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is where you tell your character what its rotation should be right now. 
    /// This is the ONLY place where you should set the character's rotation
    /// </summary>
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        switch (CurrentCharacterState)
        {
            case CharacterState.Default:
                {
                    if (_lookInputVector.sqrMagnitude > 0f && OrientationSharpness > 0f)
                    {
                        // Smoothly interpolate from current to target look direction
                        Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                        if (_isPushingBlock)
                        {
                            smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _pushingDirection, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;
                        }

                        // Set the current rotation (which will be used by the KinematicCharacterMotor)
                        currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
                    }

                    Vector3 currentUp = (currentRotation * Vector3.up);
                    if (BonusOrientationMethod == BonusOrientationMethod.TowardsGravity)
                    {
                        // Rotate from current up to invert gravity
                        Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                    }
                    else if (BonusOrientationMethod == BonusOrientationMethod.TowardsGroundSlopeAndGravity)
                    {
                        if (Motor.GroundingStatus.IsStableOnGround)
                        {
                            Vector3 initialCharacterBottomHemiCenter = Motor.TransientPosition + (currentUp * Motor.Capsule.radius);

                            Vector3 smoothedGroundNormal = Vector3.Slerp(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGroundNormal) * currentRotation;

                            // Move the position to create a rotation around the bottom hemi center instead of around the pivot
                            Motor.SetTransientPosition(initialCharacterBottomHemiCenter + (currentRotation * Vector3.down * Motor.Capsule.radius));
                        }
                        else
                        {
                            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        }
                    }
                    else
                    {
                        Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                    }
                    break;
                }
            case CharacterState.Displacing:
                switch (_currentDisplacementState)
                {
                    case DisplacementState.None:
                        break;
                    case DisplacementState.Anchoring:
                        {
                            currentRotation = Quaternion.Slerp(_anchoringStartRotation, DisplacerTargetRotation, (_anchoringTimer / _anchorTime));
                            break;
                        }
                    case DisplacementState.Displacing:
                        {
                            currentRotation = Quaternion.Slerp(DisplacerTargetRotation, DisplacerDestinationRotation, (_displacementTimer / DisplacementTime));
                            break;
                        }
                    case DisplacementState.Deanchoring:
                        {
                            currentRotation = Quaternion.Slerp(_deanchoringStartRotation, DisplacerDestinationRotation, (_anchoringTimer / _anchorTime));
                            break;
                        }
                    default:
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is where you tell your character what its velocity should be right now. 
    /// This is the ONLY place where you can set the character's velocity
    /// </summary>
    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        switch (CurrentCharacterState)
        {
            case CharacterState.Default:
                {
                    // Ground movement
                    if (Motor.GroundingStatus.IsStableOnGround)
                    {
                        float currentVelocityMagnitude = currentVelocity.magnitude;

                        Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

                        // Reorient velocity on slope
                        currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                        // Calculate target velocity
                        Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                        Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _moveInputVector.magnitude;

                        if (_isPushingBlock)
                        {
                            float dotproduct = Vector3.Dot(_pushingDirection, reorientedInput);
                            if (dotproduct > _moveInputVectorGrace)
                            {
                                //Set movement direction to pushing direction
                                inputRight = Vector3.Cross(_pushingDirection, Motor.CharacterUp);
                                reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                            }
                            else
                            {
                                //Don't do anything to movement direction
                            }
                        }

                        Vector3 targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

                        // Smooth movement Velocity
                        currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-StableMovementSharpness * deltaTime));
                    }
                    // Air movement
                    else
                    {
                        // Add move input
                        if (_moveInputVector.sqrMagnitude > 0f)
                        {
                            Vector3 addedVelocity = _moveInputVector * AirAccelerationSpeed * deltaTime;

                            Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                            // Limit air velocity from inputs
                            if (currentVelocityOnInputsPlane.magnitude < MaxAirMoveSpeed)
                            {
                                // clamp addedVel to make total vel not exceed max vel on inputs plane
                                Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, MaxAirMoveSpeed);
                                addedVelocity = newTotal - currentVelocityOnInputsPlane;
                            }
                            else
                            {
                                // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                                if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                                {
                                    addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                                }
                            }

                            // Prevent air-climbing sloped walls
                            if (Motor.GroundingStatus.FoundAnyGround)
                            {
                                if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                                {
                                    Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                                    addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                                }
                            }

                            // Apply added velocity
                            currentVelocity += addedVelocity;
                        }

                        // Gravity
                        if (_currentCharacterState == CharacterTypeState.Solid || _currentCharacterState == CharacterTypeState.Liquid)
                        {
                            currentVelocity += Gravity * deltaTime;
                        }
                        else if (_currentCharacterState == CharacterTypeState.Gas && Physics.Raycast(Motor.TransientPosition, Vector3.down, GasStateGroundCheckDistance, Motor.StableGroundLayers))
                        {
                            currentVelocity += Gravity * deltaTime;
                        }

                        // Drag
                        currentVelocity *= (1f / (1f + (Drag * deltaTime)));
                    }

                    // Handle jumping
                    _jumpedThisFrame = false;
                    _timeSinceJumpRequested += deltaTime;
                    if (_jumpRequested && _currentCharacterState == CharacterTypeState.Solid)
                    {
                        // See if we actually are allowed to jump
                        if (!_jumpConsumed && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime))
                        {
                            // Calculate jump direction before ungrounding
                            Vector3 jumpDirection = Motor.CharacterUp;
                            if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                            {
                                jumpDirection = Motor.GroundingStatus.GroundNormal;
                            }

                            // Makes the character skip ground probing/snapping on its next update. 
                            // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                            Motor.ForceUnground();

                            // Add to the return velocity and reset jump state
                            currentVelocity += (jumpDirection * JumpUpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                            currentVelocity += (_moveInputVector * JumpScalableForwardSpeed);
                            _jumpRequested = false;
                            _jumpConsumed = true;
                            _jumpedThisFrame = true;
                        }
                    }

                    if (_isTryingToPushBlock && _moveInputVector != Vector3.zero)
                    {
                        if (!_isPushingBlock)
                        {
                            _pushingRequestTime += deltaTime;
                            if (_pushingRequestTime > _timeToStartPushing)
                            {
                                _isPushingBlock = true;
                            }
                        }
                        else
                        {
                            _pushingRequestTime = 0f;
                        }
                    }
                    else
                    {
                        _pushingRequestTime = 0f;
                        _isPushingBlock = false;
                        _isTryingToPushBlock = false;
                        _pushingDirection = Vector3.zero;
                    }

                    // Take into account additive velocity
                    if (_internalVelocityAdd.sqrMagnitude > 0f)
                    {
                        currentVelocity += _internalVelocityAdd;
                        _internalVelocityAdd = Vector3.zero;
                    }
                    break;
                }
            case CharacterState.Displacing:
                {
                    currentVelocity = Vector3.zero;

                    switch (_currentDisplacementState)
                    {
                        case DisplacementState.None:
                            break;
                        case DisplacementState.Anchoring:
                            Vector3 tmpPositionAnchor = Vector3.Lerp(_anchoringStartPosition, DisplacerTargetPosition, (_anchoringTimer / _anchorTime));
                            currentVelocity = Motor.GetVelocityForMovePosition(Motor.TransientPosition, tmpPositionAnchor, deltaTime);
                            break;
                        case DisplacementState.Displacing:
                            Vector3 tmpDisplacement = Vector3.Lerp(DisplacerTargetPosition, DisplacerDestinationPosition, CurrentDisplacer.FromToCurve.Evaluate(_displacementTimer / DisplacementTime));
                            currentVelocity = Motor.GetVelocityForMovePosition(Motor.TransientPosition, tmpDisplacement, deltaTime);
                            break;
                        case DisplacementState.Deanchoring:
                            Vector3 tmpPosition = Vector3.Lerp(_deanchoringStartPosition, DisplacerDestinationPosition, (_anchoringTimer / _anchorTime));
                            currentVelocity = Motor.GetVelocityForMovePosition(Motor.TransientPosition, tmpPosition, deltaTime);
                            break;
                        default:
                            break;
                    }

                    break;
                }

        }
    }

    /// <summary>
    /// (Called by KinematicCharacterMotor during its update cycle)
    /// This is called after the character has finished its movement update
    /// </summary>
    public void AfterCharacterUpdate(float deltaTime)
    {
        switch (CurrentCharacterState)
        {
            case CharacterState.Default:
                {
                    // Handle jump-related values
                    {
                        // Handle jumping pre-ground grace period
                        if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
                        {
                            _jumpRequested = false;
                        }

                        if (AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
                        {
                            // If we're on a ground surface, reset jumping values
                            if (!_jumpedThisFrame)
                            {
                                _jumpConsumed = false;
                            }
                            _timeSinceLastAbleToJump = 0f;
                        }
                        else
                        {
                            // Keep track of time since we were last able to jump (for grace period)
                            _timeSinceLastAbleToJump += deltaTime;
                        }
                    }

                    // Handle uncrouching
                    if (_isCrouching && !_shouldBeCrouching)
                    {
                        // Do an overlap test with the character's standing height to see if there are any obstructions
                        Motor.SetCapsuleDimensions(0.5f, 2f, 1f);
                        if (Motor.CharacterOverlap(
                            Motor.TransientPosition,
                            Motor.TransientRotation,
                            _probedColliders,
                            Motor.CollidableLayers,
                            QueryTriggerInteraction.Ignore) > 0)
                        {
                            // If obstructions, just stick to crouching dimensions
                            Motor.SetCapsuleDimensions(0.5f, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
                        }
                        else
                        {
                            // If no obstructions, uncrouch
                            MeshRoot.localScale = new Vector3(1f, 1f, 1f);
                            _isCrouching = false;
                        }
                    }
                    break;
                }
            case CharacterState.Displacing:
                {
                    switch (_currentDisplacementState)
                    {
                        case DisplacementState.None:
                            break;
                        case DisplacementState.Anchoring:
                            if (_anchoringTimer >= _anchorTime)
                            {
                                _anchoringTimer = 0f;
                                _currentDisplacementState = DisplacementState.Displacing;
                            }
                            else
                            {
                                _anchoringTimer += deltaTime;
                            }
                            break;
                        case DisplacementState.Displacing:
                            if (Vector3.Distance(Motor.TransientPosition, DisplacerDestinationPosition) < 0.05f)
                            {
                                _deanchoringStartRotation = Motor.TransientRotation;
                                _deanchoringStartPosition = Motor.TransientPosition;

                                _displacementTimer = 0f;
                                _currentDisplacementState = DisplacementState.Deanchoring;
                            }
                            else
                            {
                                _displacementTimer += deltaTime;
                            }
                            break;
                        case DisplacementState.Deanchoring:
                            if (_anchoringTimer >= _anchorTime)
                            {
                                _anchoringTimer = 0f;
                                TransitionToState(CharacterState.Default);
                            }
                            else
                            {
                                _anchoringTimer += deltaTime;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                }
        }
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        // Handle landing and leaving ground
        if (Motor.GroundingStatus.IsStableOnGround && !Motor.LastGroundingStatus.IsStableOnGround)
        {
            OnLanded();
        }
        else if (!Motor.GroundingStatus.IsStableOnGround && Motor.LastGroundingStatus.IsStableOnGround)
        {
            OnLeaveStableGround();
        }
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        if (IgnoredColliders.Count == 0)
        {
            return true;
        }

        if (IgnoredColliders.Contains(coll))
        {
            return false;
        }

        return true;
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        //Pushing blocks
        // Write here the collision code for pushing an pushable object.
        if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Pushable") && _canPushBlocks && _currentCharacterState == CharacterTypeState.Solid)
        {
            KRB_PushableBlock pushableObject = hitCollider.GetComponent<KRB_PushableBlock>();

            //Here, cache the PushableObject.
            //In this code, set a bool 'isTryingToPush' to true. When this is true, in the update, increase a timer by deltatime to determine the 'time' spent to push
            //the obstacle so that there is confirmation of a request to push. When the timer is over the time necessary to accept request to push, the
            //character is placed in a push position (similar to how ladders work...) while 'isTryingToPush' remains true.
            //While isTryingToPush is true and the request has been confirmed, then the character actually pushes the PushableObject via a method determined in
            //PushableObject. This should use a PhysicsMover as explained in the kinematicRBController to move the block around.

            //When in an isPushing state, the player's inputs automatically redirect to the opposite of the pushed normal as long as they're within a certain angle
            //of that pushed normal, so as to 'lock' the player in the direction of the push until they decide to go away by inputting a direction that goes further than
            //the established grace dotproduct.

            float dotproduct = Vector3.Dot(hitNormal, _moveInputVector);
            if (dotproduct < -_moveInputVectorGrace)
            {
                _isTryingToPushBlock = true;
                _pushingDirection = -hitNormal;

                if (_isPushingBlock)
                {
                    pushableObject.PushObject(-hitNormal);
                }

            }
            else
            {
                _isTryingToPushBlock = false;
            }

        }

    }

    public void AddVelocity(Vector3 velocity)
    {
        switch (CurrentCharacterState)
        {
            case CharacterState.Default:
                {
                    _internalVelocityAdd += velocity;
                    break;
                }
        }
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
    }

    protected void OnLanded()
    {
    }

    protected void OnLeaveStableGround()
    {
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
    }
}
