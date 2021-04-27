using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChoisirCreature : MonoBehaviour
{
    // Le choix de la créature au début de la partie
    public static Creature ChoixCreature; 
    // Liste de différentes créatures pour le jeu
    public List<Creature> creatures = new List<Creature>();

    void Start()
    {
        // Pour chaque créature, crée un objet et l'ajoute dans la liste
        Creature Diablotin = new Creature(0, "Diablotin", true, true, true, false, false);
        creatures.Add(Diablotin);
        Creature Mara = new Creature(1, "Mara", true, false, true, false, true);
        creatures.Add(Mara);
        Creature Gremlin = new Creature(2, "Gremlin", true, false, true, true, false);
        creatures.Add(Gremlin);
        Creature FeuFollet = new Creature(3, "Feu follet", true, true, false, false, true);
        creatures.Add(FeuFollet);
        Creature Spriggan = new Creature(4, "Spriggan", true, true, false, true, false);
        creatures.Add(Spriggan);
        Creature Harlequin = new Creature(5, "Harlequin", true, false, false, true, true);
        creatures.Add(Harlequin);
        Creature Spectre = new Creature(6, "Spectre", false, true, true, false, true);
        creatures.Add(Spectre);
        Creature Goule = new Creature(7, "Goule", false, true, true, true, false);
        creatures.Add(Goule);
        Creature Sorciere = new Creature(8, "Sorcière", false, false, true, true, true);
        creatures.Add(Sorciere);
        Creature Vampire = new Creature(9, "Vampire", false, true, false, true, true);
        creatures.Add(Vampire);

        // Choix aléatoire d'une créature dans la liste
        int id = Random.Range(0, 10);
        ChoixCreature = creatures.Find(Creature => Creature.ID == id);
    }
}

/* Défini ce qu'est une créature */
public class Creature
{
    public int ID { get; set; }
    public string Nom { get; set; }
    public bool Livre { get; set; }
    public bool Orbes { get; set; }
    public bool Insectes { get; set; }
    public bool Traces { get; set; }
    public bool Portail { get; set; }
    public Creature(int id, string nom, bool livre, bool orbes, bool insectes, bool traces, bool portail)
    {
        ID = id;
        Nom = nom;
        Livre = livre;
        Orbes = orbes;
        Insectes = insectes;
        Traces = traces;
        Portail = portail;
    }
}
