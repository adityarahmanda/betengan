using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public bool canMove = true;
    public float moveSpeed;
    private Vector2 movement;

    [Header("Power")]
    public float power = 100f;
    private bool isRegeneratePower;

    [Header("Reference")]
    public PlayerHands hands;
    public ProgressBar powerBar;

    private Rigidbody2D rb;
    private Animator anim;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(canMove)
        {
            //Character Movement 
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if(movement.sqrMagnitude > 0)
            {
                Vector3 characterScale = transform.localScale;
                if(movement.x < 0)
                {
                    characterScale.x = -1;
                }
                if(movement.x > 0)
                {
                    characterScale.x = 1;
                }
                transform.localScale = characterScale;

                anim.SetBool("isRun", true);
            }
            else
            {
                anim.SetBool("isRun", false);
            }
        }

        /*
        if(power > 0 && !isRegeneratePower)
        {
            power -= Time.deltaTime * GameManager.instance.powerDecreaseSpeed;
            if(power < 0) power = 0;

            powerBar.SetFillAmount(power);
        }

        if(power < 100 && isRegeneratePower)
        {
            power += Time.deltaTime * GameManager.instance.powerRegenerateSpeed;
            if(power > 100) power = 100;

            powerBar.SetFillAmount(power);
        }
        */
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void SetPosition(Vector2 _pos)
    {
        transform.position = _pos;
    }

    public void SetAnimation(string _name, bool _value)
    {
        anim.SetBool(_name, _value);
    }

    /*
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
    */
}