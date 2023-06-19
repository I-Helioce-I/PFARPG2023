using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSpawn : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField] private List<GameObject> characterInfo;
    [SerializeField] private List<Image> sprites;
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> levels;
    [SerializeField] private List<TextMeshProUGUI> currentHealths;
    [SerializeField] private List<TextMeshProUGUI> maxHealths;
    [SerializeField] private List<TextMeshProUGUI> currentEther;
    [SerializeField] private List<TextMeshProUGUI> maxEther;

    private void OnEnable()
    {
        switch (Player.instance.AllControlledCharacters.Count)
        {
            case 1:
                SetStats(0);

                characterInfo[0].SetActive(true);
                characterInfo[1].SetActive(false);
                characterInfo[2].SetActive(false);
                break;
            case 2:
                SetStats(0);
                SetStats(1);

                characterInfo[0].SetActive(true);
                characterInfo[1].SetActive(true);
                characterInfo[2].SetActive(false);
                break;
            case 3:
                SetStats(0);
                SetStats(1);
                SetStats(2);

                characterInfo[0].SetActive(true);
                characterInfo[1].SetActive(true);
                characterInfo[2].SetActive(true);
                break;
        }
    }

    private void SetStats(int i)
    {
        Character player = Player.instance.AllControlledCharacters[i];
        CharacterStats stat = player.GetComponent<CharacterStats>();

        sprites[i].sprite = Player.instance.AllControlledCharacters[i].sprite;
        names[i].text = player.charaName;
        levels[i].text = stat.Level.ToString();
        currentHealths[i].text = stat.Health.CurrentValue.ToString();
        maxHealths[i].text = " / " + stat.Health.MaxValue.ToString();
        currentEther[i].text = stat.Ether.CurrentValue.ToString();
        maxEther[i].text = " / " + stat.Ether.MaxValue.ToString();
    }
}
