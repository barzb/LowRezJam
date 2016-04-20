using UnityEngine;
using System.Collections;
using System;

/*
    'Action' is a delegate (pointer) to a method, that takes zero, 
        one or more input parameters, but does not return anything.

    'Func' is a delegate (pointer) to a method, that takes zero, 
        one or more input parameters, and returns a value (or reference).
*/
public abstract class IBehaviour : MonoBehaviour
{
    protected Action Selector(Func<bool> condition, Action ifTrue, Action ifFalse)
    {
        return () => { if (condition()) ifTrue(); else ifFalse(); };
    }

    protected Action Sequence(Action a, Action b)
    {
        return () => { a(); b(); };
    }

    // Executed in Update()
    protected abstract void Execute();
    void Update() { Execute(); }
}