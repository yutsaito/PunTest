using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
        public void OnClickCharacterPick(int whichCharacter)
    {
        //PlayerInfoのｼﾝｸﾞﾙﾄﾝが存在するか否か
        if (PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySelectedCharacter = whichCharacter; //Set equal to the parameter that's passed in this function
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);  //PlayerPrefsｸﾗｽのｾｯﾄｲﾝﾄﾒｿｯﾄﾞで、変数(名)myCharacterにﾊﾟﾗﾒｰﾀwhichCharacterを代入しろ
        }
    }
}
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
