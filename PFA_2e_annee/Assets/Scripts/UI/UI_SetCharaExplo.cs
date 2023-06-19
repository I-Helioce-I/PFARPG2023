using System.Collections;
using System.Collections.Generic;
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

    private Vector2 selectedSD;
    private Vector2 unselectedSD;

    private Vector3 selectedAP;
    private Vector3 unselectedLeftAP;
    private Vector3 unselectedRightAP;
    private float speedChange;

    private Color selectedColor;
    private Color unselectedColor;

    private IEnumerator _currentSwitchingCoroutine;

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
        switch (charaSelected)
        {
            case CharaSelected.Isen:
                isenRT.sizeDelta = selectedSD;
                leaghanRT.sizeDelta = unselectedSD;
                urielRT.sizeDelta = unselectedSD;

                //isenRT.anchoredPosition = Vector3.Lerp(isenRT.anchoredPosition, selectedAP, speedChange);
                //leaghanRT.anchoredPosition = Vector3.Lerp(leaghanRT.anchoredPosition, unselectedRightAP, speedChange);
                //urielRT.anchoredPosition = Vector3.Lerp(urielRT.anchoredPosition, unselectedLeftAP, speedChange);

                //isenRT.anchoredPosition = selectedAP;
                //leaghanRT.anchoredPosition = unselectedRightAP;
                //urielRT.anchoredPosition = unselectedLeftAP;

                isenBG.color = selectedColor;
                isenMask.color = selectedColor;
                leaghanBG.color = unselectedColor;
                leaghanMask.color = unselectedColor;
                urielBG.color = unselectedColor;
                urielMask.color = unselectedColor;
                break;

            case CharaSelected.Leaghan:
                isenRT.sizeDelta = unselectedSD;
                leaghanRT.sizeDelta = selectedSD;
                urielRT.sizeDelta = unselectedSD;

                //isenRT.anchoredPosition = Vector3.Lerp(isenRT.anchoredPosition, unselectedLeftAP, speedChange);
                //leaghanRT.anchoredPosition = Vector3.Lerp(leaghanRT.anchoredPosition, selectedAP, speedChange);
                //urielRT.anchoredPosition = Vector3.Lerp(urielRT.anchoredPosition, unselectedRightAP, speedChange);

                //isenRT.anchoredPosition = unselectedLeftAP;
                //leaghanRT.anchoredPosition = selectedAP;
                //urielRT.anchoredPosition = unselectedRightAP;

                isenBG.color = unselectedColor;
                isenMask.color = unselectedColor;
                leaghanBG.color = selectedColor;
                leaghanMask.color = selectedColor;
                urielBG.color = unselectedColor;
                urielMask.color = unselectedColor;
                break;

            case CharaSelected.Uriel:
                isenRT.sizeDelta = unselectedSD;
                leaghanRT.sizeDelta = unselectedSD;
                urielRT.sizeDelta = selectedSD;

                //isenRT.anchoredPosition = Vector3.Lerp(isenRT.anchoredPosition, unselectedRightAP, speedChange);
                //leaghanRT.anchoredPosition = Vector3.Lerp(leaghanRT.anchoredPosition, unselectedLeftAP, speedChange);
                //urielRT.anchoredPosition = Vector3.Lerp(urielRT.anchoredPosition, selectedAP, speedChange);

                //isenRT.anchoredPosition = unselectedRightAP;
                //leaghanRT.anchoredPosition = unselectedLeftAP;
                //urielRT.anchoredPosition = selectedAP;

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

    private IEnumerator SwitchToChar(CharaSelected fromCharacter, CharaSelected toCharacter, float overTime)
    {
        float timer = 0f;

        while(timer < overTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    private IEnumerator SwitchRotation()
    {
        // from -> to
        //Quaternion rot = circle.transform.rotation;
        //rot = Vector3.Lerp(circle.transform.rotation, circle.transform.rotation.z + 120, speedChange);

        yield return null;
    }
}
