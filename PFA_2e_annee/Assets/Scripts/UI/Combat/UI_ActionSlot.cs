using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ActionSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void ActionEvent(ActionDescription action);
    public event ActionEvent ActionSet = null;
    public event ActionEvent ActionRemoved = null;
    public event ActionEvent ActionSelected = null;

    [Header("Action")]
    [SerializeField] private ActionDescription Action;
    public ActionDescription GetAction
    {
        get
        {
            return Action;
        }
    }

    [Header("UI Objects")]
    public Button Button;
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Damage;
    [SerializeField] private Image Mask;

    [Header("Parameters")]
    [SerializeField] private float UnfoldTime = .3f;

    private bool _unfold = false;
    private float _unfoldTimer = 99f;
    private float _originalMaskFillValue = 0f;
    private GameObject _currentSelected;

    private void Awake()
    {
        if (Action != null)
        {
            SetAction(Action);
        }

        Mask.fillAmount = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Unfold();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Fold();
    }

    private void Update()
    {
        if (_unfoldTimer < UnfoldTime)
        {
            _unfoldTimer += Time.deltaTime;

            float interpolation = 0f;
            if (_unfold)
            {
                interpolation = Mathf.Lerp(_originalMaskFillValue, 1f, _unfoldTimer / UnfoldTime);
                
            }
            else
            {
                interpolation = Mathf.Lerp(_originalMaskFillValue, 0f, _unfoldTimer / UnfoldTime);
            }
            Mask.fillAmount = interpolation;
        }
    }

    public void SetAction(ActionDescription action)
    {
        if (action == null) return;

        Action = action;
        Icon.sprite = action.Icon;
        Icon.color = action.IconColor;
        Name.text = action.Name;

        string minDamage = action.MinMaxDamage.x.ToString();
        string maxDamage = action.MinMaxDamage.y.ToString();
        string damageType = action.TypeOfDamage.ToString();
        string actionNumber = action.NumberOfTimes.ToString();

        Damage.text = actionNumber + " x " + minDamage + "-" + maxDamage + " " + damageType;

        ActionSet?.Invoke(action);
    }

    public void RemoveAction()
    {
        ActionDescription action = Action;
        Action = null;
        ActionRemoved?.Invoke(action);
    }

    public void Unfold()
    {
        if (_unfold) return;
        _unfold = true;
        _unfoldTimer = 0f;
        _originalMaskFillValue = Mask.fillAmount;
    }

    public void Fold()
    {
        if (!_unfold) return;
        _unfold = false;
        _unfoldTimer = 0f;
        _originalMaskFillValue = Mask.fillAmount;
    }

    public void ForceFold()
    {
        _unfold = false;
        _unfoldTimer = 99f;
        _originalMaskFillValue = 0f;
        Mask.fillAmount = 0f;
    }

    public void ActionSelect()
    {
        ActionSelected?.Invoke(Action);
    }
}
