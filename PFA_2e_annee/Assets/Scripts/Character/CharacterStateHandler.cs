using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CharacterTypeState
{
    None,
    Solid,
    Liquid,
    Gas,
    TriplePoint,
}
public class CharacterStateHandler : MonoBehaviour
{
    public delegate void CharacterTypeStateEvent(CharacterTypeState fromState, CharacterTypeState toState);
    public event CharacterTypeStateEvent TransitionedFromTo = null;

    public CharacterStats CharacterStats;
    public CharacterBattle Battle;

    public CharacterTypeState StartingState;

    [SerializeField][ReadOnlyInspector] private CharacterTypeState _internalState;

    public List<CharacterTypeState> PossibleStates = new List<CharacterTypeState>();

    public GameObject SolidCharacterMesh;
    public GameObject LiquidCharacterMesh;
    public GameObject GasCharacterMesh;

    public CharacterAnimatorHandler AnimatorHandler;

    public ParticleSystem VFX_SolidTransitionFlash;
    public ParticleSystem VFX_LiquidTransitionFlash;
    public ParticleSystem VFX_GasTransitionFlash;
    private IEnumerator _currentFlashCoroutine;
    private Renderer[] _renderers;

    public CharacterTypeState CharacterTypeState
    {
        get
        {
            return _internalState;
        }
        set
        {
            if (_internalState == value) return;
            Flash(1f, 100f);
            AnimatorHandler.PlayAnimThenAction("TransitionIn", () =>
            {
                TransitionToState(value);
            });
        }
    }

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);

        CharacterTypeState = StartingState;
        TransitionToState(StartingState);
    }

    private void Start()
    {
        switch (StartingState)
        {
            case CharacterTypeState.Solid:
                CharacterStats.Temperature.Damage(7f);
                break;
            case CharacterTypeState.Liquid:
                CharacterStats.Temperature.Damage(4f);
                break;
            case CharacterTypeState.Gas:
                CharacterStats.Temperature.Damage(1f);
                break;
            default:
                break;
        }
    }

    public void CheckTemperatureTransitions()
    {
        float value = CharacterStats.Temperature.CurrentValue;
        if (value < 4f)
        {
            CharacterTypeState = CharacterTypeState.Solid;
        }
        else if (value >= 4f && value < 7f)
        {
            CharacterTypeState = CharacterTypeState.Liquid;
        }
        else if (value >= 7f && value <= 9f)
        {
            CharacterTypeState = CharacterTypeState.Gas;
        }
    }

    private void TransitionToState(CharacterTypeState toState)
    {
        CharacterTypeState fromState = _internalState;
        _internalState = toState;

        OnCharacterTransition(fromState, toState);

        TransitionedFromTo?.Invoke(fromState, toState);
    }

    private void OnCharacterTransition(CharacterTypeState fromState, CharacterTypeState toState)
    {
        switch (fromState)
        {
            case CharacterTypeState.None:
                break;
            case CharacterTypeState.Solid:
                SolidCharacterMesh.SetActive(false);
                break;
            case CharacterTypeState.Liquid:
                LiquidCharacterMesh.SetActive(false);
                break;
            case CharacterTypeState.Gas:
                GasCharacterMesh.SetActive(false);
                break;
            case CharacterTypeState.TriplePoint:
                break;
            default:
                break;
        }

        switch (toState)
        {
            case CharacterTypeState.None:
                break;
            case CharacterTypeState.Solid:
                SolidCharacterMesh.SetActive(true);
                AnimatorHandler = SolidCharacterMesh.GetComponent<CharacterAnimatorHandler>();
                Battle.CharacterAnimatorHandler = AnimatorHandler;
                VFX_SolidTransitionFlash.Play();
                break;
            case CharacterTypeState.Liquid:
                LiquidCharacterMesh.SetActive(true);
                AnimatorHandler = LiquidCharacterMesh.GetComponent<CharacterAnimatorHandler>();
                Battle.CharacterAnimatorHandler = AnimatorHandler;
                VFX_LiquidTransitionFlash.Play();
                break;
            case CharacterTypeState.Gas:
                GasCharacterMesh.SetActive(true);
                AnimatorHandler = GasCharacterMesh.GetComponent<CharacterAnimatorHandler>();
                Battle.CharacterAnimatorHandler = AnimatorHandler;
                VFX_GasTransitionFlash.Play();
                break;
            case CharacterTypeState.TriplePoint:
                break;
            default:
                break;
        }
        AnimatorHandler.PlayAnimThenAction("TransitionOut", null);
        Flash(100f, 1f, false);
    }

    private void ForceTransitionNoCalls(CharacterTypeState toState)
    {
        CharacterTypeState fromState = _internalState;
        _internalState = toState;
        OnCharacterTransition(fromState, toState);
    }

    public void SwitchStateForward()
    {
        int currentIndex = -1;
        for (int i = 0; i < PossibleStates.Count; i++)
        {
            if (_internalState == PossibleStates[i])
            {
                currentIndex = i;
            }
        }

        currentIndex++;

        if (currentIndex > PossibleStates.Count - 1)
        {
            //Play transitionIn animation. At the end of the animation, play transitionOut animation.
            Flash(1f, 100f);
            AnimatorHandler.PlayAnimThenAction("TransitionIn", () =>
            {
                TransitionToState(PossibleStates[0]);
            });

        }
        else
        {
            Flash(1f, 100f);
            AnimatorHandler.PlayAnimThenAction("TransitionIn", () =>
            {
                TransitionToState(PossibleStates[currentIndex]);
            });
        }
    }

    public void SwitchStateBackward()
    {
        int currentIndex = -1;
        for (int i = 0; i < PossibleStates.Count; i++)
        {
            if (_internalState == PossibleStates[i])
            {
                currentIndex = i;
            }
        }

        currentIndex--;

        if (currentIndex < 0)
        {
            Flash(1f, 100f);
            AnimatorHandler.PlayAnimThenAction("TransitionIn", () =>
            {
                TransitionToState(PossibleStates[PossibleStates.Count - 1]);
            });
        }
        else
        {
            Flash(1f, 100f);
            AnimatorHandler.PlayAnimThenAction("TransitionIn", () =>
            {
                TransitionToState(PossibleStates[currentIndex]);
            });
        }
    }

    public void SwitchStateTo(CharacterTypeState toState)
    {
        CharacterTypeState = toState;
        Flash(1f, 100f);
        AnimatorHandler.PlayAnimThenAction("TransitionIn", () =>
        {
            TransitionToState(toState);
        });
    }

    private void Flash(float fromIntensity, float toIntensity, bool revertToFrom = true)
    {
        if (_currentFlashCoroutine != null) StopCoroutine(_currentFlashCoroutine);
        _currentFlashCoroutine = MeshFlash(fromIntensity, toIntensity, .4f, revertToFrom);
        StartCoroutine(MeshFlash(fromIntensity, toIntensity, .4f, revertToFrom));
    }

    private IEnumerator MeshFlash(float fromIntensity, float toIntensity, float overTime, bool revertToFrom = true)
    {
        float timer = 0f;

        foreach (Renderer renderer in _renderers)
        {
            renderer.material.SetFloat("_albedoIntensity", fromIntensity);
        }

        while (timer < overTime)
        {
            timer += Time.deltaTime;
            float lerpdIntensity = Mathf.Lerp(fromIntensity, toIntensity, timer / overTime);
            foreach (Renderer renderer in _renderers)
            {
                renderer.material.SetFloat("_albedoIntensity", lerpdIntensity);
            }
            yield return null;
        }

        //Revert to original colors
        if (revertToFrom)
        {
            foreach (Renderer renderer in _renderers)
            {
                renderer.material.SetFloat("_albedoIntensity", fromIntensity);
            }
        }
        else
        {
            foreach (Renderer renderer in _renderers)
            {
                renderer.material.SetFloat("_albedoIntensity", toIntensity);
            }
        }

    }
}
