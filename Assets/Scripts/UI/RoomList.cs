using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomList : MonoBehaviour {
    public RectTransform contentPanel;
    public GameObject roomItemPrefab;
    public InputField UserNameField;
    // Use this for initialization
    public void SetupRoom(RoomInfo ri)
    {
        RoomItem ro = Instantiate(roomItemPrefab, contentPanel).GetComponent<RoomItem>();
        ro.name = ri.Name;
        
    }

    public void CreateRoom()
    {
        RoomOptions rop = new RoomOptions();
        rop.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        rop.CustomRoomProperties.Add("PlayerName", UserNameField.text);
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, null);
    }
}
