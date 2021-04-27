using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAEtat : MonoBehaviour
{

    public Stack<Etat> Etats { get; set; }

    private void Awake()
    {
        Etats = new Stack<Etat>();
    }

    private void Update()
    {
        if (RegarderEtat() != null)
        {
            RegarderEtat().Executer();
        }
    }

    public void ActiverEtat(System.Action activer, System.Action entrer, System.Action sortir)
    {
        if (RegarderEtat() != null)
            RegarderEtat().Sortir();

        Etat etat = new Etat(activer, entrer, sortir);
        Etats.Push(etat);
        RegarderEtat().Entrer();

    }

    public void ActiverProchainEtat()
    {
        RegarderEtat().Sortir();
        RegarderEtat().ActiverAction = null;
        Etats.Pop();
        RegarderEtat().Entrer();
    }

    private Etat RegarderEtat()
    {
        return Etats.Count > 0 ? Etats.Peek() : null;
    }
}
