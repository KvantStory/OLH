using UnityEngine;
using UnityEngine.UI;
using System;

public class SelAngar : MonoBehaviour
{

    public void ButClick()
    {
        if (Convert.ToInt32(this.gameObject.transform.GetChild(2).GetComponent<Text>().text) != 0)
        {
            GameObject.Find("Client").GetComponent<Client>().UPDP(Convert.ToInt32(this.gameObject.transform.GetChild(0).GetComponent<Text>().text));           
        }
    }
}
