using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RacketController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _racketpositionInfo;

    [SerializeField]
    private MoneyCounter _moneyCounter;

    [SerializeField]
    private ScoreCounter _scoreCounter;

    [SerializeField]
    private HitSoundBehaviour _playGameSounds;

    [SerializeField]
    private InputController _inputController;

    [SerializeField]
    private List<Sprite> _racketSprites = new(5);

    [SerializeField]
    private SpriteRenderer _racketImage;

    private readonly string _racketKey = "Racket";

    private Vector3 _racketPosition;

    public Vector3 RacketPosition { get => _racketPosition; set => _racketPosition = value; }

    private float previousSign;

    [HideInInspector]
    public UnityEvent<GameObject> IsBallHit;

    private void Awake()
    {
        IsBallHit = new UnityEvent<GameObject>();

        if (!PlayerPrefs.HasKey(_racketKey))
            PlayerPrefs.SetInt(_racketKey, 1);

        SetRacketImage();
    }

    private void Start()
    {
        _racketPosition = transform.position;
        previousSign = Mathf.Sign(_racketPosition.x);
    }

    private void Update()
    {
        transform.position = _racketPosition;

        if (Mathf.Sign(_racketPosition.x) != Mathf.Sign(previousSign))
        {
            transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, -transform.rotation.z, transform.rotation.w);
        }

        previousSign = Mathf.Sign(_racketPosition.x);

        if (_racketpositionInfo != null)
            _racketpositionInfo.text = transform.position.ToString();
    }

    private void SetRacketImage()
    {
        int ballIndex = PlayerPrefs.GetInt(_racketKey) - 1;
        _racketImage.sprite = _racketSprites[ballIndex];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Coin"))
            {
                _playGameSounds.PlayCoinSound();
                _moneyCounter.IncreaseMoney();
                Destroy(collision.gameObject);

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                _playGameSounds.PlayHitSound();
                _scoreCounter.IncreaseScore();
                Destroy(collision.gameObject, 1.0f);
            }
        }
    }
}
