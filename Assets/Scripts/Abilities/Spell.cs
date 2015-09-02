﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Spell : ScriptableObject
{
	
	[SerializeField]
	Sprite _correspondingSprite;
	public Sprite correspondingSprite
	{
		get { return _correspondingSprite; }
		set { _correspondingSprite = value; }
	}
	
	[SerializeField]
	protected string _description = "Description";

    public string description
    {
        get { return _description; }
        set { _description = value; }
    }

    [SerializeField]
    public float _cooldown = 0.0f;

    public float cooldown
    {
        get { return _cooldown; }
        set { _cooldown = value; }
    }

    [SerializeField]
    protected float _castTime = 0.0f;

    public float castTime
    {
        get { return _castTime; }
        set { _castTime = value; }
    }
    
    virtual protected void OnSpellTriggered(){}
    virtual protected void OnUpdate(){}
    virtual protected void OnSpellHit(){}
}