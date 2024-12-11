using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    [SerializeField] private GameObject tasksPrefab;

    [SerializeField] private string stickCollectTask = "Collect sticks ";
    [SerializeField] private int stickQuantity = 3;
    private GameObject stickTask;
    [SerializeField] private string stoneCollectTask = "Collect stones ";
    [SerializeField] private int stoneQuantity = 3;
    private GameObject stoneTask;
    [SerializeField] private string leafCollectTask = "Collect leafs ";
    [SerializeField] private int leafQuantity = 2;
    private GameObject leafTask;
    [SerializeField] private string storageInteraction = "Store your items";
    private GameObject storageTask;
    [SerializeField] private string craftSlingshot = "Craft a slingshot";
    private GameObject craftSlingshotTask;
    [SerializeField] private string boatTravel = "Go to the easter island";
    private GameObject boatTravelTask;
    [SerializeField] private string killMoai = "Destroy the moai sentinel";
    private GameObject killMoaiTask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stickCollectTask += $" 0/{stickQuantity}";
        stoneCollectTask += $" 0/{stoneQuantity}";
        leafCollectTask += $" 0/{leafQuantity}";

        stickTask = Instantiate(tasksPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTasks()
    {

    }


}
