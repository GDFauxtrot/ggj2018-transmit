using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = " Player bullet types")]

public class bulletScriptable : ScriptableObject {


	public List<Sprite> bulletTypes;





	public float PlayerSpeed;
	public float PlayerDamage;

    public float UpPlayerSpeed;
    public float UpPlayerDamage;

    public bool upgraded;


	public Sprite EnemySprite;
	public float EnemySpeed;
	public float EnemyDamage;

	public void resetEverything()
	{
		upgraded=false;
	}

	public void PLayerGrabStronger()
	{
		upgraded=true;
	}

};
