using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.SplashScreen;


public class ShopButtonsStatus : MonoBehaviour
{
    [Tooltip("List of shop buttons prices texts.")]
    [SerializeField]
    private List<TMP_Text> shopButtonsPricesTexts = new(3);

    [Tooltip("List of shop buttons prices coins.")]
    [SerializeField]
    private List<Image> shopButtonsPricesCoins = new(3);

    [Tooltip("List of shop buttons texts.")]
    [SerializeField]
    private List<TMP_Text> shopButtonsTexts = new(4);

    private ShopController shopBehaviour;

    private readonly string _ownedRacketsKey = "OwnedRackets";

    private readonly string _racketKey = "Racket";

    private void Awake()
    {
        shopBehaviour = GetComponent<ShopController>();
        shopBehaviour.ShipEquipped.AddListener(UpdateButtonsStatus);

        if (!PlayerPrefs.HasKey(_ownedRacketsKey))
            PlayerPrefs.SetInt(_ownedRacketsKey, 1);

        if (!PlayerPrefs.HasKey(_racketKey))
            PlayerPrefs.SetInt(_racketKey, 1);
    }

    private void Start()
    {
        //Debug.Log("Current ship " + PlayerPrefs.GetInt(_racketKey, 1));
        UpdateButtonsStatus(PlayerPrefs.GetInt(_racketKey, 1));

        RacketType shipTypeIndex = (RacketType)IndexToShipType(PlayerPrefs.GetInt(_racketKey, 1));

        //Debug.Log("ShipType " + shipTypeIndex);
        //Debug.Log("ShipType " + (int)shipTypeIndex);
    }

    public int IndexToShipType(int index)
    {
        return index switch
        {
            1 => 1,
            2 => 2,
            3 => 4,
            4 => 8,
            _ => 0,
        };
    }

    private void UpdateButtonsStatus(int shipIndex)
    {
        for (int i = 0; i < shopButtonsTexts.Count; i++)
        {
            bool isRacketOwned = shopBehaviour.IsRacketOwned((RacketType)IndexToShipType(i + 1), PlayerPrefs.GetInt(_ownedRacketsKey, 1));

            //Debug.Log($"Racket {i + 1} owned status is " + isRacketOwned);

            if (i + 1 == shipIndex)
            {
                shopButtonsTexts[i].text = "Equipped";

                if (i > 0 && isRacketOwned)
                {
                    shopButtonsPricesCoins[i - 1].enabled = false;
                    shopButtonsPricesTexts[i - 1].text = "";
                }
            }
            else if (isRacketOwned && shopButtonsTexts[i].text == "Equipped")
            {
                shopButtonsTexts[i].text = "Equip";
                if (i > 0)
                {
                    shopButtonsPricesCoins[i - 1].enabled = false;
                    shopButtonsPricesTexts[i - 1].text = "";
                }
            }
            else if (i > 0 && isRacketOwned)
            {
                shopButtonsTexts[i].text = "Equip";
                shopButtonsPricesCoins[i - 1].enabled = false;
                shopButtonsPricesTexts[i - 1].text = "";
            }
        }
    }
}