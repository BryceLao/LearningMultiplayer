using Mirror;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;
    
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;

    private void Start() {
        if (cam == null) {
            Debug.LogError("PlayerShoot: No Camera Referenced");
            this.enabled = false;
        }
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }
    
    [Client]
    private void Shoot() {
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask)) {
            if (_hit.collider.tag == PLAYER_TAG) {
                CmdPlayerShot(_hit.collider.name, weapon.damage);
            }
        }
    }

    [Command]
    private void CmdPlayerShot(string _playerID, int _damage) {
        Debug.Log(_playerID+ " Has Been Shot.");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);

    }
}
