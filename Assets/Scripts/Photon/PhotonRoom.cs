using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks,IInRoomCallbacks
{
    //Room info
    public static PhotonRoom room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    //Player info
    private Player[] photonPlayers;     //このPlayer[]は何だろ？ PhotonRealtime中にあるらしいが・・・
    public int playersInRoom;
    public int myNumberInRoom;

    public int playerInGame;

    //Delayed start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;

    //Singletonにする
    private void Awake()
    {
        if (PhotonRoom.room == null) { PhotonRoom.room = this; }
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;     //これでSingleTonというのか？置き換えてしまっているが・・・
            }
        }
        DontDestroyOnLoad(this.gameObject);

        //onenabele,unenable function
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;

    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;

    }

    // Update is called once per frame
    void Update()
    {
        if (MultiplayerSetting.multiplayerSetting.delayStart)
        {
            if (playersInRoom == 1)
            {
                RestartTimer();
                if (!isGameLoaded)
                {
                    if(readyToStart)
                    { atMaxPlayers -= Time.deltaTime;
                        lessThanMaxPlayers = atMaxPlayers;
                        timeToStart = atMaxPlayers;
                    }
                    else if (readyToCount)
                    {
                        lessThanMaxPlayers -= Time.deltaTime;
                        timeToStart = lessThanMaxPlayers;
                    }
                    Debug.Log("Display time to start to the players+timeToStart");
                    if (timeToStart <= 0)
                    {
                        StartGame();
                    }
                }
            }
        }
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are now in a room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();
        if (MultiplayerSetting.multiplayerSetting.delayStart)
        {
            Debug.Log("Displayer players in room out of max players possible(" + playersInRoom + ":" + MultiplayerSetting.multiplayerSetting.maxPlayers + ")");
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSetting.multiplayerSetting.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)      //（Playerがmaxで）もしﾏｽﾀｰｸﾗｲｱﾝﾄでなかったら　ﾏｽﾀｰｸﾗｲｱﾝﾄ？って何？
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;   //ﾏｽﾀｰｸﾗｲｱﾝﾄだったら、CurrentRoomをCLOSE
            }
        }
        else
        {
            StartGame();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room");
        photonPlayers = PhotonNetwork.PlayerList;       //新しい人が来たのでﾘｽﾄを更新
        playersInRoom++;
        if (MultiplayerSetting.multiplayerSetting.delayStart)
        {
            Debug.Log("Displayer players in room out of max players possible(" + playersInRoom + ":" + MultiplayerSetting.multiplayerSetting.maxPlayers + ")");
            if (playersInRoom > 1)
            {
                readyToCount = true;
            }
            if (playersInRoom == MultiplayerSetting.multiplayerSetting.maxPlayers)
            {
                readyToStart = true;
                if (PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
                }

            }
        }

    
    void StartGame()
    {
        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient) return;
        if (MultiplayerSetting.multiplayerSetting.delayStart)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiplayerSetting.multiplayerSetting.multiplayerScene);
    }

    void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 6;
        readyToCount = false;
        readyToStart = false;
    }

    void OnSceneFinishedLoading(Scene scene,LoadSceneMode mode)     //他の人がroomにjoinで呼ばれる、本質的にOnJoinedRoon()と同一
    {
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSetting.multiplayerSetting.multiplayerScene)
        {
            isGameLoaded = true;

            if (MultiplayerSetting.multiplayerSetting.delayStart)
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else
            {
                RPC_CreatePlayer();
            }
        }
    }
    [PunRPC]        //ラベルというもの, PunRPC は Photon Unity Networking Remote Procedure Call の略で、このラベル（属性）をつけた関数はリモートから呼ばれますよ、という意味
    private void RPC_LoadedGameScene()
    {
        playerInGame++;
        if (playerInGame == PhotonNetwork.PlayerList.Length)
        {
            PV.RPC("RPC_CreatedPlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position,Quaternion.identity,0);
        //Instantiateは3つの引数をとり、Instantiate(ﾌﾟﾚﾊﾌﾞ,位置情報,回転情報)
        //PhotonNetwork型の、ﾌﾟﾚﾊﾌﾞ変数PhotonPrefabs/PhotonNetworkPlayerのtransform.position、無回転、を指定してｲﾝｽﾀﾝｽを生成しろ
        //Path.Combine("PhotonPrefabs","PhotonNetworkPlayer")でPhotonPrefabs/PhotonNetworkPlayerの意味
        //このtransform.positionという書き方は、変数に設定された位置を指定することになるのではなく！、このスクリプトがアタッチされているゲームオブジェクトの位置をもってくる、このﾌﾟﾛｼﾞｪｸﾄの場合、RoomController
        //Quaternion.identityで無回転
        //最初の３つは通常のInstantiateと同じです。最後のグループには、プレファブにアタッチされているPhotonViewのプロパティにあるgroupと同じ数字を入れる　←　このPhotonNetworkPlayerのPhotonViewにはgroupﾌﾟﾛﾊﾟﾃｨがないような・・・

    }

}
