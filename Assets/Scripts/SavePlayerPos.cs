using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayerPos : MonoBehaviour
{
    public GameObject player;
    public List<float> player_y_list;

    void Start () {
    InvokeRepeating("Player_Y", 0f, 0.25f);  //1s delay, repeat every 1s
    }

    void Player_Y() {
        float pY = player.transform.position.y;
        player_y_list.Add(pY);
    }
}
