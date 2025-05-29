using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Material> colorPool = new List<Material>();
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform plane;
    [SerializeField] int idleCt = 3;

    //make a pool of random unique vectors in plane and then randomly place characters into those ((could be a dictionary of vector bool to make sure its not already occupied))
    private List<PlayerController> characters = new List<PlayerController>();
    private PlayerController currentPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateFirstPlayer();

        //float minX = plane.position.x - (plane.localScale.x / 2) + playerPrefab.transform.localScale.x;
        //float maxX = plane.position.x + (plane.localScale.x / 2) - playerPrefab.transform.localScale.x;
        //float minZ = plane.position.z - (plane.localScale.z / 2) + playerPrefab.transform.localScale.z;
        //float maxZ = plane.position.z + (plane.localScale.z / 2) - playerPrefab.transform.localScale.z;
    }

    private void CreateFirstPlayer()
    {
        GameObject newCharacter = Instantiate(playerPrefab, playerPrefab.transform.position, Quaternion.identity);
        currentPlayer = newCharacter.GetComponent<PlayerController>();
        currentPlayer.StartRunning();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
