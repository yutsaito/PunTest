using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView PV;
    private CharacterController myCC;   //CharacterControllerというのは(おそらく)Photonに記述されているｸﾗｽで、ｷｬﾗｸﾀの動きに関するｺｰﾄﾞが入ってる、今回Avatorにｱﾀｯﾁしてある
    public float movementSpeed;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();        //ｺﾝﾎﾟｰﾈﾝﾄであるPhotonViewをｹﾞｯﾄして、変数PVに入れよ
        myCC = GetComponent<CharacterController>();     //ｺﾝﾎﾟｰﾈﾝﾄであるCharacterControllerをgetして、myCCにいれよ
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            BasicMovement();
            BasicRotation();
        }
    }

    void BasicMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            myCC.Move(transform.forward * Time.deltaTime * movementSpeed);  //CharacterControllerのmyCCのMoveﾌﾟﾛﾊﾟﾃｨ(ﾒｯｿｯﾄﾞかも)に、前進せよ×ｷｰ押されてる時間×移動速度を設定せよ
        }

        if (Input.GetKey(KeyCode.A))
        {
            myCC.Move(-transform.right * Time.deltaTime * movementSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            myCC.Move(-transform.forward * Time.deltaTime * movementSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            myCC.Move(transform.right * Time.deltaTime * movementSpeed);
        }
    }

    void BasicRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        transform.Rotate(new Vector3(0, mouseX, 0));
    }

}
//Just to be clearer: Not shown was he attached the  PlayerMovement to the prefab named PlayerAvatar 
//and gave the public variables: Movement Speed and Rotation Speed a value. anything over 0 will work 
//(if you don't set it ..its default is '0" and it won't move) he never skips a beat though many do  
//but not on his videos..this was a first I have seen a semi-oversight. I blame it on video editing! :)﻿
