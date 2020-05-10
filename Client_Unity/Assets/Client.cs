using System.Collections;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

public class Client : MonoBehaviour
{ 
    [Header("Ячейки судов")]
    public GameObject Pcell;
    public GameObject PContainer;
    [Header("Ячейки ангаров")]
    public GameObject Acell;
    public GameObject AContainer;
    [Header("Графика судов")]
    public GameObject Qplane;
    public GameObject Grafic;
    [Header("Редакт. позиций и отступа")]
    public float otctupX;
    public float otctupY;
    [Space]
    public float umnojit;
    [Header("Изменение размера")]
    public float skX;
    public float skY;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private string message;

    public void ConnectedToServer()
    {
        if (socketReady)
            return;

        string host = "127.0.0.1";
        int port = 2845;

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }

    private void Start()
    {
        //ConnectedToServer();
    }
 
    private void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                byte[] buffer = new byte[1024];
                socket.Client.Receive(buffer);

                message = Encoding.UTF8.GetString(buffer);
                OnIncomingData();
                if (message != null)
                {
                    Debug.Log(message);
                }
            } 

        }
    }

    public void Proba()
    {
        message = GameObject.Find("TextPr").GetComponent<Text>().text;
        OnIncomingData();
    }


    private void OnIncomingData()
    {
        if(message.Contains("RUPDA"))
        {
            Match regex = Regex.Match(message, "RUPDA:(.*):(.*):(.*):(.*):(.*)");//RUPDA:id:name:Pcount:длина:ширина
            Debug.Log(message);
            ///
            string id = regex.Groups[1].Value;
            string name = regex.Groups[2].Value;
            string Pcount = regex.Groups[3].Value;
            string length = regex.Groups[4].Value;
            string width = regex.Groups[5].Value;

            Debug.Log(id);
            Debug.Log(name);
            Debug.Log(Pcount);
            ///
            GameObject Ac = Instantiate(Acell, AContainer.transform);
            Ac.transform.GetChild(0).GetComponent<Text>().text = id;
            Ac.transform.GetChild(1).GetComponent<Text>().text = name;
            Ac.transform.GetChild(2).GetComponent<Text>().text = Pcount;
            ///
        }
        if(message.Contains("RUPDP"))
        {
            Match regex = Regex.Match(message, "RUPDP:(.*):(.*):(.*):(.*):(.*):(.*):(.*):(.*):(.*):(.*):(.*):(.*)");//RUPDP:id:name:x:y:startTime:endtime:time:status:длина:ширина x12
            ///
            string id = regex.Groups[1].Value;
            string name = regex.Groups[2].Value;
            int X = Convert.ToInt32(regex.Groups[3].Value);
            int Y = Convert.ToInt32(regex.Groups[4].Value);
            string StartTime = regex.Groups[5].Value;
            string FinishTime = regex.Groups[6].Value;
            string Time = regex.Groups[7].Value;
            int length = Convert.ToInt32(regex.Groups[8].Value);
            int width = Convert.ToInt32(regex.Groups[9].Value);
            string Money = regex.Groups[10].Value;
            string OneDayMoney = regex.Groups[11].Value;
            string ErrorMoney = regex.Groups[12].Value;

            ///
            GameObject Pc = Instantiate(Pcell, PContainer.transform);
            Pc.transform.GetChild(0).GetComponent<Text>().text = id;
            Pc.transform.GetChild(1).GetComponent<Text>().text = name;
            Pc.transform.GetChild(2).GetComponent<Text>().text = Money;/////
            Pc.transform.GetChild(3).GetComponent<Text>().text = Time;
            ///
            GameObject plane = Instantiate(Qplane, Grafic.transform);
            plane.transform.localScale = new Vector3(width * skX, length * skY);
            plane.transform.position = new Vector3(X * umnojit + otctupX, Y * umnojit + otctupY);       
        }        
    }

    private void Send(string data)
    {
        if (!socketReady)
            return;

        socket.Client.Send(Encoding.UTF8.GetBytes(data));
        writer.Flush();
    }

    private void CloseSocket()
    {
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }
}
