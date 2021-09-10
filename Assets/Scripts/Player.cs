using System;
using Mirror;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    private bool[] wasEnabled;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Behaviour[] disableOnDeath;

    [SyncVar] int currenthealth;
    [SyncVar] bool _isDead = false;

    public bool isDead {
        get { return _isDead; }
        protected set{ _isDead = value;}
    }

    // void Update()
    // {
    // 	if (!isLocalPlayer)
    // 		return;
    //
    // 	if (Input.GetKeyDown(KeyCode.K))
    // 	{
    // 		RpcTakeDamage(99999);
    // 	}
    // }


    public void Setup() {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++) {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    public void SetDefaults() {
        isDead = false;
        currenthealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null) {
            _col.enabled = true;
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(int _amount) {
        if (isDead) {
            return;
        }

        currenthealth -= _amount;
        Debug.Log(transform.name + " Now Has " + currenthealth + " Health");

        if (currenthealth <= 0) {
            Die();
        }
    }

    private void Die() {
        isDead = true;
        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = false;
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null) {
            _col.enabled = false;
        }

        Debug.Log(transform.name+" Is Dead");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn() {
     yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

     SetDefaults();
     Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
     transform.position = _spawnPoint.position;
     transform.rotation = _spawnPoint.rotation;

     Debug.Log(transform.name+" Respawned");
    }
}
