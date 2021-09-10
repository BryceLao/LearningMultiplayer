using System;
using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SerializeField] private int maxHealth = 100;

    [SyncVar] int currenthealth;

    private void Awake() {
        SetDefaults();
    }

    public void SetDefaults() {
        currenthealth = maxHealth;
    }

    public void TakeDamage(int _amount) {
        currenthealth -= _amount;

        Debug.Log(transform.name + " Now Has " + currenthealth + " Health");
    }
}
