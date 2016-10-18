using UnityEngine;
using System.Collections;

abstract public class Observable : MonoBehaviour {



    public void addObserver(Observer obs)
    {

    }

    public void removeObserver(Observer obs)
    {

    }

    abstract public void notify(string description);

}
