using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallsBorder : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent GameLose;

    private GameObject _ball;

    private void Awake()
    {
        GameLose = new UnityEvent();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            GameLose.Invoke();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Destroy(collision.gameObject);
        }
    }
}
