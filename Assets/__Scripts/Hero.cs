﻿using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {
	static public Hero S;

	public float speed = 30;
	public float rollMult = -45;
	public float pitchMult = 30;

	public float shieldLevel = 1;

	public bool ____________;

	public Bounds bounds;

	// Use this for initialization
	void Awake () {
		S = this;
		bounds = Utils.CombineBoundsOfChildren (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		float xAxis = Input.GetAxis ("Horizontal");
		float yAxis = Input.GetAxis ("Vertical");

		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		transform.position = pos;

		bounds.center = transform.position;

		Vector3 off = Utils.ScreenBoundsCheck (bounds, BoundsTest.onScreen);
		if (off != Vector3.zero) {
			pos = -off;
			transform.position = pos;
		}

		transform.rotation = Quaternion.Euler (yAxis * pitchMult, xAxis * rollMult, 0);
	}
}