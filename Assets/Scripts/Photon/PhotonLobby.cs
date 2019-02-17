using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

   public GameObject battleButton;
   public GameObject cancelButton;

    

    private void Awake()
    {
        lobby = this; //Create the singleton, lives the Main menu scene;
    }

    // Start is called before the first frame update
    void Start()    {
        PhotonNetwork.ConnectUsingSettings(); //Connects to Master photon server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        battleButton.SetActive(true); //Player is now connected to servers, enable battlebutton to alow join a game
    }

    public void OnBattleButtonClicked()
    {
        Debug.Log("BattleButtonClicked");
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. There must be no open games available"); //Joinに失敗する理由は、利用できる部屋がない、が多いので
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Trying to create a Room");
        int randomRoomName = Random.Range(0, 10000);
//        PhotonNetwork.CreateRoom(null, new RoomOptions());
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSetting.multiplayerSetting.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

 //   public override void OnJoinedRoom()
//    {
 //       Debug.Log("We are now in a room");
 //   }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
        CreateRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCancelButtonClicked()
    {
        Debug.Log("CancelButton was Clicked");
        cancelButton.SetActive(false);
        battleButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
