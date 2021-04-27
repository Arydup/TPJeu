using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Ce script s'occupe des collisions entre le personnage et 
 * les objets qui l'entoure
 */
public class Interactions : MonoBehaviour
{
    public AudioSource SonInteractions;     // L'audioSource qui joue les sons, ce n'est pas le personnage  
    public AudioClip SonSprayPeinture;      // Le son du spray de peinture
    public AudioClip Sonlumieres;           // Le son pour allumer et fermer les lumières
    public GameObject imageObjet;           // L'image centrale lorsqu'on regarde un objet
    public GameObject imageLumiere;         // L'image centrale lorsqu'on regarde une lumière
    public GameObject pointRouge;           // Le point rouge dans l'écran
    public GameObject[] lumieresSalles;     // Un tableau contenant toutes les lumières de la maison
    public GameObject parentObjets;         // L'objet parent lorsque le joueur prend un objet
    bool peutPrendreObjets = true;          // Si le joueur peut prendre un objet par terre
    GameObject objetALancer;                // L'objet dans les mains du joueur
    public Camera raycastCamera;            // La caméra qui s'occupe des raycast. Elle suit la Main Camera, mais ses clipping planes sont normaux
    public GameObject PeintureCanvas;       // L'image de la canette de peinture
    public GameObject BonbonCanvas;         // L'image de la canne en bonbon
    public GameObject LivreCanvas;          // L'image du livre
    public GameObject peinture;             // la peinture jaune par terre
    public GameObject journal;              // L'interface pour ouvrir le journal   
    int utilisationPeinture = 5;            // Le nombre de spray disponibles

    void Update()
    {
        // Si le jeu est en pause, empêche de cliquer partout
        if (journal.GetComponent<ModifierInterface>().jeuEnPause)
            return;

        Ray camRay = raycastCamera.ScreenPointToRay(Input.mousePosition);

        // Contient les infos retournées par le Raycast sur l’objet touché 
        RaycastHit infoCollision;

        if (Physics.Raycast(camRay.origin, camRay.direction, out infoCollision, 1.9f))
        {
            // Si le raycast touche un interrupteur
            if(infoCollision.collider.gameObject.tag == "Interrupteur")
            {
                // Active l'image de la lumiere
                imageLumiere.SetActive(true);
                pointRouge.SetActive(false);

                if (Input.GetMouseButtonDown(0))
                {
                    // Active les différents groupes de lumières en fonction de leurs noms
                    switch (infoCollision.collider.gameObject.name)
                    {
                    case "PFB_Lightswitch (1)":
                        ActiverLumieres(0);
                    break;

                    case "PFB_Lightswitch (2)":
                        ActiverLumieres(1);
                        break;

                    case "PFB_Lightswitch (3)":
                        ActiverLumieres(2);
                        break;
                    }
                }
            }
            else
            {
                // Désactive l'image de la lumière si autre chose est touché
                imageLumiere.SetActive(false);
                pointRouge.SetActive(true);
            }
            // Si le raycast touche un objet
            if (infoCollision.collider.gameObject.tag == "Objet")
            {
                // Active l'image de la main
                imageObjet.SetActive(true);
                pointRouge.SetActive(false);

                if (peutPrendreObjets && Input.GetKeyDown(KeyCode.E))
                {
                    // Stoque l'objet dans la variable objetALancer
                    objetALancer = infoCollision.collider.gameObject;
                    PrendreObjets();
                } 
            }
            else
            {
                // Désactive l'image de la main si autre chose est touché
                imageObjet.SetActive(false);
            }
            // Si le joueur regarde le sol avec le vaporisateur de peinture en appuyant sur F
            if(peutPrendreObjets == false)
            {
                if (objetALancer.gameObject.name == "VaporisateurPeinture" && infoCollision.collider.gameObject.tag == "Sol")
                {
                    if (utilisationPeinture > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            // Met une tache de peinture par terre où le joueur regarde
                            SonInteractions.GetComponent<AudioSource>().PlayOneShot(SonSprayPeinture);
                            GameObject tachePeinture = Instantiate(peinture);
                            tachePeinture.SetActive(true);
                            tachePeinture.transform.position = new Vector3(infoCollision.point.x, infoCollision.point.y + 0.03f , infoCollision.point.z);
                            utilisationPeinture--;
                        }
                    }
                }
            }
        }
        else
        {
            // Active l'image du point rouge si le raycast n'entre pas en contact avec quelque chose 
            // et que le range est dépassé
            imageLumiere.SetActive(false);
            imageObjet.SetActive(false);
            pointRouge.SetActive(true);
        }

