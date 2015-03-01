using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class NetworkManager : Photon.MonoBehaviour
	{
		private const string roomName = "PongJustu Test Room";
		private RoomInfo[] roomsList;

		void Start()
		{
			PhotonNetwork.ConnectUsingSettings("0");
		}

		void OnGUI()
		{
			GUIStyle cStyle = new GUIStyle(GUI.skin.label);
			cStyle.normal.textColor = Color.red;

			GUILayout.Label("ping: " + PhotonNetwork.GetPing().ToString(), cStyle);

			if (!PhotonNetwork.connected)
			{
				GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString(), cStyle);
			}
			else if (PhotonNetwork.room == null)
			{
				// Create Room
				if (GUI.Button(new Rect(100, 100, 250, 100), "Create Room"))
					PhotonNetwork.CreateRoom(roomName, true, true, 2);

				// Join Room
				if (roomsList != null)
				{
					for (int i = 0; i < roomsList.Length; i++)
					{
						if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join Room: " + roomsList[i].name))
							PhotonNetwork.JoinRoom(roomsList[i].name);
					}
				}
			}
		}

		void OnReceivedRoomListUpdate()
		{
			roomsList = PhotonNetwork.GetRoomList();
		}

		void OnJoinedRoom()
		{
			Debug.Log("Joined to Room");
		}
	}
}
