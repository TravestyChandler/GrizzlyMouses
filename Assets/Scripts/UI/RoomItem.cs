using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour {
    public Text roomName;
    public Text roomDescription;
    public Text roomUser;

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
        UIController.Instance.CloseRoomList();
    }

}
