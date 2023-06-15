using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartyPanelSpawn : MonoBehaviour
{
    public static PartyPanelSpawn instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        SetStats(0);

        switch (Player.instance.AllControlledCharacters.Count)
        {
            case 1:
                buttons[0].SetActive(true);
                buttons[1].SetActive(false);
                buttons[2].SetActive(false);
                break;
            case 2:
                buttons[0].SetActive(true);
                buttons[1].SetActive(true);
                buttons[2].SetActive(false);
                break;
            case 3:
                buttons[0].SetActive(true);
                buttons[1].SetActive(true);
                buttons[2].SetActive(true);
                break;
        }
    }

    [Header("Buttons")]
    [SerializeField] private List<GameObject> buttons;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI stateTxt;
    [SerializeField] private TextMeshProUGUI currentHealthTxt;
    [SerializeField] private TextMeshProUGUI maxHealthTxt;
    [SerializeField] private TextMeshProUGUI currentEtherTxt;
    [SerializeField] private TextMeshProUGUI maxEtherTxt;
    [SerializeField] private TextMeshProUGUI speedTxt;
    [SerializeField] private TextMeshProUGUI physicalDmgTxt;
    [SerializeField] private TextMeshProUGUI physicalDefTxt;
    [SerializeField] private TextMeshProUGUI magicDmgTxt;
    [SerializeField] private TextMeshProUGUI magicDefTxt;
    //[SerializeField] private TextMeshProUGUI critChanceTxt;
    //[SerializeField] private TextMeshProUGUI criteDamageTxt;
    //[SerializeField] private TextMeshProUGUI evasionTxt;
    //[SerializeField] private TextMeshProUGUI hitchanceTxt;

    public void SetStats(int i)
    {
        Character player = Player.instance.AllControlledCharacters[i];
        CharacterStats stat = player.GetComponent<CharacterStats>();

        stateTxt.text = stat.CharacterStateHandler.StartingState.ToString();
        currentHealthTxt.text = stat.Health.CurrentValue.ToString();
        maxHealthTxt.text = stat.Health.MaxValue.ToString();
        currentEtherTxt.text = stat.Ether.CurrentValue.ToString();
        maxEtherTxt.text = stat.Ether.MaxValue.ToString();
        speedTxt.text = stat.Speed.CurrentValue.ToString();
        physicalDmgTxt.text = stat.PhysicalDamage.CurrentValue.ToString();
        physicalDefTxt.text = stat.PhysicalResistance.CurrentValue.ToString();
        magicDmgTxt.text = stat.MagicalDamage.CurrentValue.ToString();
        magicDefTxt.text = stat.MagicalResistance.CurrentValue.ToString();
    }
}
