using UnityEngine;
using System.Collections;

public static class ServerData
{
	public static string userID;
	public static string strReceiveChatting="";
	public static string strSendChatting;
}

public class GUIScript : MonoBehaviour {
	
	public string strID;
	public Rect rectID;
	
	public string strSendChatting;
	public Rect rectSendChatting;
	
	public Rect rectReceiveChatting;
	
	public string strEnter;
	public Rect rectEnter;
	
	// Use this for initialization
	void Awake() {
		strID = "InputYourID";
		rectID = new Rect(Screen.width*0.05f,Screen.height*0.05f,Screen.width*0.3f,Screen.height*0.05f);
		strSendChatting = "Input Send Chatting";
		rectSendChatting = new Rect(Screen.width*0.05f,Screen.height*0.9f,Screen.width*0.7f,Screen.height*0.05f);
		
		//ServerData.strReceiveChatting = new string('a');
		rectReceiveChatting = new Rect(Screen.width*0.05f, Screen.height*0.15f,Screen.width*0.8f,Screen.height*0.7f);
		
		strEnter = "Enter";
		rectEnter = new Rect(Screen.width*0.8f, Screen.height*0.8f,Screen.width*0.15f,Screen.height*0.15f);
		
	}
	
	void OnGUI () {
		if(GUI.Button(rectEnter,strEnter)){
			print(strSendChatting);
			
			ServerData.userID = strID;
			ServerData.strSendChatting = strSendChatting;
			
			COMM_MSG.msg = true;
			COMM_MSG.type = Constants.CHATTING;
			
			strSendChatting ="";
		}
		
		if(Input.GetMouseButtonDown(0)){
			if(IsInVector3InRectOnScreen(Input.mousePosition,rectID)){
				strID = "";
			}
			else if(IsInVector3InRectOnScreen(Input.mousePosition,rectSendChatting)){
				strSendChatting = "";
			}
		}
	
		//	ID
		strID =GUI.TextField(rectID,strID);
		
		//	Send Chatting
		strSendChatting = GUI.TextField(rectSendChatting,strSendChatting);
		
		//	Recieved Chatting
		GUI.TextArea(rectReceiveChatting, ServerData.strReceiveChatting );
		
	}
	
	bool IsInVector3InRectOnScreen(Vector3 position, Rect rect){
		position = new Vector3(position.x,Screen.height-position.y,position.z);
		if(position.x>rect.xMin&& position.x<rect.xMax && position.y>rect.yMin && position.y<rect.yMax){
			return true;
		}
		else{
			return false;
		}
	}
			
			
}
