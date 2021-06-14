using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int id;
    public Team team;

    private Vector3 lastPosition;

    [Header("Movement")]
    public float moveSpeed;
    private Vector2 movement;

    private bool isFacingRight;
    private bool isRunning;

    [Header("Power")]
    public float power = 100f;
    private bool isRegeneratePower;

    [Header("Reference")]
    public PlayerHands hands;
    private Rigidbody2D rb;
    
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        lastPosition = transform.position;
    }

    public void Initialize(int _playerId)
    {
        id = _playerId;
    }

    private void Update()
    {
        if(movement.sqrMagnitude > 0)
        {
            if(movement.x < 0 && isFacingRight)
            {
                ServerSend.PlayerScaleX(id, -1);
                isFacingRight = false;
            }

            if(movement.x > 0 && !isFacingRight)
            {
                ServerSend.PlayerScaleX(id, 1);
                isFacingRight = true;
            }

            if(!isRunning) 
            {                
                isRunning = true;
                ServerSend.PlayerAnimation(id, "isRun", true);
            }
        }
        else
        {
            if(isRunning) 
            {                
                isRunning = false;
                ServerSend.PlayerAnimation(id, "isRun", false);
            }
        }

        if(power > 0 && !isRegeneratePower)
        {
            power -= Time.deltaTime * GameManager.instance.powerDecreaseSpeed;
            if(power < 0) power = 0;

            ServerSend.PlayerPower(id, power);
        }

        if(power < 100 && isRegeneratePower)
        {
            power += Time.deltaTime * GameManager.instance.powerRegenerateSpeed;
            if(power > 100) power = 100;

            ServerSend.PlayerPower(id, power);
        }
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        if(transform.position != lastPosition)
        {
            ServerSend.PlayerPosition(id, transform.position);
            lastPosition = transform.position;
        }
    }

    public void MovementInput(Vector2 _movementInput)
    {
        movement = _movementInput;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(team == Team.RedTeam && other.gameObject.tag == "Red Team Base")
        {
            isRegeneratePower = true;
        }

        if(team == Team.BlueTeam && other.gameObject.tag == "Blue Team Base")
        {
            isRegeneratePower = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(team == Team.RedTeam && other.gameObject.tag == "Red Team Base")
        {
            isRegeneratePower = false;
        }
    
        if(team == Team.BlueTeam && other.gameObject.tag == "Blue Team Base")
        {
            isRegeneratePower = false;
        }
    }
}