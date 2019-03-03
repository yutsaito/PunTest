using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{   //First thing I"m going to do is tp create some varuables
    //want to persist(?) from Scene to Scene, Iwanna be to access this script from other script
    //So I"m going to create Singleton for this script

    public static PlayerInfo PI;

    public int mySelectedCharacter;

    public GameObject[] allCharacters; //全てのｷｬﾗｸﾀﾘｽﾄ これはｹﾞｰﾑｵﾌﾞｼﾞｪｸﾄPlayerInfoのInspecterで、ｻｲｽﾞを入力後、それぞれのｷｬﾗｸﾀPrefabをｱﾀｯﾁ(?)して設定する

    //singleton
    private void OnEnable()
    {
        if (PlayerInfo.PI == null)      //もし、PlayerInfo.csの変数PIが空だったら(null)
        {
            PlayerInfo.PI = this;      //「これ」をPIにいれろ
        }
        else
        {
            if (PlayerInfo.PI != this)  //もし、PlayerInfo.csの変数PIが、「これ」じゃなかったら →　ﾘｾｯﾄｼﾝｸﾞﾙﾄﾝ
            {
                Destroy(PlayerInfo.PI.gameObject);      //そのゲームオブジェクトPIののｲﾝｽﾀﾝｽを破壊しろ(このgameObjectの使いKたがわからない）
                PlayerInfo.PI = this;                   //そしてそのPIに「これ」をいれろ　→　ここまでがシングルトン
            }
        }
//        DontDestroyOnLoad(this.gameObject);     //「この」ｹﾞｰﾑｵﾌﾞｼﾞｪｸﾄのｲﾝｽﾀﾝｽgameObjectは壊さないように
//5:31でこれを書いているのに、5:32では消えている、そのためｺﾒﾝﾄｱｳﾄした
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MyCharacter"))      //PlayerPrefabses がexsistかどうか？　HasKeyって何？　→　値があるか/ないか
        {
            mySelectedCharacter = PlayerPrefs.GetInt("MyCharacter");    //GetIntって何?　→　値の取り出し
        }
        else
        {
            mySelectedCharacter = 0;
            PlayerPrefs.SetInt("MyCharacter", mySelectedCharacter);     //そもそもPlayerPrefsってなんだっけ？　→　Unityに用意されているｸﾗｽ 値を保持する
                    //PlayerPrefbsｸﾗｽ(Unityあらかじめｸﾗｽ)のsetInt を使って、変数MyCharacterにmySelectedCharacterをｾｯﾄする
        }
    }

}

//クラスとインスタンスの違いです。
//GameObject（大文字）は「GameObjectというクラス」を指します。
//一方gameObject（小文字）は「そのコンポーネントが付いているGameObjectオブジェクトそのもの（＝インスタンス）」を指します。
//例えば「gameObject」とだけ書かれていたら「現在のスクリプトが付いているオブジェクトのGameObjectコンポーネント」を指し、
//「coll.gameObject」なら「coll（＝ぶつかったオブジェクト）のGameObjectコンポーネント」を指します

//GameObjectは、GameObjectクラスを指定する書き方。だから、GameObject.FindでGameObjectクラスに定義されているstaticメソッドFindを指定する事ができる。
//gameObjectは、GameObjectクラスのインスタンスを指定する書き方。だから、GameObjectクラスのインスタンスのtagプロパティを指定することができる。

//PlayerPrefabsｸﾗｽ　　ﾃﾞｰﾀが存在するかどうか、PlayerPrefs.HasKey　例：public static bool HasKey(string key);
//値を保持したいとき、 ベストスコアとか、ゲームを終了した後にも値を保持したい時。 ゲーム作る上では不可欠なのでメモ。
//PlayerPrefsとやらを使う。
//値のセットは
//PlayerPrefs.SetInt("ここ変数名", 入れたい値(int));
//値の取り出しは
//PlayerPrefs.GetInt("ここ変数名");
//値が入ってるかは
//PlayerPrefs.HasKey("ここ変数名");
//でbool値が返ってくる。
//Setするまで値は持てないので、初めて起動するときは例えば、
//"Init" とかのフラグになる値があるか見て、持ってなかったら値をまずセットさせる。
//そうすれば次回起動時からはGetすることができる。

