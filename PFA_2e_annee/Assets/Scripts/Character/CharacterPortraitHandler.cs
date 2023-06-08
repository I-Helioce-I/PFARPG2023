using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterPortraitEmotion
{
    Neutral,
    Angry,
    Sad,
    Flustered,
    Edgy,
}
public class CharacterPortraitHandler : MonoBehaviour
{
    public delegate void CharacterEmotionEvent(CharacterPortraitEmotion fromState, CharacterPortraitEmotion toState);
    public event CharacterEmotionEvent TransitionedFromTo = null;

    public CharacterPortraitSet PortraitSet;
    public CharacterPortraitEmotion StartingState;

    [SerializeField][ReadOnlyInspector] private CharacterPortraitEmotion _internalEmotion;
    public CharacterPortraitEmotion CurrentEmotion
    {
        get
        {
            return _internalEmotion;
        }
        set
        {
            if (_internalEmotion == value) return;
            TransitionToState(value);
        }
    }
    public Sprite CurrentPortrait
    {
        get
        {
            switch (CurrentEmotion)
            {
                case CharacterPortraitEmotion.Neutral:
                    return PortraitSet.Neutral;
                    break;
                case CharacterPortraitEmotion.Angry:
                    return PortraitSet.Angry;
                    break;
                case CharacterPortraitEmotion.Sad:
                    return PortraitSet.Sad;
                    break;
                case CharacterPortraitEmotion.Flustered:
                    return PortraitSet.Flustered;
                    break;
                case CharacterPortraitEmotion.Edgy:
                    return PortraitSet.Edgy;
                    break;
                default:
                    return PortraitSet.Neutral;
                    break;
            }
        }
    }

    private void Start()
    {
        CurrentEmotion = StartingState;
    }

    private void TransitionToState(CharacterPortraitEmotion toState)
    {
        CharacterPortraitEmotion fromState = _internalEmotion;
        _internalEmotion = toState;

        OnEmotionTransition(fromState, toState);

        TransitionedFromTo?.Invoke(fromState, toState);
    }

    private void OnEmotionTransition(CharacterPortraitEmotion fromState, CharacterPortraitEmotion toState)
    {
        switch (fromState)
        {
            case CharacterPortraitEmotion.Neutral:
                break;
            case CharacterPortraitEmotion.Angry:
                break;
            case CharacterPortraitEmotion.Sad:
                break;
            case CharacterPortraitEmotion.Flustered:
                break;
            case CharacterPortraitEmotion.Edgy:
                break;
            default:
                break;
        }

        switch (toState)
        {
            case CharacterPortraitEmotion.Neutral:
                break;
            case CharacterPortraitEmotion.Angry:
                break;
            case CharacterPortraitEmotion.Sad:
                break;
            case CharacterPortraitEmotion.Flustered:
                break;
            case CharacterPortraitEmotion.Edgy:
                break;
            default:
                break;
        }
    }
}
