using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sir : MonoBehaviour
{
    public float upForce = 5f;
    public float sideForce = 2f;
    public float rotacija = 150f;

    private float rotationSpeed;
    public bool kraj=false;
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 force = new Vector2(Random.Range(-sideForce, sideForce), upForce);
        rb.AddForce(force, ForceMode2D.Impulse);
        rotationSpeed = Random.Range(-rotacija,rotacija);
    }
    void Update()
    {
        if (!kraj)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }
    public void Pogodak()
    {
        kraj = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Play();
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }
}
