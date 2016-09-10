using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {
	public Vector2 rotMinMax = new Vector2 (15, 90);
	public Vector2 driftMinMax = new Vector2 (.25f, 2);
	public float lifeTime = 6f;
	public float fadeTime = 4f;
	public bool ________________;
	public WeaponType type;
	public GameObject cube;
	public TextMesh letter;
	public Vector3 rotPerSecond;
	public float birthTime;

	void Awake() {
		// Find the Cube reference
		cube = transform.Find("Cube").gameObject; 
		letter = GetComponent<TextMesh>();

		Vector3 vel = Random.onUnitSphere;
		vel.z = 0; // Flatten the vel to the XY plane
		vel.Normalize(); // Make the length of the vel 1

		vel *= Random.Range(driftMinMax.x, driftMinMax.y);
		GetComponent<Rigidbody>().velocity = vel;

		transform.rotation = Quaternion.identity;

		rotPerSecond = new Vector3( Random.Range(rotMinMax.x,rotMinMax.y), 
									Random.Range(rotMinMax.x,rotMinMax.y),
									Random.Range(rotMinMax.x,rotMinMax.y) ); 

		InvokeRepeating( "CheckOffscreen", 2f, 2f );

		birthTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		cube.transform.rotation = Quaternion.Euler( rotPerSecond*Time.time );

		float u = (Time.time - (birthTime+lifeTime)) / fadeTime;

		if (u >= 1) {
			Destroy (this.gameObject);
			return;
		}
		if (u>0) {
			Color c = cube.GetComponent<Renderer>().material.color; 
			c.a = 1f-u; 
			cube.GetComponent<Renderer>().material.color = c;

			c = letter.color;
			c.a = 1f - (u*0.5f);
			letter.color = c;
		}
	}

	public void SetType( WeaponType wt ) {
		// Grab the WeaponDefinition from Main
		WeaponDefinition def = Main.GetWeaponDefinition( wt );
		// Set the color of the Cube child 
		cube.GetComponent<Renderer>().material.color = def.color; 
		//letter.color = def.color; // We could colorize the letter too 
		letter.text = def.letter; // Set the letter that is shown
		type = wt; // Finally actually set the type
	}

	public void AbsorbedBy( GameObject target ) {
		Destroy( this.gameObject );
	}

	void CheckOffscreen() {
		// If the PowerUp has drifted entirely off screen
		if ( Utils.ScreenBoundsCheck( cube.GetComponent<Collider>().bounds, BoundsTest.offScreen ) != Vector3.zero ) {  
			Destroy( this.gameObject );
		} 
	}
}
