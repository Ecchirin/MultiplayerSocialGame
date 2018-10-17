using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System;
using System.Runtime.InteropServices;

public static class COMM_MSG
{
	public static int type;
	public static int roomNum;
	public static bool readyCount;
	public static bool msg;
	public static string myId;
	public static bool logOut;
}

static class Constants
{
	public const int CHATTING = 1000;
}

public static class Datas
{
	// Send data write	
	public static StringBuilder login = new StringBuilder();
	public static StringBuilder ready = new StringBuilder();	// Ready
	public static StringBuilder id = new StringBuilder();	
	public static StringBuilder msg = new StringBuilder();	
}

public struct _chatting_packet
{
    public int message_length;
    public char[] message;
};

public class Client : MonoBehaviour {

	public Socket socket;
	
	private int SenddataLength;
	private int ReceivedataLength;
	
	private byte[] Sendbyte;
	private byte[] Receivebyte = new byte[512];
	private string ReceiveString;
	private string [] IdString;	

	//	Server's IP Adrress..
	private string strIP = "127.0.0.1";

	public string tempString;

	public string s1;
	public bool readyOk = false;
    _chatting_packet recvPacket = new _chatting_packet();
		
	void Awake()
	{	
		DontDestroyOnLoad(this);
	}

    void Start()
    {
        recvPacket.message = new char[512];
        recvPacket.message_length = 0;

        try
        {
            IPAddress serverIP = IPAddress.Parse(strIP);
            int serverPort = 7890;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                   ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(serverIP, serverPort);
            socket.Connect(ipep);

            Thread newCounter = new Thread(new ThreadStart(ReadThread));
            newCounter.Start();
        }
        catch (SocketException SCE)
        {
            Debug.Log("Socket connect error!:" + SCE.ToString());
            return;
        }

        try
        {
        }
        catch (SocketException err)
        {
            Debug.Log("Socket send or receive error!:" + err.ToString());
        }
    }

    //void Start()
    //{
    //    try
    //    {
    //        IPAddress serverIP = IPAddress.Parse(strIP);
    //        int serverPort = 7890;
    //        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
    //                               ProtocolType.Udp);
    //        IPEndPoint ipep = new IPEndPoint(serverIP, serverPort);
    //        socket.Connect(ipep);

    //        Thread newCounter = new Thread(new ThreadStart(ReadThread));
    //        newCounter.Start();
    //    }
    //    catch (SocketException SCE)
    //    {
    //        Debug.Log("Socket connect error!:" + SCE.ToString());
    //        return;
    //    }

    //    try
    //    {
    //    }
    //    catch (SocketException err)
    //    {
    //        Debug.Log("Socket send or receive error!:" + err.ToString());
    //    }
    //}

    // Update is called once per frame
    void Update () {
		if(COMM_MSG.msg){
			Thread newCounter2 = new Thread(new ThreadStart(WriteThread));
				newCounter2.Start();
		}
		if(COMM_MSG.logOut == true){
			SendMessage("_OnApplicationQuit", SendMessageOptions.DontRequireReceiver);
		}
	}

	void ReadThread()
	{
		while(true)
        {
            // Receive
            socket.Receive(Receivebyte);
            //Buffer.BlockCopy(Receivebyte, 0, (Array)recvPacket, 0, Receivebyte.Length);
            //Marshal.Copy(Receivebyte, 0, recvPacket, Receivebyte.Length);
            recvPacket = fromBytes(Receivebyte);

            Debug.Log("GET MESSAGE = " + recvPacket);
            //recvPacket = (_chatting_packet)&Receivebyte;
            //ReceiveString = Encoding.Default.GetString(Receivebyte);
            //ReceivedataLength = Encoding.Default.GetByteCount(ReceiveString.ToString());
            //ServerData.strReceiveChatting = string.Format("{0}{1}{2}", ServerData.strReceiveChatting, ReceiveString, '\n');

            //socket.Receive(Receivebyte);
            //ReceiveString = Encoding.Default.GetString(Receivebyte);
            //recvPacket = (_chatting_packet)ReceiveString;
        }
	}
	
	public void WriteThread()
	{
		if(COMM_MSG.type == Constants.CHATTING){
			Datas.msg.Append(string.Format ("{0}:{1}", ServerData.userID, ServerData.strSendChatting));
			SenddataLength = Encoding.Default.GetByteCount(Datas.msg.ToString());
			Sendbyte = Encoding.Default.GetBytes(Datas.msg.ToString());
			socket.Send(Sendbyte, Sendbyte.Length, 0);
			Datas.msg.Remove(0,Sendbyte.Length);
		}
		COMM_MSG.msg = false;
	}
	void _OnApplicationQuit()
	{
		socket.Close();
		socket = null;		
	}
	void OnApplicationQuit()
	{
		socket.Close();
		socket = null;		
	}

    _chatting_packet fromBytes(byte[] arr)
    {
       _chatting_packet str = new _chatting_packet();

        int size = Marshal.SizeOf(recvPacket);
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(arr, 0, ptr, size);

        str = (_chatting_packet)Marshal.PtrToStructure(ptr, str.GetType());
        Marshal.FreeHGlobal(ptr);

        return str;
    }
}
