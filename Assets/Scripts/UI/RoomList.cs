using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomList : MonoBehaviour {
    public RectTransform contentPanel;
    public GameObject roomItemPrefab;
    public InputField UserNameField;
    public RectTransform rect;
    public bool isOpen = true;
    // Use this for initialization
    public void SetupRoom(RoomInfo ri)
    {

        if(ri.PlayerCount == 2)
        {
            return;
        }
        RoomItem ro = Instantiate(roomItemPrefab, contentPanel).GetComponent<RoomItem>();
        ro.GetComponent<RectTransform>().localScale = Vector3.one;
        ro.roomName.text = ri.Name;
        Debug.Log(ri.CustomProperties.Count);
        if (ri.CustomProperties.ContainsKey("PlayerName"))
        {
            Debug.Log("Found player name");
            ro.roomUser.text = ri.CustomProperties["PlayerName"].ToString();
        }
    }

    public void EmptyRoomList()
    {
        foreach (RoomItem roomItem in contentPanel.GetComponentsInChildren<RoomItem>())
        {
            Destroy(roomItem.gameObject);
        }
    }
    public void CreateRoom()
    {
       
        RoomOptions rop = new RoomOptions();
        rop.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        rop.MaxPlayers = 2;
        rop.IsVisible = true;
        rop.CustomRoomProperties.Add("PlayerName", UserNameField.text);
        PhotonNetwork.CreateRoom(null, rop, null);
        Close(UIController.Instance.roomListPanelTimer);
    }

    public void Close(float closeTime)
    {
        isOpen = false;
        StartCoroutine(ClosePanel(closeTime));
    }

    IEnumerator ClosePanel(float closeTime)
    {
        float timer = 0f;
        while (timer < closeTime)
        {
            yield return null;
            timer += Time.deltaTime;
            float val = Mathf.Lerp(1, 0, timer / closeTime);
            rect.localScale = new Vector3(val, val, val);
        }
        rect.localScale = Vector3.zero;
    }
    public void Open(float openTime)
    {
        StartCoroutine(OpenPanel(openTime));
    }

    public IEnumerator OpenPanel(float openTime)
    {
        float timer = 0f;
        while (timer < openTime)
        {
            yield return null;
            timer += Time.deltaTime;
            float val = Mathf.Lerp(0, 1, timer / openTime);
            rect.localScale = new Vector3(val, val, val);
        }
        rect.localScale = Vector3.one;
    }
}
