using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrvoSankanje : MonoBehaviour
{
    public Vector2 pocetnaVelicina = new Vector2(0.1f, 0.1f);
    public Vector2 krajnjaVelicina = new Vector2(10f, 10f);
    public float krajnjaPozicija = -9f;
    public float krajnjaPozicijaZaVelicinu=-2f;
    public float duzinaPuta = 5f;
    public float brzinaPutaUbrzana=2.5f;
    private float prosloVreme = 0f;
    
    private bool dosloDoKraja = false;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = Sankanje.orderInLayer;
    }

    void Update()
    {
        prosloVreme += Time.deltaTime;

        float trenutnaYpozicija;
        if(dosloDoKraja)
        {
            trenutnaYpozicija = Mathf.Lerp(transform.position.y, krajnjaPozicija, brzinaPutaUbrzana*Time.deltaTime);
        }
        else
        {
            float faktor = prosloVreme / duzinaPuta;
            float eksponencijalniFaktor = Mathf.Pow(faktor, 2);

            trenutnaYpozicija = Mathf.Lerp(0.3f, krajnjaPozicijaZaVelicinu, eksponencijalniFaktor);
        }

        transform.position = new Vector2(transform.position.x,trenutnaYpozicija);

        if (trenutnaYpozicija>krajnjaPozicijaZaVelicinu)
        {
            float scaleLerpFactor = Mathf.InverseLerp(0.3f, krajnjaPozicijaZaVelicinu, trenutnaYpozicija);
            transform.localScale = Vector2.Lerp(pocetnaVelicina, krajnjaVelicina, scaleLerpFactor);
        }
        else if(!dosloDoKraja)
        {
            transform.localScale = krajnjaVelicina;
            Camera.main.GetComponent<Sankanje>().skor++;
            duzinaPuta /=2f;
            dosloDoKraja = true;
        }

        if (trenutnaYpozicija <= krajnjaPozicija+0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Camera.main.GetComponent<Sankanje>().Kraj();
        }
    }
}
