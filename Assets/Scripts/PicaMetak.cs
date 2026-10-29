using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicaMetak : MonoBehaviour
{
    private Rigidbody2D rb;
    public float brzina;
    public static float brzinaStatik;

    private bool desno;

    private bool udario=true;

    private bool opalio=false;

    private bool pritisnuo = false;

    private float brzinaRotacije = 200f;

    public bool stvoren=false;

    private Transform transformacija;
    private float pocetnaVelicina = 0.45f;
    private float krajnjaVelicina = 1.25f;
    private float ukupnoVreme=1f;
    private float trenutnoVreme=0f;

    private int broj = 0;

    private bool falling = false;
    private void Start()
    {
        switch (gameObject.name)
        {
            case "Pica1(Clone)":
                broj = 0;
                break;
            case "Pica2(Clone)":
                broj = 1;
                break;
            case "Pica3(Clone)":
                broj = 2;
                break;
            case "Pica4(Clone)":
                broj = 3;
                break;
            case "Pica5(Clone)":
                broj = 4;
                break;
            case "Pica6(Clone)":
                broj = 5;
                break;
            case "Pica7(Clone)":
                broj = 6;
                break;
            case "Pica8(Clone)":
                broj = 7;
                break;
            case "Pica9(Clone)":
                broj = 8;
                break;
            case "Pica10(Clone)":
                broj = 9;
                break;
            default:
                break;
        }

        brzinaStatik = brzina;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        int random = UnityEngine.Random.Range(0, 2);
        if(random==0)
        {
            desno = true;
        }
        else
        {
            desno = false;
        }

        transformacija = transform;
        if (stvoren)
        {
            opalio = true;
            rb.gravityScale = 1;

            rb.linearVelocity = new Vector2(0,1) * (brzina / 2);
        }
        else
        {
            transformacija.localScale = new Vector3(pocetnaVelicina, pocetnaVelicina, 1);
        }
    }

    private void Update()
    {
        if (!opalio && !PicaDrop.krajIgre)
        {
            RacunanjeUgla();
        }

        if (!udario && !stvoren)
        {
            Rotacija();
        }

        if(Input.touchCount==0 && !opalio && pritisnuo && !PicaDrop.krajIgre)
        {
            opalio = true;
            Opali();
        }

        if (!stvoren && opalio && !udario && trenutnoVreme<ukupnoVreme)
        {
            trenutnoVreme += Time.deltaTime;

            float proporcija=Mathf.Clamp01(trenutnoVreme/ukupnoVreme);

            float velicina = Mathf.Lerp(pocetnaVelicina, krajnjaVelicina, proporcija);

            transformacija.localScale = new Vector3(velicina, velicina, 1);
        }
    }

    public void RacunanjeUgla()
    {
        if(Input.touchCount>0)
        {
            pritisnuo = true;

            Touch touch = Input.GetTouch(0);

            Vector2 touchPozicija = Camera.main.ScreenToWorldPoint(touch.position);

            Vector2 razlika = touchPozicija - (Vector2)transform.position;
            razlika.Normalize();

            float ugao = Mathf.Atan2(razlika.y, razlika.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, -(ugao + 90));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        udario = true;

        if (collision.gameObject.name==gameObject.name)
        {

            PicaDrop.skor += (broj + 2)*2;

            Vector2 pozicija = new Vector2((collision.transform.position.x+gameObject.transform.position.x)/2, (collision.transform.position.y + gameObject.transform.position.y) / 2);

            PicaDrop.stvoriBroj = new Tuple<int, Vector2>(broj+1, pozicija);

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "ProveraL" || collision.gameObject.name == "ProveraD")
        {
            falling = true;
        }
        else if (falling)
        {
            PicaDrop.krajIgre = true;
        }
    }

    private void Rotacija()
    {
        if(desno)
        {
            transform.Rotate(0, 0, -brzinaRotacije * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, brzinaRotacije*Time.deltaTime);
        }
    }

    public void Opali()
    {
        PicaDrop.skor += broj + 1;

        rb.linearVelocity = transform.up * brzina;
        rb.gravityScale = 1;

        udario = false;

        PicaDrop.mozePucati = true;
        PicaDrop.cekanje = false;
        PicaDrop.gasi = true;
    }
}
