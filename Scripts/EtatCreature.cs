using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/* Ce script défini les états de la créature 
 * et utilise la machine à état pour changer d'un à l'autre
 */
public class EtatCreature : MonoBehaviour
{
    Creature monstre;               // Le type de créature choisi aléatoirement
    MachineAEtat cerveau;           // La machine a état
    NavMeshAgent agent;             // La créature
    float changerEsprit;            // Le timer avant que la créature change d'idée
    public GameObject joueur;       // Le joueur
    bool fixer = false;             // Si la créature fixe le joueur
    float timerMin;                 // Le minimum de temps avant que la créature change d'idée
    float timerMax;                 // Le maximum de temps avant que la créature change d'idée
    float modificateurSanteMentale; // Évalue la santé mentale du joueur. Plus sa santé est basse, plus la créature fait des choses
    bool aPortee;                   // Si le joueur est a portée
    public GameObject livre;        // Le livre
    bool interagirLivre;            // Si la créature peut interagir avec le livre
    float timerChasse;              // Le temps d'une chasse

    public GameObject portail;      // Le portail
    public bool choixDroit;         // Le pied à instancier
    public GameObject PiedGauche;   // Le pied gauche de la créature
    public GameObject PiedDroit;    // Le pied droit de la créature
    public GameObject GenrateurInsectes; // Le générateur d'insecte
    public static int santeMentale; // La santé mentale du joueur
    //public Text texteSanteMentale;  // Le texte affichant la santé mentale

    public AudioClip sonFixer;      // Le son lorque la créature fixe
    public AudioClip sonChasse;     // Le son de début de chasse
    public AudioClip sonPas;        // Le son des pas de la créature
    public AudioClip sonTouche;     // Le son de fin de chasse, que la créature a touché ou non le joueur
    public AudioClip sonInterupteur; // Son utiliser lorsque la créature ferme les lumières

    void Start()
    {
        // Reset les variables
        santeMentale = 3;
        choixDroit = true;
        agent = GetComponent<NavMeshAgent>();
        cerveau = GetComponent<MachineAEtat>();
        cerveau.ActiverEtat(Idle, EntrerIdle, null);
        timerMin = Random.Range(0.5f, 1.5f);
        timerMax = Random.Range(1.5f, 3);
        aPortee = false;
        interagirLivre = false;
        modificateurSanteMentale = 1;
        monstre = ChoisirCreature.ChoixCreature;
        // Si le monstre a des orbes, les active
        if (monstre.Orbes)
        {
            transform.Find("orbes").gameObject.SetActive(true);
        }
        // Si le monstre a des araignées, les active
        if (monstre.Insectes)
        {
            GenrateurInsectes.SetActive(true);
        }
        // Donne une position de départ aléatoire
        Vector3 directionMarche = (Random.insideUnitSphere * 6f) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(directionMarche, out navMeshHit, 6f, NavMesh.AllAreas);
        Vector3 destination = navMeshHit.position;
        agent.transform.position = destination;
    }

    void Update()
    {
        //texteSanteMentale.text = "Santé mentale : " + santeMentale;
        aPortee = Vector3.Distance(transform.position, joueur.transform.position) < 1;
        interagirLivre = Vector3.Distance(transform.position, livre.transform.position) < 1;
        if(fixer) agent.transform.LookAt(joueur.transform);
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Peinture" && monstre.Traces)
        {
            cerveau.ActiverEtat(Tracer, EntrerTracer, SortirTracer);
        }
    }

    /* l'état idle de la créature. Elle ne bouge pas et
     * elle peut fermer les lumières. C'est
     * dans le idle que sont décidés les états suivants
     */
    void EntrerIdle()
    {
        agent.ResetPath();
        GetComponent<Animator>().SetBool("marche", false);
        float jouerLumiere = Random.Range(0, 8) / modificateurSanteMentale;
        if(jouerLumiere <= 1)
        {
            GetComponent<AudioSource>().PlayOneShot(sonInterupteur);
            Invoke("ChangerLumieres", 0f);
        }
    }
    void Idle()
    {
        changerEsprit -= Time.deltaTime;

        // Choisi un état aléatoire avec des probabilités ajustées
        if (changerEsprit <= 0)
        {
            float test = Random.Range(0, (150 / modificateurSanteMentale));

            if (test <= 5)
            {
                cerveau.ActiverEtat(ChasserJoueur, EntrerChasserJoueur, SortirChasserJoueur);
            }
            else if (test <= 15)
            {
                cerveau.ActiverEtat(FixerJoueur, EntrerFixerJoueur, SortirFixerJoueur);
            }
            else if (test <= 40 && monstre.Portail)
            {
                cerveau.ActiverEtat(Teleporter, EntrerTeleporter, null);
            }
            else
            {
                cerveau.ActiverEtat(Marcher, EntrerMarche, SortirMarche);
                changerEsprit = Random.Range(timerMin, timerMax);
            }
        }
    }

    /* l'état marche de la créature. Elle détermine un point aléatoire et se 
     * déplace vers lui. Elle brûle le livre si elle passe à côté et peut
     * faire des bruits de pas. 
     */
    void EntrerMarche()
    {
        int marcherFort = Random.Range(1, 11);
        if(marcherFort == 1)
        {
            InvokeRepeating("FaireBruit", 0f, 1f);
        }
        // Se déplace vers un point choisi
        GetComponent<Animator>().SetBool("marche", true);
        Vector3 directionMarche = (Random.insideUnitSphere * 4f) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(directionMarche, out navMeshHit, 4f, NavMesh.AllAreas);
        Vector3 destination = navMeshHit.position;
        agent.SetDestination(destination);
    }
    void Marcher()
    {
        if(interagirLivre && monstre.Livre)
        {
            livre.GetComponent<AudioSource>().Play();
            livre.transform.Find("feu").gameObject.SetActive(true);
        }

        if (agent.remainingDistance <= 1f)
        {
            agent.ResetPath();
            float test = Random.Range(0, 10);
            if (test <= 3)
                cerveau.ActiverEtat(Idle, EntrerIdle, null);
            else
                cerveau.ActiverEtat(Marcher, EntrerMarche, SortirMarche);
        }
    }
    void SortirMarche()
    {
        CancelInvoke("FaireBruit");
        GetComponent<Animator>().SetBool("marche", false);
    }

