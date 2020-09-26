using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{
    public float tapForce = 10;
    public float tiltSmooth = 5;

    public Vector3 startPos;
    new Rigidbody2D rigidbody;

    Quaternion downRotation;
    Quaternion forwardRoation;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRoation = Quaternion.Euler(0, 0, 35);
    }

    private void Update()
    {
        if (!gameManager.GameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                transform.rotation = forwardRoation;
                rigidbody.velocity = Vector3.zero;
                rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ScoreZone"))
        {
            // Score Event
            OnPlayerScored();
            // Play a Sound
        }
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            rigidbody.simulated = false;
            OnPlayerDied();
            // Dead Event
        }
    }

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    void OnGameStarted()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;
    }

    void OnGameOverComfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverComfirmed;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverComfirmed;
    }

    GameManager gameManager {
        get
        {
            return GameManager.Instance;
        }
    }
}
