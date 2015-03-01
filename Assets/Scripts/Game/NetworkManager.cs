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
			GUIStyle debugLabelStyle = new GUIStyle(GUI.skin.label);
			debugLabelStyle.normal.textColor = Color.blue;

			GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

			GUILayout.Label("ping: " + PhotonNetwork.GetPing().ToString(), debugLabelStyle);
			GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString(), debugLabelStyle);

			if (PhotonNetwork.connected && !PhotonNetwork.inRoom)
			{
				if (GUI.Button(new Rect(Screen.width / 2f - 75, Screen.height / 3f - 15, 150, 30), "Start Online Multiplayer", buttonStyle))
					ConnectWithPlayer();
			}
		}

		public void ConnectWithPlayer()
		{
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.isOpen = true;
			roomOptions.isVisible = true;
			roomOptions.maxPlayers = 2;

			TypedLobby typedLobby = new TypedLobby();
			typedLobby.Name = roomName;

			PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
		}

		void OnJoinedRoom()
		{
			if (PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers)
				Debug.Log("Room Ready");
		}
	}
}
