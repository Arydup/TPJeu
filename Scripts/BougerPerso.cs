using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ce script est utilisé pour les mouvements du personnage et de la caméra
public class BougerPerso : MonoBehaviour
{
    public static float vitesseDeplacementPerso = 1f;   // Vitesse de déplacement du personnage
    public float vitesseHorizontale;                    // Vitesse horizontale de la souris
    public float vitesseVerticale;                      // Vitesse verticale de la souris
    public GameObject cameraFPS;                        // La caméra qui sert de vue du personnage 
    float rotationV = 0f;
    float rotationH;
    public GameObject journal;                          // pour vérifier si le jeu est en pause
    public AudioClip[] sonPas;                          // Un tableau de son de pas
 
    void Start()
    {
        // Active le verrouillage du curseur
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        // Bouge le personnage en fonction des touches appuyées et sur son axe local
        var locVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
        locVel.x = Input.GetAxisRaw("Horizontal");
        locVel.y = 0f;
        locVel.z = Input.GetAxisRaw("Vertical");
        GetComponent<Rigidbody>().velocity = transform.TransformDirection(locVel).normalized * vitesseDeplacementPerso;

        // Si le personnage est en mouvement, active le son de marche
        if (GetComponent<Rigidbody>().velocity.magnitude > 0)
        {
            if (!IsInvoking("JouerSon"))
            { 
                InvokeRepeating("JouerSon",0f, 1f);
            }
        }
        else
        {
            CancelInvoke("JouerSon");
        }
    }

    void Update()
    {
        // Si le jeu est en pause, empêche la rotation de la camera de s'effectuer
        if(journal.GetComponent<ModifierInterface>().jeuEnPause) return;

        // Variation de la position horizontale de la souris et tourne le personnage
        rotationH = Input.GetAxis("Mouse X") * vitesseHorizontale;
        transform.Rotate(0, rotationH, 0);

        // Variation de la position verticale de la souris et tourne la caméra FPS avec des limites
        rotationV += Input.GetAxis("Mouse Y") * vitesseVerticale;

        // limite la valeur de l’angle de rotation entre une min et une max
        rotationV = Mathf.Clamp(rotationV, -65, 45);

        // Applique les angles de rotation à la caméra, 
        cameraFPS.transform.localEulerAngles = new Vector3(-rotationV, 0, 0);
        
    }
    /* Cette fonction choisi un pas au hasard et le fait jouer */
    void JouerSon()
    {
        GetComponent<AudioSource>().PlayOneShot(sonPas[Random.Range(0, sonPas.Length)]);
    }
}



