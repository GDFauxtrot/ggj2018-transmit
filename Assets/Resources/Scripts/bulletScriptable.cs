using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = " Player bullet types")]

public class bulletScriptable : ScriptableObject {


	[Tooltip("0 is the default, 1 is upgraded, 2 is enemy")]
	public List<Sprite> bulletTypes;





	public float PlayerSpeed;
	public int PlayerDamage;

    public float UpPlayerSpeed;
    public int UpPlayerDamage;

    public bool upgraded;


	public float EnemySpeed;
	public int EnemyDamage;

	public void resetEverything()
	{
		upgraded=false;
	}

	public void PLayerGrabStronger()
	{
		upgraded=true;
	}

};
