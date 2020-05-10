using UnityEngine;
using UnityEngine.UI;
using System;

public class SelAngar : MonoBehaviour
{

    public GameObject Grafic;
    public GameObject Planes;
    public GameObject ButR;
    public GameObject ButL;

    public void ButClick()
    {
        if (Convert.ToInt32(this.gameObject.transform.GetChild(2).GetComponent<Text>().text) != 0)
        {
            GameObject.Find("Client").GetComponent<Client>().UPDP(Convert.ToInt32(this.gameObject.transform.GetChild(0).GetComponent<Text>().text));           
        }
    }
    public void ButClickR()
    {
        Planes.active = false;
        Grafic.active = true;
        ButR.active = false;
        ButL.active = true;
        
    }
    public void ButClickL()
    {
        ButL.active = false;
        ButR.active = true;
        Grafic.active = false;
        Planes.active = true;
    }
}
