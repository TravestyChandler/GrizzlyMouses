using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomJoined : MonoBehaviour {

    public void OnJoinedRoom()
    {
        Debug.Log("Connected players " + PhotonNetwork.playerList.Length.ToString());
        if(PhotonNetwork.playerList.Length == 1)
        {
            PhotonNetwork.playerName = "1";
            UIController.Instance.SetPlayerText("1");
        }
        else if (PhotonNetwork.playerList.Length == 2){
            UIController.Instance.SetPlayerText("2");
            PhotonNetwork.playerName = "2";
            PhotonView photonView = PhotonView.Get(GameManager.instance);
            photonView.RPC("RPCStartGame", PhotonTargets.All);
        }
    }
}
