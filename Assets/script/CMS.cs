using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMS : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    void Start()
    {
        
    }
    void Move() 
    {
        Vector3 pos = transform.position;
        pos.x -= Time.deltaTime * _speed;
        if (pos.x < -12f)
            Destroy(gameObject);
        transform.position = pos;
    }
    void Update()
    {
        Move();
    }
    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.CompareTag("Player"))
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
