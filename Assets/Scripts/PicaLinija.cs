using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicaLinija : MonoBehaviour
{
    [Header("Duzina linije")]
    [SerializeField] private int brojSegmenata=50;
    [SerializeField] private float duzinaKrive = 3.5f;

    private Vector2[] pozicijeSegmenata;

    private LineRenderer lineRenderer;

    private float brzina;
    private float gravitacija=1f;
    private Transform pocetak;

    private void Start()
    {
        pozicijeSegmenata = new Vector2[brojSegmenata];

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = brojSegmenata;

        pocetak = PicaDrop.pocetakPucanja;
        brzina = PicaMetak.brzinaStatik;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector2 touchPozicija = Camera.main.ScreenToWorldPoint(touch.position);

            Vector2 razlika = touchPozicija - (Vector2)transform.position;
            razlika.Normalize();

            float ugao = Mathf.Atan2(razlika.y, razlika.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, -(ugao + 90));
        }

        Vector2 pocetnaPozicija = pocetak.position;
        pozicijeSegmenata[0] = pocetnaPozicija;
        lineRenderer.SetPosition(0, pocetnaPozicija);

        Vector2 pocetnaSila=transform.up*brzina;

        for (int i = 1; i < brojSegmenata; i++)
        {
            float vremeUticaj = i * Time.fixedDeltaTime * duzinaKrive;

            Vector2 gravitacijaUticaj=0.5f*Physics2D.gravity*vremeUticaj*vremeUticaj*gravitacija;

            pozicijeSegmenata[i] = pocetnaPozicija + pocetnaSila * vremeUticaj + gravitacijaUticaj;
            lineRenderer.SetPosition(i, pozicijeSegmenata[i]);
        }
    }
}
