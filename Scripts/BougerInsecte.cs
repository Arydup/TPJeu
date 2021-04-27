using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Ce script s'occupe du mouvement de chaque araignée
 *
 */
public class BougerInsecte : MonoBehaviour
{
    MachineAEtat cerveau;           // La machine a état
    NavMeshAgent agent;             // Le navMesh de l'araignée
    public GameObject canneNoel;
    bool bonbon;                    // true si l'araignée est proche de la canne

    void Start()
    {
        cerveau = GetComponent<MachineAEtat>();
        agent = GetComponent<NavMeshAgent>();
        cerveau.ActiverEtat(Marcher, EntrerMarche, null);
    }

    void Update()
    {
        // Si l'araignée est près de la canne, met bonbon a true
        bonbon = Vector3.Distance(transform.position, canneNoel.transform.position) < 1;
    }
    
    /* L'état de marche de l'araignée. Elle se fait en boucle
     */
    void EntrerMarche()
    {
        // Trouve une destination aléatoire et s'y rend
        Vector3 directionMarche = (Random.insideUnitSphere * 5f) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(directionMarche, out navMeshHit, 5f, NavMesh.AllAreas);
        Vector3 destination = navMeshHit.position;
        agent.SetDestination(destination);
    }
    void Marcher()
    {
        
        if (bonbon)
        {
            // L'araignée s'arrête sur la canne pour la manger
            agent.SetDestination(canneNoel.transform.position);
        }
        // Recommence l'état si elle arrive près de la destination
        else if(agent.remainingDistance <= 1f)
        {
            agent.ResetPath();
            cerveau.ActiverEtat(Marcher, EntrerMarche, null);
        }

    }
}
