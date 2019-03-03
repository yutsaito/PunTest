using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public int characterValue;
    public GameObject myCharacter;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        //次のｺｰﾄﾞはこのScriptでは最後に書いている
        //Call a RPC function, Only sent from LocalPlayer
        if (PV.IsMine)  //Check to make sure We are the local Player
        {   //Call RPC function
            //RPCを実行したいタイミングで、PhotonViewのRPCメソッドを実行します。//
            //第一引数で指定した文字列のメソッドを、第二引数で指定した対象で実行する.
            //第三引数以降（可変引数）を渡すと、引数ありのメソッド
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.mySelectedCharacter);
        }
    }

    [PunRPC]    //Add Tag   RPCを送る場合はまず、RPCで実行したいメソッドにPunRPCAttributeをつけ目印を付けます。こうすることで、このメソッドがRPC対応であることをシステムに伝えることができます。
    void RPC_AddCharacter(int whichCharacter)
    {
        characterValue = whichCharacter;        //これは後から付け足したｺｰﾄﾞ。先に下を書いて、その後、whichCharacterをﾈｯﾄﾜｰｸ間で保存するために書いている。
        //wWith inside this function, We need Instantiated character we have selected
        myCharacter = Instantiate(PlayerInfo.PI.allCharacters[whichCharacter],transform.position,transform.rotation, transform);
        //PlayerInfo.cs型の変数PI中のﾘｽﾄallCharactersの[選択番号要素]のｲﾝｽﾀﾝｽを作って変数myCharacterに入れろ
        //位置は,回転は、このAvatarScriptがｱﾀｯﾁされたｹﾞｰﾑｵﾌﾞｼﾞｪｸﾄの位置と回転とし、
        //このCharacterをPlayerAvatarのChildとしたいので、最後transformを入れる。→　ここのtransformがよくわからない,なぜこれで「PlayerAvatar」のChildに？

        //it will(?) also be a good idea to save this value(whichCharacter) that We pass across the Network, So add the top・new variable・・characterValue
    }
}
//https://www.urablog.xyz/entry/2016/09/19/232345#PhotonViewisMine%E3%82%92%E4%BD%BF%E3%81%A3%E3%81%A6%E6%89%80%E6%9C%89%E8%80%85%E3%81%AE%E5%88%A4%E6%96%AD%E3%82%92
//ルーム内にてオブジェクト生成の同期を取る際は、PhotonNetwork.Instantiate() を使用すればいいんでしたね。
//　では作成したオブジェクトが、自分が作成したオブジェクトなのか、他の人が作成したオブジェクトなのか判断できるのでしょうか。
//　PhotonNetwork.Instantiate() にて作成するオブジェクトに必ずアタッチしろと言われたPhotonViewあたりが何か知っていそうです。

//Photon Unity Networking: メインページ

//isMineプロパティが使えそうです。
//　自分自身が所有者である場合にtrueが返ってくるそうです。
//　なるほど、生成者ではなく所有者という考え方なのですね。生成したプレイヤーとそれを制御するプレイヤーが違う可能性は確かにありそうですね。

//同期対象のオブジェクトを生成する箇所でも触れた通り、別のクライアントに対してなにかしらのメッセージを送りたい場合があります。
//その際に利用されるのがこの「RPC（Remote Procedure Call）」です。
//名称からも分かるように、リモートクライアントに対して関数を実行する命令を送るための仕組みです。

//ボールに対して「蹴る」などのアクションをしたい。 そうした場合に、みながみな、
//一斉にボールに対してアクションを実行してしまっては成り立つものも成り立ちません。
//ではどうするのか。 それが「ローカルプレイヤー」という概念と、権限（やオーナーシップ）という概念です。 
//ローカルプレイヤーとは、今まさに実行されているゲームをプレイしているユーザただひとりだけが設定される設定です。
// （当然、遠隔地の別のPCでは別のキャラクターが「ローカルプレイヤー」として設定される）
//そしてたくさんいるローカルプレイヤーのうち、誰がボールに対するアクションが行えるのか、を決めるのが
//「権限」というわけです。 つまり、（上の例で言えば）権限を持ったローカルプレイヤーだけがボールを操作することができる、というわけです。