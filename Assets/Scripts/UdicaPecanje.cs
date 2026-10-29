using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UdicaPecanje : MonoBehaviour
{
    public SpriteRenderer riba1;
    public SpriteRenderer riba2;
    public SpriteRenderer riba3;
    public SpriteRenderer riba4;
    public SpriteRenderer riba5;

    private int slikaRibe = 1;

    private Camera mainCamera;
    private Pecanje pecanje;
    private ParticleSystem mehuri;
    private ParticleSystem mehuriZlatni;

    private void Start()
    {
        mainCamera = Camera.main;
        pecanje = mainCamera.GetComponent<Pecanje>();
        int brojDece = GetComponent<Transform>().childCount;
        GameObject mehuriObjekat=gameObject.transform.GetChild(brojDece-2).gameObject;
        mehuri = mehuriObjekat.GetComponent<ParticleSystem>();
        GameObject mehuriZlatniObjekat = gameObject.transform.GetChild(brojDece-1).gameObject;
        mehuriZlatni = mehuriZlatniObjekat.GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="misolovka")
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Animator>().SetTrigger("kraj");
            pecanje.kraj=true;
        }
        else if(collision.gameObject.tag=="sir")
        {
            mehuriZlatni.Play();
            pecanje.skor += 10;
            
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag=="krug")
        {
            mehuri.Play();
            pecanje.skor += 5;
            switch (slikaRibe)
            {
                case 1:
                    riba1.enabled = true;
                    riba1.sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    slikaRibe++;
                    break;
                case 2:
                    riba2.enabled = true;
                    riba2.sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    slikaRibe++;
                    break;
                case 3:
                    riba3.enabled = true;
                    riba3.sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    slikaRibe++;
                    break;
                case 4:
                    riba4.enabled = true;
                    riba4.sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    slikaRibe++;
                    break;
                case 5:
                    riba5.enabled = true;
                    riba5.sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    slikaRibe = 1;
                    break;
                default:
                    break;
            }
            Destroy(collision.gameObject);
        }
        else
        {
            mehuri.Play();
            pecanje.skor += 1;
            switch (slikaRibe)
            {
                case 1:
                    riba1.enabled = true;
                    riba1.sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    slikaRibe++;
                    break;
                case 2:
                    riba2.enabled = true;
                    riba2.sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    slikaRibe++;
                    break;
                case 3:
                    riba3.enabled = true;
                    riba3.sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    slikaRibe++;
                    break;
                case 4:
                    riba4.enabled = true;
                    riba4.sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    slikaRibe++;
                    break;
                case 5:
                    riba5.enabled = true;
                    riba5.sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
                    slikaRibe = 1;
                    break;
                default:
                    break;
            }
            Destroy(collision.gameObject);
        }
    }
}
