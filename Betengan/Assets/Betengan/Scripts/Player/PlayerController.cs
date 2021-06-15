using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public int id;
    public Team team;

    private Vector2 movement;

    [Header("Power")]
    public float power = 100f;
    private bool isRegeneratePower;

    [Header("Reference")]
    public ProgressBar powerBar;
    public TextMeshPro usernameText;

    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void Initialize(int _playerId, string _username)
    {
        id = _playerId;
        usernameText.text = _username;
    }

    private void Update()
    {
        if(GameManager.instance.paused) 
        {
            if(movement != Vector2.zero)
            {
                movement = Vector2.zero;
                ClientSend.PlayerInput(movement);
            }
            return;
        }

        MovementInput();
    }

    public void MovementInput()
    {
        if(id == Client.instance.myId)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            
            ClientSend.PlayerInput(movement);
        }
    }

    public void SetPosition(Vector2 _pos)
    {
        transform.position = _pos;
    }

    public void SetScaleX(float scaleX)
    {
        Vector3 characterScale = transform.localScale;
        characterScale.x = scaleX;

        transform.localScale = characterScale;
    }

    public void SetAnimation(string _name, bool _value)
    {
        anim.SetBool(_name, _value);
    }

    public void SetPower(float _power)
    {
        power = _power;
        powerBar.SetFillAmount(_power);
    }
}