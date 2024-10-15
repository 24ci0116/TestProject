using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //LoadSceneを使うために必要

public class TitleDirector : MonoBehaviour
{
    void Update()
    {
        //if (Input.GetMouseButtonDown(0)) //マウスがクリックされたら
        if (Input.GetKeyDown(KeyCode.Space))//スペースが押されたら
        {
            SceneManager.LoadScene("GameScene"); //GameSceneに移動する
        }
    }
}
