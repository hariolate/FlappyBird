using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{
    public float tapForce = 10;
    public float tiltSmooth = 5;

    public Vector3 startPos;
    private Rigidbody2D _rigidbody;

    private Quaternion _downRotation;
    private Quaternion _forwardRotation;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _downRotation = Quaternion.Euler(0, 0, -90);
        _forwardRotation = Quaternion.Euler(0, 0, 35);
    }

    private void Update()
    {
        if (gameManager.GameOver) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            transform.rotation = _forwardRotation;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, _downRotation, tiltSmooth * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ScoreZone"))
        {
            // Score Event
            OnPlayerScored?.Invoke();
            // Play a Sound
        }
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            _rigidbody.simulated = false;
            OnPlayerDied?.Invoke();
            // Dead Event
        }
    }

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    private void OnGameStarted()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.simulated = true;
    }

    private void OnGameOverConfirmed()
    {
        var myTransform = transform;
        myTransform.localPosition = startPos;
        myTransform.rotation = Quaternion.identity;
    }

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    GameManager gameManager => GameManager.instance;
}
