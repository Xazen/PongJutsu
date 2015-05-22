using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class MultiplayerManager : Photon.MonoBehaviour
{	
	public static void ConnectPhoton()
	{
		Debug.Log("Connect to Photon");

		if (!PhotonNetwork.connected)
			PhotonNetwork.ConnectUsingSettings("0");
	}

	public static void DisconnectPhoton()
	{
		Debug.Log("Disconnect from Photon");

		if (PhotonNetwork.connected)
			PhotonNetwork.Disconnect();
	}

	public static bool onlineMode
	{
		get
		{
			return !PhotonNetwork.offlineMode;
		}
		set
		{
			if (value == true)
			{
				PhotonNetwork.offlineMode = false;

				ConnectPhoton();
			}
			else
			{
				DisconnectPhoton();

				PhotonNetwork.offlineMode = true;
			}
		}
	}

	public static bool CanControlFaction(Faction faction)
	{
		if (!onlineMode)
			return true;

		return ((PhotonNetwork.isMasterClient && faction == Faction.Left) || (!PhotonNetwork.isMasterClient && faction == Faction.Right)) ? true : false;
	}

	public static bool isMasterClient
	{
		get { return PhotonNetwork.isMasterClient; }
	}

	void OnGUI()
	{
		GUIStyle debugLabelStyle = new GUIStyle(GUI.skin.label);
		debugLabelStyle.normal.textColor = Color.blue;

		GUILayout.Label("ping: " + PhotonNetwork.GetPing().ToString(), debugLabelStyle);
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString(), debugLabelStyle);
		GUILayout.Label(PhotonNetwork.ServerAddress, debugLabelStyle);
	}

	public static void StartOnlineMultiplayer()
	{
		onlineMode = true;

		PhotonNetwork.JoinRandomRoom();
	}

	public static void StartLocalMultiplayer()
	{
		onlineMode = false;

		RoomOptions roomOptions = new RoomOptions();
		roomOptions.isOpen = false;
		roomOptions.isVisible = false;
		roomOptions.maxPlayers = 1;

		PhotonNetwork.CreateRoom("Local", roomOptions, TypedLobby.Default);
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
		{
			Debug.Log("Room filled and ready!");
			photonView.RPC("EnterGame", PhotonTargets.AllViaServer);
		}
	}

	[RPC]
	void EnterGame()
	{
		GameManager.EnterGame();
	}
}
