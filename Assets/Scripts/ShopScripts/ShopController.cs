using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    [Tooltip("List of racket select/buy buttons.")]
    [SerializeField]
    private List<Button> buttons = new(4);

    [Tooltip("List of rackets prices.")]
    [SerializeField]
    private List<int> _racketsPrices = new(3) { 50, 200, 500 };

    [SerializeField]
    private GameObject buyPanel;

    [SerializeField]
    private TMP_Text _buyPanelText;

    [SerializeField]
    private Button agreeBuyButton;

    [SerializeField]
    private Button disagreeBuyButton;

    private readonly string _ownedRacketsKey = "OwnedRackets";

    private readonly string _racketKey = "Racket";

    private readonly string _moneyKey = "Money";

    public UnityEvent<int> ShipEquipped;

    private void Awake()
    {
        //// DEBUG !!!
        //PlayerPrefs.SetInt(_ownedRacketsKey, 1);
        //PlayerPrefs.SetInt(_racketKey, 1);
        //PlayerPrefs.SetInt(_moneyKey, 0);

        ShipEquipped = new UnityEvent<int>();

        if (!PlayerPrefs.HasKey(_ownedRacketsKey))
            PlayerPrefs.SetInt(_ownedRacketsKey, 1);

        if (!PlayerPrefs.HasKey(_racketKey))
            PlayerPrefs.SetInt(_racketKey, 1);

        if (!PlayerPrefs.HasKey(_moneyKey))
            PlayerPrefs.SetInt(_moneyKey, 0);

        for (int i = 0; i < buttons.Count; i++)
        {
            int buttonIndex = i + 1;
            buttons[i].onClick.AddListener(delegate {

                //ToggleAllButtons(buttons, false);
                CheckUsability(buttonIndex);

            });
        }
    }


    public bool IsRacketOwned(RacketType racketType, int ownedRackets)
    {
        return ((ownedRackets & (int)racketType) == (int)racketType);
    }

    private void CheckUsability(int racket)
    {
        _buyPanelText.text = "You agree to buy this item?";

        int ownedRackets = PlayerPrefs.GetInt(_ownedRacketsKey, 1);

        bool isRacketOwned = IsRacketOwned((RacketType)IndexToRacketType(racket), ownedRackets);

        //Debug.Log($"Racket {racket} owned status is " + isRacketOwned);

        if (isRacketOwned)
        {
            Equip(racket);
        }
        else
        {
            buyPanel.SetActive(true);

            agreeBuyButton.onClick.RemoveAllListeners();
            agreeBuyButton.onClick.AddListener(() => BuyAgree((RacketType)IndexToRacketType(racket), racket, ownedRackets));

            disagreeBuyButton.onClick.AddListener(BuyDisagree);
        }
    }

    private void Equip(int racketIndex)
    {
        // For text change to "Equipped"
        ShipEquipped.Invoke(racketIndex);

        PlayerPrefs.SetInt(_racketKey, racketIndex);

        //ToggleAllButtons(buttons, true);
    }

    private void Update()
    {
        //Debug.Log("Current money " + PlayerPrefs.GetInt(_moneyKey, 0));
    }

    private void BuyAgree(RacketType racket, int racketIndex, int ownedRackets)
    {
        if (_racketsPrices[racketIndex - 2] <= PlayerPrefs.GetInt(_moneyKey, 0))
        {
            //Debug.Log("Current money " + PlayerPrefs.GetInt(_moneyKey, 0));

            int moneyCount = PlayerPrefs.GetInt(_moneyKey, 0) - _racketsPrices[racketIndex - 2];

            //Debug.Log("Result money " + moneyCount);

            PlayerPrefs.SetInt(_moneyKey, moneyCount);

            PlayerPrefs.SetInt(_racketKey, racketIndex);

            ownedRackets |= (int)racket;

            PlayerPrefs.SetInt(_ownedRacketsKey, ownedRackets);

            //ToggleAllButtons(buttons, true);

            ShipEquipped.Invoke(racketIndex);

            buyPanel.SetActive(false);
        }
        else
        {
            _buyPanelText.text = "You have not money!";
        }
    }

    private void BuyDisagree()
    {
        //ToggleAllButtons(buttons, true);
        buyPanel.SetActive(false);
    }

    private void ToggleAllButtons(List<Button> buttonsList, bool toggleOn = true)
    {
        if (toggleOn)
            buttonsList.ForEach(button => button.enabled = true);
        else
            buttonsList.ForEach(button => button.enabled = false);
    }

    private void OnDestroy()
    {
        ShipEquipped.RemoveAllListeners();
    }

    public int IndexToRacketType(int index)
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
}

[Flags]
public enum RacketType
{
    // Rackets 4321
    // Mask  1111
    None = 0,
    RacketOne = 1 << 0,
    RacketTwo = 1 << 1,
    RacketThree = 1 << 2,
    RacketFour = 1 << 3,
}