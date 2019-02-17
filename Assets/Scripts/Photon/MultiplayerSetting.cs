using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSetting : MonoBehaviour
{
    public static MultiplayerSetting multiplayerSetting;

    public bool delayStart; //false:contituous laodong game, true:call delay start game
    public int maxPlayers;

    public int menuScene;
    public int multiplayerScene;

    private void Awake()   //Singletonにする。ひとつしか、「この」ｲﾝｽﾀﾝｽを存在させない。無かったら「これ」を作り、あれば「これ」をDestroy
    {
        if (MultiplayerSetting.multiplayerSetting == null) {
            MultiplayerSetting.multiplayerSetting = this;
        }
        else
        {
            if (MultiplayerSetting.multiplayerSetting != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);  //これって「this」がない場合があるんじゃないのか？
    }
}
