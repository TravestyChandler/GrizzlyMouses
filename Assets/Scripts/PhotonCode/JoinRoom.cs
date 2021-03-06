﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoom : MonoBehaviour {

    
	/// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
	public bool AutoConnect = true;

	public byte Version = 1;

	/// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
	private bool ConnectInUpdate = true;


	public virtual void Start()
	{
		PhotonNetwork.autoJoinLobby = true;    // we join randomly. always. no need to join a lobby to get the list of rooms.

    }

    public void ShowRoomList()
    {
        UIController.Instance.room.EmptyRoomList();
        RoomInfo[] ris = PhotonNetwork.GetRoomList();
        if (UIController.Instance.room.isOpen)
        {

        }
        else {
            UIController.Instance.RoomListPopUp();
        }
        if (ris.Length > 0)
        {
            foreach (RoomInfo room in PhotonNetwork.GetRoomList())
            {
                if (room.PlayerCount >= 2) { 

                }
                else {
                    UIController.Instance.room.SetupRoom(room);
                }
            }
        }
        else
        {
           // Debug.Log("No rooms exist, please create one.");
        }
    }

	public virtual void Update()
	{
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            //Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
        }
    }


	// below, we implement some callbacks of PUN
	// you can find PUN's callbacks in the class PunBehaviour or in enum PhotonNetworkingMessage


	public virtual void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
    }

	public virtual void OnJoinedLobby()
	{
		//Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
    }

    public virtual void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2, IsVisible = true }, null);
	}

	// the following methods are implemented to give you some context. re-implement them as needed.

	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}
    public void OnReceivedRoomListUpdate()
    {
        if (!PhotonNetwork.inRoom)
        {
           
            ShowRoomList();
        }
    }

    public void OnJoinedRoom()
	{
		//Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
        //Debug.Log(PhotonNetwork.GetRoomList().Length);
    }
}
