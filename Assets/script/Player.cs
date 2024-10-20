using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _heightJump;
    [SerializeField] private AudioClip _audioDead;
    private bool _isJump;
    private bool _isDead;
    private Animator _anim;
    private Rigidbody2D _rig;
    private AudioSource _audioJump;
    private AudioSource _audioSource;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _rig = GetComponent<Rigidbody2D>();
        _isJump = false;
        _isDead = false;
        _audioJump = GetComponent<AudioSource>();

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = _audioDead;
    }

    void Update()
    {
        Jump();
        HandleAnimation();
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.CompareTag("CMS"))
        {
            _isDead = true;
            _anim.SetBool("Run", false);
            _anim.SetTrigger("Dead");
            _audioSource.Play();
        }
        else if (target.gameObject.CompareTag("Ground"))
        {
            _isJump = false;
        }
    }

    void HandleAnimation()
    {
        _anim.SetBool("Jump", _isJump);
        _anim.SetBool("Run", !_isJump && !_isDead);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isJump && !_isDead)
        {
            _rig.velocity = new Vector2(0f, _heightJump);
            _isJump = true;
            _audioJump.Play();
        }
    }

    public bool IsDead()
    {
        return _isDead;
    }
}
