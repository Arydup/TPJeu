using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* Ce script s'occupe de la scène d'introduction
 *
 */
public class Introduction : MonoBehaviour
{
    public GameObject joueurSon;        // Le gameobject qui contient l'AudioSource
    public AudioClip sonSelection;  // Son de clic
    public Text avertissement;
    public InputField nom;          
    public static string NomJoueur; // le nom du joueur, est appelé à la fin du jeu

    public void ChangerScene()
    {
        // Si le joueur a entré une valeur dans l'InputField
        if (nom.text != "")
        {
            // Change la scène et enregistre le nom du joueur
            NomJoueur = nom.text;
            SceneManager.LoadScene("Jeu");
        }
        else
        {
            // Active le message d'avertissement
            avertissement.gameObject.SetActive(true);
        }
    }

    /* Fonction qui joue un son lorsqu'on clique sur les boutons */
    public void JouerSon()
    {
        joueurSon.GetComponent<AudioSource>().PlayOneShot(sonSelection);
    }

    public void Quitter()
    {
        Application.Quit();
    }
}
