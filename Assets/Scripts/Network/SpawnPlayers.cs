using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.SceneManagement;
public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private GameObject Player;
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            InstantiatePlayers();
        }
        else
        {
            SceneManager.LoadScene("Loading");
        }
    }
    private void InstantiatePlayers()
    {
        PhotonNetwork.Instantiate(Path.Combine("PlayerPrefabs", "Head"), Vector3.zero, Quaternion.identity, 0);
        PhotonNetwork.Instantiate(Path.Combine("PlayerPrefabs", "Body"), Vector3.zero, Quaternion.identity, 0);
        PhotonNetwork.Instantiate(Path.Combine("PlayerPrefabs", "LeftHandPrefab"), Vector3.zero, Quaternion.identity, 0);
        PhotonNetwork.Instantiate(Path.Combine("PlayerPrefabs", "RightHandPrefab"), Vector3.zero, Quaternion.identity, 0);

        Player.AddComponent<RecognizePlayerFunctions>().view = Player.GetComponent<PhotonView>();
    }
}