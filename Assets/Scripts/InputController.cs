using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private RacketController _racketController;

    [SerializeField]
    private TouchDetection _touchDetection;

    [SerializeField]
    private Camera mainCamera;

    //private float _touchSpeed = 2E+1f;

    [SerializeField]
    private HitSoundBehaviour _hitSoundBehaviour;

    private bool _isActive = true;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    private ScreenThreshold _screenThreshold;

    private struct ScreenThreshold
    {
        public float PaddingX { get; private set; }
        public float PaddingY { get; private set; }

        public float MinX { get; private set; }
        public float MaxX { get; private set; }
        public float MinY { get; private set; }
        public float MaxY { get; private set; }

        public ScreenThreshold(float paddingX, float paddingY)
        {
            PaddingX = paddingX;
            PaddingY = paddingY;

            MinX = Camera.main.ViewportToWorldPoint(new Vector3(paddingX, 0, 0)).x;
            MaxX = Camera.main.ViewportToWorldPoint(new Vector3(1 - paddingX, 0, 0)).x;
            MinY = Camera.main.ViewportToWorldPoint(new Vector3(0, paddingY, 0)).y;
            MaxY = Camera.main.ViewportToWorldPoint(new Vector3(0, paddingY + 0.1f, 0)).y;
        }
    }

    private void Awake()
    {
        _screenThreshold = new ScreenThreshold(0.2f, 0.1f);
    }

    private void Update()
    {
        if (_isActive && _touchDetection.TouchDetected)
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                DetectTouch();
            }
        }
    }

    private void DetectTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector3 touchPosition = mainCamera.ScreenToWorldPoint(touch.position);

            touchPosition.x = Mathf.Clamp(touchPosition.x, _screenThreshold.MinX, _screenThreshold.MaxX);
            touchPosition.y = Mathf.Clamp(touchPosition.y, _screenThreshold.MinY, _screenThreshold.MaxY);

            _racketController.RacketPosition = new Vector3(touchPosition.x, touchPosition.y, 0);
        }
    }
}
