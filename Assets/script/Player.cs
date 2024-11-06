using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _heightJump;

    [SerializeField]
    private AudioClip _audioDead;

    [SerializeField]
    private AudioClip[] _jumpSounds;

    private bool _isJump;

    private bool _isDead;

    private Animator _anim;

    private Rigidbody2D _rig;

    private AudioSource _audioSource;

    private int _jumpCounter;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rig = GetComponent<Rigidbody2D>();
        _isJump = false;
        _isDead = false;
        _audioSource = base.gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_isDead)
        {
            Jump();
        }
        HandleAnimation();
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.CompareTag("CMS"))
        {
            _isDead = true;
            _anim.SetBool("Run", value: false);
            _anim.SetTrigger("Dead");
            PlayDeathSound();
        }
        else if (target.gameObject.CompareTag("Ground"))
        {
            _isJump = false;
        }
    }

    private void HandleAnimation()
    {
        _anim.SetBool("Jump", _isJump);
        _anim.SetBool("Run", !_isJump && !_isDead);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isJump)
        {
            _rig.velocity = new Vector2(0f, _heightJump);
            _isJump = true;
            if (_jumpCounter == 3)
            {
                _audioSource.clip = _jumpSounds[1];
            }
            else
            {
                _audioSource.clip = _jumpSounds[0];
            }
            _audioSource.Play();
            _jumpCounter = (_jumpCounter + 1) % 4;
        }
    }

    private void PlayDeathSound()
    {
        _audioSource.clip = _audioDead;
        _audioSource.Play();
    }

    public bool IsDead()
    {
        return _isDead;
    }
}

