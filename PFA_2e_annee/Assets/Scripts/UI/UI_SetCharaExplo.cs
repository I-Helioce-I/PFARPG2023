using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetCharaExplo : MonoBehaviour
{
    enum CharaSelected
    {
        Isen,
        Leaghan,
        Uriel
    }

    [SerializeField][ReadOnlyInspector] private CharaSelected charaSelected;

    [SerializeField] private GameObject circle;

    [Header("Isen")]
    [SerializeField] private GameObject isenGO;
    [SerializeField] private Image isenBG;
    [SerializeField] private Image isenMask;
    [SerializeField] private RectTransform isenRT;

    [Header("Leaghan")]
    [SerializeField] private GameObject leaghanGO;
    [SerializeField] private Image leaghanBG;
    [SerializeField] private Image leaghanMask;
    [SerializeField] private RectTransform leaghanRT;

    [Header("Uriel")]
    [SerializeField] private GameObject urielGO;
    [SerializeField] private Image urielBG;
    [SerializeField] private Image urielMask;
    [SerializeField] private RectTransform urielRT;

    [Header("Lerp")]
    [SerializeField] private Vector3 endPos = new Vector3(0, 125, 0);
    [SerializeField] private Vector3 startPos = new Vector3(0, -75, 0);
    [SerializeField] private float duration = 3f;
    private float elapseTime;

    private Vector2 selectedSD;
    private Vector2 unselectedSD;

    private Vector3 selectedAP;
    private Vector3 unselectedLeftAP;
    private Vector3 unselectedRightAP;
    private float speedChange;

    private Color selectedColor;
    private Color unselectedColor;

    private IEnumerator _currentSwitchingCoroutine;

    private Quaternion newRotation;

    private void Start()
    {
        charaSelected = CharaSelected.Isen;

        isenRT = isenGO.GetComponent<RectTransform>();
        leaghanRT = leaghanGO.GetComponent<RectTransform>();
        urielRT = urielGO.GetComponent<RectTransform>();

        selectedSD = new Vector2(150, 150);
        unselectedSD = new Vector2(100, 100);

        selectedAP = new Vector3(0, 0, 0);
        unselectedLeftAP = new Vector3(-150, -125);
        unselectedRightAP = new Vector3(150, -125);

        selectedColor = new Color(1, 1, 1, 1);
        unselectedColor = new Color(0.25f, 0.25f, 0.25f, 1);
        speedChange = 1f;

        ChangeType();
    }

    public void ForwardVer()
    {
        switch (charaSelected)
        {
            case CharaSelected.Isen:
                charaSelected = CharaSelected.Leaghan;
                break;
            case CharaSelected.Leaghan:
                charaSelected = CharaSelected.Uriel;
                break;
            case CharaSelected.Uriel:
                charaSelected = CharaSelected.Isen;
                break;
            default:
                break;
        }

        ChangeType();
    }

    public void BackwardVer()
    {
        switch (charaSelected)
        {
            case CharaSelected.Isen:
                charaSelected = CharaSelected.Uriel;
                break;
            case CharaSelected.Leaghan:
                charaSelected = CharaSelected.Isen;
                break;
            case CharaSelected.Uriel:
                charaSelected = CharaSelected.Leaghan;
                break;
            default:
                break;
        }

        ChangeType();
    }

    private void ChangeType()
    {
        ChangeSizeDelta();
        ChangeAnchoredPosition();
        ChangeColor();
    }

    private void ChangeSizeDelta()
    {
        switch (charaSelected)
        {
            case CharaSelected.Isen:
                isenRT.sizeDelta = selectedSD;
                leaghanRT.sizeDelta = unselectedSD;
                urielRT.sizeDelta = unselectedSD;
                break;
            case CharaSelected.Leaghan:
                isenRT.sizeDelta = unselectedSD;
                leaghanRT.sizeDelta = selectedSD;
                urielRT.sizeDelta = unselectedSD;
                break;
            case CharaSelected.Uriel:
                isenRT.sizeDelta = unselectedSD;
                leaghanRT.sizeDelta = unselectedSD;
                urielRT.sizeDelta = selectedSD;
                break;
        }
    }

    private void ChangeAnchoredPosition()
    {
        switch (charaSelected)
        {
            case CharaSelected.Isen:
                isenRT.anchoredPosition = selectedAP;

                leaghanRT.anchoredPosition = unselectedRightAP;

                urielRT.anchoredPosition = unselectedLeftAP;

                break;
            case CharaSelected.Leaghan:
                isenRT.anchoredPosition = unselectedLeftAP;

                leaghanRT.anchoredPosition = selectedAP;

                urielRT.anchoredPosition = unselectedRightAP;

                break;
            case CharaSelected.Uriel:
                isenRT.anchoredPosition = unselectedRightAP;

                leaghanRT.anchoredPosition = unselectedLeftAP;

                urielRT.anchoredPosition = selectedAP;

                break;
            default:
                break;
        }
    }

    private void ChangeColor()
    {
        switch (charaSelected)
        {
            case CharaSelected.Isen:
                isenBG.color = selectedColor;
                isenMask.color = selectedColor;

                leaghanBG.color = unselectedColor;
                leaghanMask.color = unselectedColor;

                urielBG.color = unselectedColor;
                urielMask.color = unselectedColor;

                break;
            case CharaSelected.Leaghan:
                isenBG.color = unselectedColor;
                isenMask.color = unselectedColor;

                leaghanBG.color = selectedColor;
                leaghanMask.color = selectedColor;

                urielBG.color = unselectedColor;
                urielMask.color = unselectedColor;

                break;
            case CharaSelected.Uriel:
                isenBG.color = unselectedColor;
                isenMask.color = unselectedColor;

                leaghanBG.color = unselectedColor;
                leaghanMask.color = unselectedColor;

                urielBG.color = selectedColor;
                urielMask.color = selectedColor;

                break;
            default:
                break;
        }
    }
}
