using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimatorHandler : MonoBehaviour
{
    public enum AnimatorState
    {
        AnimatorMotor,
        ForcedAnimation,
    }

    public Animator Animator;
    [SerializeField] private KRB_CharacterController CharacterController;
    [SerializeField][ReadOnlyInspector] private AnimatorState _internalState = AnimatorState.AnimatorMotor;

    private string _currentAnimationPlaying;
    private Action _onAnimationComplete;
    private bool _animChecked = false;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (_internalState)
        {
            case AnimatorState.AnimatorMotor:
                HandleLocomotion();
                break;
            case AnimatorState.ForcedAnimation:
                if (AnimatorIsPlaying(_currentAnimationPlaying))
                {
                    _animChecked = true;
                }
                if(!AnimatorIsPlaying(_currentAnimationPlaying) && _animChecked)
                {
                    if (_onAnimationComplete != null)
                    {
                        _onAnimationComplete();
                        _onAnimationComplete = null;
                    }

                    _animChecked = false;
                    _internalState = AnimatorState.AnimatorMotor;
                }
                break;
            default:
                break;
        }
    }

    private void HandleLocomotion()
    {
        Animator.SetBool("isGrounded", CharacterController.Motor.GroundingStatus.IsStableOnGround);
        Animator.SetFloat("VerticalSpeed", CharacterController.Motor.Velocity.y);
        float forwardVelocity = 0;
        if (CharacterController.MoveInputVector != Vector3.zero)
        {
            if (Mathf.Abs(CharacterController.Motor.Velocity.x) > 0 || Mathf.Abs(CharacterController.Motor.Velocity.z) > 0)
            {
                forwardVelocity = 1;
            }
        }
        Animator.SetFloat("ForwardSpeed", forwardVelocity);
    }

    public void PlayAnimThenAction(string animationName, Action onAnimationComplete)
    {
        Animator.Play(animationName);
        _currentAnimationPlaying = animationName;
        _onAnimationComplete = onAnimationComplete;
        _internalState = AnimatorState.ForcedAnimation;
    }

    public void PlayAnim(string animationName)
    {
        Animator.Play(animationName);
        _currentAnimationPlaying = animationName;
        _internalState = AnimatorState.ForcedAnimation;
    }

    private bool AnimatorIsPlaying()
    {
        return Animator.GetCurrentAnimatorStateInfo(0).length > Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    private bool AnimatorIsPlaying(string animationName)
    {
        return AnimatorIsPlaying() && Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
}