        // Si le joueur possède un objet, il peut le lancer avec le clique droit de la souris
        if (Input.GetKeyDown(KeyCode.Mouse1) && peutPrendreObjets == false)
        {
            LacherObjets();
        }
    }

    // Fonction qui sert à activer ou fermer les lumières selon l'index de la salle
    // si le joueur appuie sur le clique gauche de la souris devant un interrupteur.
    public void ActiverLumieres(int salleIndex)
    {
        SonInteractions.GetComponent<AudioSource>().PlayOneShot(Sonlumieres);
        if (!lumieresSalles[salleIndex].activeInHierarchy)
        {
            lumieresSalles[salleIndex].SetActive(true);
        }
        else
        {
            lumieresSalles[salleIndex].SetActive(false);
        }
       
    }

    /* Fonction qui positionne l'objet pris dans le coin droit de l'écran.
     * L'objet suit la caméra et il pourra être lancé par la suite.
     */
    public void PrendreObjets()
    {
        peutPrendreObjets = false;
        // Ignore la collision de l'objet avec le joueur
        Physics.IgnoreCollision(objetALancer.GetComponent<Collider>(), GetComponent<Collider>());
        // Change le parent de l'objet et lui applique la position et la rotation de son parent
        objetALancer.transform.parent = parentObjets.transform;
        objetALancer.transform.position = parentObjets.transform.position;
        objetALancer.SetActive(false);
        if (objetALancer.name == "livre")
        {
            LivreCanvas.SetActive(true);
            objetALancer.transform.rotation = parentObjets.transform.rotation;
        }
        else if(objetALancer.name == "VaporisateurPeinture")
        {
            PeintureCanvas.SetActive(true);
        
        }
        else if (objetALancer.name == "Bonbon")
        {
            BonbonCanvas.SetActive(true);

        }
        else if(objetALancer.name == "Cellulaire")
        {
            objetALancer.transform.rotation = parentObjets.transform.rotation;
            objetALancer.SetActive(true);
        }
        // Empêche l'objet de bouger dans l'écran
        objetALancer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        objetALancer.GetComponent<Rigidbody>().useGravity = false;
    }

    /* Fonction qui lance l'objet en appliquant une vélocité vers l'avant de son axe local
     */
    public void LacherObjets()
    {
        peutPrendreObjets = true;
        if (objetALancer.name == "livre")
        {
            LivreCanvas.SetActive(false);
        }
        else if (objetALancer.name == "VaporisateurPeinture")
        {
            PeintureCanvas.SetActive(false);
        }
        else if (objetALancer.name == "Bonbon")
        {
            BonbonCanvas.SetActive(false);
        }
        // L'objet n'a plus de parent
        objetALancer.transform.parent = null;
        objetALancer.SetActive(true);
        // Réactive la gravité et le collider pour que l'objet tombe
        objetALancer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        objetALancer.GetComponent<Rigidbody>().useGravity = true;
        // Applique une vélocité pour lancer l'objet en angle en fonction de son axe local
        var locVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
        locVel.x = 0;
        locVel.y = 1;
        locVel.z = 2;
        objetALancer.GetComponent<Rigidbody>().velocity = transform.TransformDirection(locVel);
    }
}
