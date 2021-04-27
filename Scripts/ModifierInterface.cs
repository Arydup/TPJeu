using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/* Ce script sert à gérer le UI pour ouvrir et fermer le journal
 */
public class ModifierInterface : MonoBehaviour
{
    public static Creature ChoixJoueur;     // Le choix de créature du joueur
    public bool jeuEnPause;                 // Si le jeu est en pause ou non
    public GameObject Journal;              // L'objet du journal
    public GameObject interaction;          // l'AudioSource qui joue les sons
    public GameObject texteJournal;         // Le texte qui dit d'ouvrir le journal
    public AudioClip SonOuvrir;             // Un son d'ouverture pour le liver
    public AudioClip SonTournerPage;        // Un son pour tourner les pages
    
    public Toggle ToggleLivre;              
    public Toggle ToggleOrbes;
    public Toggle ToggleAraignee;
    public Toggle ToggleTraces;
    public Toggle TogglePortail;

    void Start()
    {
        jeuEnPause = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Ouvre et ferme le journal avec J
        if (Input.GetKeyDown(KeyCode.J))
        {
            interaction.GetComponent<AudioSource>().PlayOneShot(SonOuvrir);
            ActiverJournal();
        }
        // Si le joueur n'a plus de santé mentale, la partie est perdu
        if (EtatCreature.santeMentale <= 0)
        {
            FinirJeu();
        }
    }

    /* Cette fonction active et désactive le journal
     * et met le jeu en pause s'il ne l'est pas
     */
    void ActiverJournal()
    {
        // Désactive le texte d'appuyer sur J
        if (texteJournal.activeInHierarchy) texteJournal.SetActive(false);
        
        // Inverse le jeu en pause
        jeuEnPause = !jeuEnPause;

        // Arrête le temps de jeu si le jeu est en pause
        Time.timeScale = jeuEnPause == true ? 0.0f : 1.0f;
        Journal.SetActive(jeuEnPause);

        // unlock le curseur si le jeu est en pause
        Cursor.lockState = jeuEnPause == true ? CursorLockMode.None : CursorLockMode.Locked;
    }

    /* Cette fonction applique les valeurs des toggles
     * dans une nouvelle créature et affiche la scène de fin
     */
    public void FinirJeu()
    {
        ChoixJoueur = new Creature(ChoisirCreature.ChoixCreature.ID, ChoisirCreature.ChoixCreature.Nom, ToggleLivre.isOn, ToggleOrbes.isOn, ToggleAraignee.isOn, ToggleTraces.isOn, TogglePortail.isOn);
        SceneManager.LoadScene("Fin");
    }

    /* Foncion qui fait jouer un son quand on tourne les pages du journal*/
    public void TournerPage()
    {
        interaction.GetComponent<AudioSource>().PlayOneShot(SonTournerPage);
    }
}
