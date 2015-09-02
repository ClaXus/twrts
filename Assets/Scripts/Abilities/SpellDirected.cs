using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpellDirected : Spell {
    [SerializeField]
    GameObject _projectile;
	
    public GameObject projectile
    {
        get { return _projectile; }
        set { _projectile = value; }
    }

    [SerializeField]
    int _value;

    public int value
    {
        get { return _value; }
        set { _value = value; }
    }

    protected void OnSpellTriggered()
    {
        
    }

    protected void OnUpdate()
    {

    }

    protected void OnSpellHit()
    {

    }
}
