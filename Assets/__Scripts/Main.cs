using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
	static public Main S;
	static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;

	public GameObject[] prefabEnemies;
	public float enemySpawnPerSecond = 0.5f; 
	public float enemySpawnPadding = 1.5f;
	public WeaponDefinition[] weaponDefinitions;

	public GameObject prefabPowerUp;
	public WeaponType[] powerUpFrequency = new  WeaponType[] {
							WeaponType.blaster, WeaponType.blaster,
							WeaponType.spread,
							WeaponType.shield                    } ;

	public bool ________________;

	public WeaponType[] activeWeaponTypes;

	private float enemySpawnRate;

	void Awake() {
		S = this;
		// Set Utils.camBounds 
		Utils.SetCameraBounds(this.GetComponent<Camera>()); 
		enemySpawnRate = 1f/enemySpawnPerSecond;

		// Invoke 1 call to SpawnEnemy() in 2 seconds 
		Invoke( "SpawnEnemy", enemySpawnRate );

		// A generic Dictionary with WeaponType as the key 
		W_DEFS = new Dictionary<WeaponType, WeaponDefinition>(); 
		foreach( WeaponDefinition def in weaponDefinitions ) {
			W_DEFS[def.type] = def;
		}
	}

	static public WeaponDefinition GetWeaponDefinition( WeaponType wt ) { 
		// Check to make sure that the key exists in the Dictionary
		if (W_DEFS.ContainsKey(wt)) {
			return( W_DEFS[wt]);
		}

		return( new WeaponDefinition() );
	}

	public void SpawnEnemy() {
		int ndx = Random.Range(0, prefabEnemies.Length);
		GameObject go = Instantiate( prefabEnemies[ ndx ] ) as GameObject;
		Vector3 pos = Vector3.zero;
		float xMin = Utils.camBounds.min.x+enemySpawnPadding;
		float xMax = Utils.camBounds.max.x-enemySpawnPadding;
		pos.x = Random.Range( xMin, xMax );
		pos.y = Utils.camBounds.max.y + enemySpawnPadding; 
		go.transform.position = pos;
		Invoke( "SpawnEnemy", enemySpawnRate );
	}
		
	void Start () {
		activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
		for ( int i=0; i<weaponDefinitions.Length; i++ ) {
			activeWeaponTypes[i] = weaponDefinitions[i].type;
		}
	}

	public void DelayedRestart( float delay ) {
		Invoke("Restart", delay);
	}

	public void Restart() {
		SceneManager.LoadScene("__Scene_0");
	}
	
	public void ShipDestroyed( Enemy e ) {
		// Potentially generate a PowerUp
		if (Random.value <= e.powerUpDropChance) {
			int ndx = Random.Range(0,powerUpFrequency.Length); 
			WeaponType puType = powerUpFrequency[ndx];

			GameObject go = Instantiate( prefabPowerUp ) as GameObject; 
			PowerUp pu = go.GetComponent<PowerUp>();

			pu.SetType( puType );
			pu.transform.position = e.transform.position;
		}
	}
}
