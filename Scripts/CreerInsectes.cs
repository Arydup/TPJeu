using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Ce script s'occupe de la création d'araignées
 *
 */
public class CreerInsectes : MonoBehaviour
{
    int nbMaxAraignee = 4;      // nombre maximum d'araignées
    int nbAraignee = 0;         // nombre d'araignées présentes dans la scène
    public GameObject araignee;

    void Start()
    {
        InvokeRepeating("CreationAraignee", 10f, 12f);
    }

    // Update is called once per frame
    void Update()
    {
        // Empêche la création d'araignées s'il y en a le nombre maximum
        if(nbAraignee >= nbMaxAraignee)
        {
            CancelInvoke("CreationAraignee");
        }
    }
    
    /* Fonction qui sert à instancier des araignées
     * et leur donner une vitesse aléatoire
     */
    void CreationAraignee()
    {
        nbAraignee++;
        GameObject laCopie = Instantiate(araignee);
        laCopie.SetActive(true);
        laCopie.GetComponent<NavMeshAgent>().speed = Random.Range(1.5f, 3.5f);
    }
}
