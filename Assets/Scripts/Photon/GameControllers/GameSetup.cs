using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    
    public static GameSetup GS;

    public Transform[] spawnPoints;     //UnityのTransformｺﾝﾎﾟｰﾈﾝﾄ、position, rotation, scale　SPAWM：（卵が）ポンと生まれる
                                        //このﾘｽﾄの数はInspectorで「Spawn Points」の「サイズ」として設定する。基本はｹﾞｰﾑ参加人数。
//Singleton化する(異なるﾈｯﾄﾜｰｸﾌﾟﾚｲﾔｰが同じものを生成してくる可能性があるからだろう)
    private void OnEnable()
    {
        if (GameSetup.GS == null)
        {
            GameSetup.GS = this;
        }
    }

}
