using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class KisaMeteora : MonoBehaviour
{
    private ParticleSystem plavi;
    private ParticleSystem zuti;
    private ParticleSystem crveni;
    private ParticleSystem narandzasti;
    private ParticleSystem ljubicasti;
    private ParticleSystem zeleni;
    private ParticleSystem crni;
    private string tagObjekta = "";
    private float brzina = 6f;
    private int rot=0;
    private bool unisten = false;

    private ParticleSystem particle;

    private void Start()
    {
        tagObjekta = gameObject.tag;
        rot = Random.Range(0, 2);

        plavi=Level5.plaviEfekat;
        zuti=Level5.zutiEfekat;
        crveni=Level5.crveniEfekat;
        narandzasti=Level5.narandzastiEfekat;
        ljubicasti=Level5.ljubicastiEfekat;
        zeleni=Level5.zeleniEfekat;
        crni=Level5.crniEfekat;
    }

    private void Update()
    {
        if(Level5.kisaMeteora)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - brzina * Time.deltaTime);

            if (rot == 1)
            {
                gameObject.transform.Rotate(new Vector3(0, 0, brzina * 30 * Time.deltaTime));
            }
            else
            {
                gameObject.transform.Rotate(new Vector3(0, 0, -brzina * 30 * Time.deltaTime));
            }

            if (tagObjekta == "crveno" && Level5.pritisnuoCrveno && !unisten)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                particle = Instantiate(crveni, gameObject.transform.position, Quaternion.identity);
                particle.Play();
                unisten = true;

                Level5.skor++;
            }
            else if(tagObjekta=="plavo" && Level5.pritisnuoPlavo && !unisten)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                particle = Instantiate(plavi, gameObject.transform.position, Quaternion.identity);
                particle.Play();
                unisten = true;

                Level5.skor++;
            }
            else if(tagObjekta=="zuto" && Level5.pritisnuoZuto && !unisten)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                particle = Instantiate(zuti, gameObject.transform.position, Quaternion.identity);
                particle.Play();
                unisten = true;

                Level5.skor++;
            }
            else if(tagObjekta=="zeleno" && Level5.pritisnuoZuto && Level5.pritisnuoPlavo && !unisten)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                particle = Instantiate(zeleni, gameObject.transform.position, Quaternion.identity);
                particle.Play();

                unisten = true;

                Level5.skor++;
            }
            else if(tagObjekta == "ljubicasto" && Level5.pritisnuoCrveno && Level5.pritisnuoPlavo && !unisten)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                particle = Instantiate(ljubicasti, gameObject.transform.position, Quaternion.identity);
                particle.Play();

                unisten = true;

                Level5.skor++;
            }
            else if(tagObjekta == "narandzasto" && Level5.pritisnuoCrveno && Level5.pritisnuoZuto && !unisten)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                particle = Instantiate(narandzasti, gameObject.transform.position, Quaternion.identity);
                particle.Play();

                unisten = true;

                Level5.skor++;
            }

            if(unisten)
            {
                particle.transform.parent = Level5.efektiHolderStatik.transform;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "kraj" && Level5.kisaMeteora)
        {
            if (gameObject.tag == "trougao")
            {
                if (gameObject.name == "PlaviTrougaoMeteor(Clone)")
                {
                    particle = Instantiate(plavi, gameObject.transform.position, Quaternion.identity);
                    particle.Play();
                }
                if (gameObject.name == "CrveniTrougaoMeteor(Clone)")
                {
                    particle = Instantiate(crveni, gameObject.transform.position, Quaternion.identity);
                    particle.Play();
                }
                if (gameObject.name == "ZutiTrougaoMeteor(Clone)")
                {
                    particle = Instantiate(zuti, gameObject.transform.position, Quaternion.identity);
                    particle.Play();
                }
                if (gameObject.name == "LjubicastiTrougaoMeteor(Clone)")
                {
                    particle = Instantiate(ljubicasti, gameObject.transform.position, Quaternion.identity);
                    particle.Play();
                }
                if (gameObject.name == "ZeleniTrougaoMeteor(Clone)")
                {
                    particle = Instantiate(zeleni, gameObject.transform.position, Quaternion.identity);
                    particle.Play();
                }
                if (gameObject.name == "NarandzastiTrougaoMeteor(Clone)")
                {
                    particle = Instantiate(narandzasti, gameObject.transform.position, Quaternion.identity);
                    particle.Play();
                }

                particle.transform.parent = Level5.efektiHolderStatik.transform;

                Level5.skor++;
            }
            else
            {
                if(!unisten)
                {
                    particle = Instantiate(crni, gameObject.transform.position, Quaternion.identity);
                    particle.Play();

                    particle.transform.parent = Level5.efektiHolderStatik.transform;

                    unisten = true;
                }
            }

            Destroy(gameObject);
        }
    }
}