    /* L'état tracer sert lorsque la créature laisse des traces derrière elle.
     * Elle se dirige vers un point et instancie des pas pendant son passage
     */
    void EntrerTracer()
    {
        GetComponent<Animator>().SetBool("marche", true);
        Vector3 directionMarche = (Random.insideUnitSphere * 5f) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(directionMarche, out navMeshHit, 5f, NavMesh.AllAreas);
        Vector3 destination = navMeshHit.position;
        agent.SetDestination(destination);
        InvokeRepeating("FaireBruit", 0f, 1f);
        InvokeRepeating("CreationPas", 0f, 0.5f);
    }
    void Tracer()
    {
        if (agent.remainingDistance <= 1f)
        {
            agent.ResetPath();
            cerveau.ActiverEtat(Marcher, EntrerMarche, SortirMarche);
        }
    }
    void SortirTracer()
    {
        CancelInvoke("CreationPas");
        CancelInvoke("FaireBruit");
    }

    /* L'état fixer est un idle où la créature apparait devant le joueur en le fixant.
     * Elle disparaît après */
    void EntrerFixerJoueur()
    {
        GetComponent<AudioSource>().PlayOneShot(sonFixer);
        agent.ResetPath();
        GetComponent<Animator>().SetBool("marche", false);
        fixer = true;
        ApparitionFantome();
        changerEsprit = Random.Range(timerMin, timerMax);
    }
    void FixerJoueur()
    {
        changerEsprit -= Time.deltaTime;
        if (changerEsprit <= 0)
        {
            cerveau.ActiverEtat(Marcher, EntrerMarche, SortirMarche);
            changerEsprit = Random.Range(2 * timerMin, 3 * timerMax);
        }
    }
    void SortirFixerJoueur()
    {
        fixer = false;
        DisparitionFantome();
    }

    /* Dans l'état de chasse, la créature cours après le joueur pour lui enlever 1 de santé mentale
     * Le joueur peut se sauver, puisqu'elle chasse uniquement pendant un certain temps*/
    void EntrerChasserJoueur()
    {
        GetComponent<AudioSource>().PlayOneShot(sonChasse);
        timerChasse = Random.Range(5, 11);
        agent.ResetPath();
        GetComponent<Animator>().SetBool("marche", true);
        ApparitionFantome();
    }
    void ChasserJoueur()
    {
        timerChasse -= Time.deltaTime;
        GetComponent<NavMeshAgent>().SetDestination(joueur.transform.position);
        if (aPortee)
        {
            modificateurSanteMentale++;
            santeMentale--;
            
            // Joueur un son ou apparition
            cerveau.ActiverEtat(Marcher, EntrerMarche, SortirMarche);
        }

        if (timerChasse <= 0)
        {
            cerveau.ActiverEtat(Marcher, EntrerMarche, SortirMarche);
        }
    }
    void SortirChasserJoueur()
    {
        GetComponent<AudioSource>().PlayOneShot(sonTouche);
        DisparitionFantome();
    }

    /* L'état de téléportation peut uniquement être activé si la créature se téléporte.
     * Elle se déplace vers un point aléatoire et crée un portail où elle arrive*/
    void EntrerTeleporter()
    {
        Vector3 directionMarche = (Random.insideUnitSphere* 4f) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(directionMarche, out navMeshHit, 4f, NavMesh.AllAreas);
        Vector3 destination = navMeshHit.position;
        agent.transform.position = destination;
        portail.transform.position = new Vector3(destination.x, destination.y+1, destination.z);
    }
    void Teleporter()
    {
        agent.ResetPath();
        float test = Random.Range(0, 10);
        if (test <= 7)
            cerveau.ActiverEtat(Idle, EntrerIdle, null);
        else
            cerveau.ActiverEtat(Marcher, EntrerMarche, SortirMarche);
    }

    /* Fonction qui change le layer de la créature pour la faire apparaître*/
    void ApparitionFantome()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
    /* Fonction qui change le layer de la créature pour la faire disparaître*/
    void DisparitionFantome()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("Creature");
        }
    }
    /* Fonction qui ferme les lumières*/
    void ChangerLumieres()
    {
        //Faire un son
        foreach (GameObject lumiere in joueur.GetComponent<Interactions>().lumieresSalles)
        {
            if (lumiere.activeInHierarchy)
            {
                lumiere.SetActive(false);
            }
        }
    }
    /* Fonction qui fait jouer un bruit de pas*/
    void FaireBruit()
    {
        GetComponent<AudioSource>().PlayOneShot(sonPas);
    }
    /* Fonction qui crée des traces de pas par terre qui sont
     * visibles par le joueur*/
    void CreationPas()
    {
        GameObject tracePas;
        if (choixDroit)
        {
            tracePas = Instantiate(PiedDroit);
            choixDroit = false;
        }
        else
        {
            tracePas = Instantiate(PiedGauche);
            choixDroit = true;
        }
        tracePas.SetActive(true);
        tracePas.transform.parent = null;
        tracePas.layer = 0;
        tracePas.transform.position = transform.position;
        tracePas.transform.rotation = transform.rotation;
        tracePas.transform.Rotate(90, 90, 90);
        Destroy(tracePas, 8f);
    }
}


