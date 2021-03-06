﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour   
{
    private PhotonView PV;
    public GameObject myAvatar;     //一旦、下のStart内のmyAbvatarの右辺書いてから、ここで設定してる！

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();        //この書き方では、このｽｸﾘﾌﾟﾄをｱﾀｯﾁしたｹﾞｰﾑｵﾌﾞｼﾞｪｸﾄに、ｱﾀｯﾁされているPhotonViewをｹﾞｯﾄする（いつも思うがややこしい）
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);
        //ゼロ～GameSetup.csの配列変数spawnPointsの要素数の間の整数変数を生成し、変数spawnPickerに入れよ。
        if (PV.IsMine)      //PhotonView.isMineは実行中のPhotonViewが自分かどうかの判定に使う https://qiita.com/unsoluble_sugar/items/87a213b4facba78b455e
        {
            myAvatar=PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),   //(FileLocation,FileName)
                GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);     //position,rotation,versionNo
            //なんで上のｺｰﾄﾞではspawnPickerをﾗﾝﾀﾞﾑに生成させてる？二人のﾌﾟﾚｲﾔで同じspawnPicker値になることもあり得ないか？
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
