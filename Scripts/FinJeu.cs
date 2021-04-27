using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Ce script s'occupe des textes affichés 
 * dans la scène de conclusion
 */
public class FinJeu : MonoBehaviour
{
    
    public Text decision;       // Le texte de victoire ou défaite
    public Text typeCreature;   // Le texte qui 

    void Start()
    {
        Creature monstreJoueur = ModifierInterface.ChoixJoueur;
        Creature monstreChoisi = ChoisirCreature.ChoixCreature;

        // Si les données entrées par le joueur dans le journal sont similaires, le joueur a gagné
        if (monstreJoueur.Livre == monstreChoisi.Livre && monstreJoueur.Orbes == monstreChoisi.Orbes && monstreJoueur.Insectes == monstreChoisi.Insectes && monstreJoueur.Traces == monstreChoisi.Traces && monstreJoueur.Portail == monstreChoisi.Portail) 
        {
            decision.text = ("Victoire!");
        }
        else
        {
            decision.text = ("Defaite!");
        }

        // Ecrit le nom de la créature choisie
        typeCreature.text = Introduction.NomJoueur + "\nLe monstre était un(e) : " + monstreChoisi.Nom;
    }

    public void Recommencer()
    {
        SceneManager.LoadScene("Jeu");
    }

    public void RetournerMenu()
    {
        SceneManager.LoadScene("Intro");
    }
}
