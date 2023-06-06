using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PushableState
{
    Default,
    Frozen,
}

public class KRB_PushableBlock : MonoBehaviour, ICharacterController
{
    [Header("Kinematic motor")]
    [SerializeField] private KinematicCharacterMotor _motor;

    [Header("Grounded movement")]
    [SerializeField] private float _maxMoveSpeed = 2f;
    [SerializeField] private float _pushedMovementSharpness = 2f;
    [SerializeField] private float _unpushedMovementSharpness = 5f;
    [SerializeField] private float _orientationSharpness = 10f;
    [SerializeField] private float _pushAcceleration = 1f;
    private float _currentMoveSpeed = 0f;

    [Header("Air movement")]
    [SerializeField] private float _maxAirMoveSpeed = 30f;
    [SerializeField] private float _airAcceleration = 2f;
    [SerializeField] private float _airDrag = 0f;

    [Header("Miscellaneous")]
    [SerializeField] private List<Collider> _ignoredColliders = new List<Collider>();
    [SerializeField] private Vector3 _gravity = new Vector3(0, -30f, 0);
    [SerializeField] private Transform _meshRoot;
    [SerializeField] private PushableState _internalState = PushableState.Default;
    public PushableState CurrentState
    {
        get
        {
            return _internalState;
        }
        set
        {
            _internalState = value;
        }
    }

    private Collider[] _probedColliders = new Collider[8];
    private RaycastHit[] _probedHits = new RaycastHit[8];
    private Vector3 _pushVector;
    private Vector3 _internalVelocityAdd = Vector3.zero;

    private void Awake()
    {
        // Assign the characterController to the motor
        _motor.CharacterController = this;

        if (_meshRoot == null)
        {
            _meshRoot = this.transform;
        }
    }

    #region ICharacterController Interface

    public bool CharacterGrounded()
    {
        return _motor.GroundingStatus.IsStableOnGround;
    }

    public void Freeze()
    {
        CurrentState = PushableState.Frozen;
    }

    public void Unfreeze()
    {
        CurrentState = PushableState.Default;
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {

    }
    /// <summary>
    /// This is called when the motor wants to know what its velocity should be right now
    /// </summary>
    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        switch (_internalState)
        {
            case PushableState.Default:
                // Ground movement
                if (_motor.GroundingStatus.IsStableOnGround)
                {
                    float currentVelocityMagnitude = currentVelocity.magnitude;

                    Vector3 effectiveGroundNormal = _motor.GroundingStatus.GroundNormal;

                    // Reorient velocity on slope
                    currentVelocity = _motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                    // Calculate target velocity
                    Vector3 inputRight = Vector3.Cross(_pushVector, _motor.CharacterUp);
                    Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _pushVector.magnitude;

                    Vector3 targetMovementVelocity = reorientedInput * _maxMoveSpeed;

                    // Smooth movement Velocity
                    if (_pushVector != Vector3.zero)
                    {
                        currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-_pushedMovementSharpness * deltaTime));
                    }
                    else
                    {
                        currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-_unpushedMovementSharpness * deltaTime));
                    }


                    _pushVector = Vector3.zero;
                }
                // Air movement
                else
                {
                    // Gravity
                    currentVelocity += _gravity * deltaTime;

                    // Drag
                    currentVelocity *= (1f / (1f + (_airDrag * deltaTime)));
                }

                if (_internalVelocityAdd.sqrMagnitude > 0f)
                {
                    currentVelocity += _internalVelocityAdd;
                    _internalVelocityAdd = Vector3.zero;
                }
                break;
            case PushableState.Frozen:
                break;
            default:
                break;
        }




    }

    /// <summary>
    /// This is called before the motor does anything
    /// </summary>
    public void BeforeCharacterUpdate(float deltaTime)
    {

    }
    /// <summary>
    /// This is called after the motor has finished its ground probing, but before PhysicsMover/Velocity/etc.... handling
    /// </summary>
    public void PostGroundingUpdate(float deltaTime)
    {
        // Handle landing and leaving ground
        if (_motor.GroundingStatus.IsStableOnGround && !_motor.LastGroundingStatus.IsStableOnGround)
        {
            OnLanded();
        }
        else if (!_motor.GroundingStatus.IsStableOnGround && _motor.LastGroundingStatus.IsStableOnGround)
        {
            OnLeaveStableGround();
        }
    }
    /// <summary>
    /// This is called after the motor has finished everything in its update
    /// </summary>
    public void AfterCharacterUpdate(float deltaTime)
    {

    }
    /// <summary>
    /// This is called after when the motor wants to know if the collider can be collided with (or if we just go through it)
    /// </summary>
    public bool IsColliderValidForCollisions(Collider coll)
    {
        if (_ignoredColliders.Count == 0)
        {
            return true;
        }

        if (_ignoredColliders.Contains(coll))
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// This is called when the motor's ground probing detects a ground hit
    /// </summary>
    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {

    }
    /// <summary>
    /// This is called when the motor's movement logic detects a hit
    /// </summary>
    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {

    }
    /// <summary>
    /// This is called after every move hit, to give you an opportunity to modify the HitStabilityReport to your liking
    /// </summary>
    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {

    }
    /// <summary>
    /// This is called when the character detects discrete collisions (collisions that don't result from the motor's capsuleCasts when moving)
    /// </summary>
    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {

    }

    #endregion

    protected void OnLanded()
    {
    }

    protected void OnLeaveStableGround()
    {
    }

    public void AddVelocity(Vector3 velocity)
    {
        _internalVelocityAdd += velocity;
    }

    public void Impulse(Vector3 direction, float impulseForce, float ungroundTime)
    {
        _motor.ForceUnground(ungroundTime);
        AddVelocity(direction * impulseForce);
    }

    public void PushObject(Vector3 direction)
    {
        if (direction.y != 0)
        {
            direction = new Vector3(direction.x, 0, direction.z);
        }

        _pushVector = direction;
    }
}
