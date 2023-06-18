using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
{
    [SerializeField] protected UI_ButtonPrompt _interactButtonPrompt;

    [SerializeField] protected float _range;
    [SerializeField] protected bool _forcedInteraction;
    protected SphereCollider _collider;
    protected InteractibleHandler _currentHandler;
    private CharacterExplorationStateHandler _currentCESH;

    [Header("Outline material")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _outlineTransitionOverTime = .5f;
    [SerializeField] private float _lineThickness = .1f;
    private IEnumerator _outlineCoroutine;


    public UnityEvent<InteractibleHandler> OnInteract;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = _range;
        _collider.enabled = this.enabled;
    }

    private void Start()
    {
        if (!_forcedInteraction) _interactButtonPrompt.ForceDisappear();
    }

    private void OnEnable()
    {
        _collider.enabled = true;
    }

    private void OnDisable()
    {
        ShowPrompt(false);
        _collider.enabled = false;
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted with an interactible!");
        OnInteract?.Invoke(_currentHandler);
    }

    public void ShowPrompt(bool show)
    {
        if (_interactButtonPrompt == null) return;
        _interactButtonPrompt.Appear(show);

        if (_outlineCoroutine != null) StopCoroutine(_outlineCoroutine);
        if (_renderer)
        {
            if (show)
            {
                _outlineCoroutine = OutlineTransition(0f, _lineThickness, _outlineTransitionOverTime);
                StartCoroutine(OutlineTransition(0f, _lineThickness, _outlineTransitionOverTime));
            }
            else
            {
                _outlineCoroutine = OutlineTransition(_lineThickness, 0f, _outlineTransitionOverTime);
                StartCoroutine(OutlineTransition(_lineThickness, 0f, _outlineTransitionOverTime));
            }
        }

    }

    private void OnHandlerStateTransition(CharacterTypeState from, CharacterTypeState to)
    {
        switch (to)
        {
            case CharacterTypeState.None:
                break;
            case CharacterTypeState.Solid:
                break;
            case CharacterTypeState.Liquid:
                break;
            case CharacterTypeState.Gas:
                if (_forcedInteraction)
                {
                    Interact();
                }
                break;
            case CharacterTypeState.TriplePoint:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractibleHandler character = other.GetComponent<InteractibleHandler>();
        if (character)
        {
            character.SetInteractible(this);
            _currentHandler = character;
            CharacterExplorationStateHandler CESH = _currentHandler.GetComponent<CharacterExplorationStateHandler>();
            if (CESH)
            {
                _currentCESH = CESH;
                _currentCESH.TransitionedFromTo -= OnHandlerStateTransition;
                _currentCESH.TransitionedFromTo += OnHandlerStateTransition;
            }
        }

        if (_forcedInteraction)
        {
            Interact();
        }
        else
        {
            ShowPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractibleHandler character = other.GetComponent<InteractibleHandler>();
        if (character)
        {
            character.NullInteractible();
            _currentHandler = null;
            _currentCESH.TransitionedFromTo -= OnHandlerStateTransition;
            _currentCESH = null;
            ShowPrompt(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }

    private IEnumerator OutlineTransition(float fromWidth, float toWidth, float overTime)
    {
        Material outlineMat = _renderer.materials[1];
        outlineMat.SetFloat("_lineThickness", fromWidth);

        float timer = 0f;
        while (timer < overTime)
        {
            timer += Time.deltaTime;
            float lerpdWidth = Mathf.Lerp(fromWidth, toWidth, timer / overTime);
            outlineMat.SetFloat("_lineThickness", lerpdWidth);
            yield return null;
        }

        outlineMat.SetFloat("_lineThickness", toWidth);
    }
}
