using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Ce script définie ce qu'est la classe Etat
 * qui est utilisée par les ennemis
 */
public class Etat
{
    public Action ActiverAction, EntrerAction, SortirAction;

    public Etat(Action activer, Action entrer, Action sortir)
    {
        ActiverAction = activer;
        EntrerAction = entrer;
        SortirAction = sortir;
    }
    public void Executer()
    {
        if (ActiverAction != null)
            ActiverAction.Invoke();
    }
    public void Entrer()
    {
        if (EntrerAction != null)
            EntrerAction.Invoke();
    }
    public void Sortir()
    {
        if (SortirAction != null)
            SortirAction.Invoke();
    }
}
