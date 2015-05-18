using UnityEngine;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour
{
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings("0");
	}

	void OnGUI()
	{
		GUIStyle debugLabelStyle = new GUIStyle(GUI.skin.label);
		debugLabelStyle.normal.textColor = Color.blue;

		GUILayout.Label("ping: " + PhotonNetwork.GetPing().ToString(), debugLabelStyle);
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString(), debugLabelStyle);
		GUILayout.Label(PhotonNetwork.ServerAddress, debugLabelStyle);
	}

	public static void StartMatchmaking()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Cant find any room... Create new room");

		RoomOptions roomOptions = new RoomOptions();
		roomOptions.isOpen = true;
		roomOptions.isVisible = true;
		roomOptions.maxPlayers = 2;

		PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
	}

	void OnJoinedRoom()
	{
		Debug.Log("Joined Room");

		if (PhotonNetwork.room.playerCount == PhotonNetwork.room.maxPlayers)
			Debug.Log("Room filled and ready!");
	}
}
