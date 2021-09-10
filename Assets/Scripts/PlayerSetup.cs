using Mirror;
using UnityEngine;


[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {
   
   [SerializeField] Behaviour[] componentsToDisable;
   [SerializeField] private string remoteLayerName = "RemotePlayer";

   private Camera sceneCamera;

   private void Start() {
      if (!isLocalPlayer) {
         DisableComponents();
         AssignRemoteLayer();
      }

      else {
         sceneCamera=Camera.main; 
         
         if (sceneCamera != null) {
            sceneCamera.gameObject.SetActive(false);
         }
      }

      GetComponent<Player>().Setup();
   }

   private void DisableComponents() {
      for (int i = 0; i < componentsToDisable.Length; i++) {
         componentsToDisable[i].enabled = false;
      }
   }

   private void AssignRemoteLayer() {
      gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
   }

   private void OnDisable() {
      if (sceneCamera != null) {
         sceneCamera.gameObject.SetActive(true);
      }

      GameManager.UnRegisterPlayer(transform.name);

   }

   public override void OnStartClient() {
      base.OnStartClient();

      string _netID = GetComponent<NetworkIdentity>().netId.ToString();
      Player _player = GetComponent<Player>();

      GameManager.RegisterPlayer(_netID, _player);
   }
}
