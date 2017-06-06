using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomJoined : MonoBehaviour {

    public void OnJoinedRoom()
    {
        Debug.Log("Connected players " + PhotonNetwork.playerList.Length.ToString());
        Room r = PhotonNetwork.room;
        if (r != null)
        {
            Debug.Log("in room");
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("PlayerName", UIController.Instance.room.roomNameField.text);
            r.SetCustomProperties(hash);
            Debug.Log("Setup values");
        }
       // UIController.Instance.room.AddRoomVariables();
        if(PhotonNetwork.playerList.Length == 1)
        {
            PhotonNetwork.playerName = "1";
            UIController.Instance.SetPlayerText("1");
            GameManager.instance.photView.RequestOwnership();
			CameraController.Instance.SwapCameras (false);
        }
        else if (PhotonNetwork.playerList.Length == 2){
            UIController.Instance.SetPlayerText("2");
            PhotonNetwork.playerName = "2";
            PhotonView photonView = PhotonView.Get(GameManager.instance);
            photonView.RPC("RPCStartGame", PhotonTargets.All);
            CameraController.Instance.TurnOnP2UI();
        }
    }
}
