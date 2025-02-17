using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;
    [SerializeField] private GameObject tasksPrefab;

    [SerializeField] private string stickCollectTask = "Collect sticks ";
    private bool stickTaskComplete = false;
    [SerializeField] private int stickQuantity = 3;
    private int stickCount = 0;
    [SerializeField] private MaterialSO stickSO;
    private GameObject stickTask;

    [SerializeField] private string stoneCollectTask = "Collect stones ";
    private bool stoneTaskComplete = false;
    [SerializeField] private int stoneQuantity = 3;
    private int stoneCount = 0;
    [SerializeField] private MaterialSO stoneSO;
    private GameObject stoneTask;

    [SerializeField] private string leafCollectTask = "Collect leafs ";
    private bool leafTaskComplete = false;
    [SerializeField] private int leafQuantity = 2;
    private int leafCount = 0;
    [SerializeField] private MaterialSO leafSO;
    private GameObject leafTask;

    private bool part1Complete = false;

    [SerializeField] private string storageInteraction = "Store your items";
    private bool storageInteractionComplete = false;
    private GameObject storageTask;

    [SerializeField] private string craftSlingshot = "Craft a slingshot";
    [SerializeField] private WeaponSO slingShotSO;
    private bool craftSlingshotComplete = false;
    private GameObject craftSlingshotTask;

    [SerializeField] private string boatTravel = "Go to the easter island";
    private bool boatTravelComplete = false;
    private GameObject boatTravelTask;

    [SerializeField] private string killMoai = "Destroy the moai sentinel";
    private bool killMoaiComplete = false;
    private GameObject killMoaiTask;

    [SerializeField] private Animator fullBoardAnimator;

    public AudioClip[] TaskSounds;
    private AudioSource audioSource;
    private AudioClip[] audioClips;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivateCollectionTasks();
        audioSource = GetComponent<AudioSource>();
        audioClips = TaskSounds;
    }

    private void ActivateCollectionTasks()
    {
        stickTask = Instantiate(tasksPrefab, transform);
        stickTask.GetComponent<TextMeshProUGUI>().text = stickCollectTask + $" 0/{stickQuantity}";

        stoneTask = Instantiate(tasksPrefab, transform);
        stoneTask.GetComponent<TextMeshProUGUI>().text = stoneCollectTask + $" 0/{stoneQuantity}";

        leafTask = Instantiate(tasksPrefab, transform);
        leafTask.GetComponent<TextMeshProUGUI>().text = leafCollectTask + $" 0/{leafQuantity}";
    }


    private void ActivatePart2Tasks()
    {
        storageTask = Instantiate(tasksPrefab, transform);
        storageTask.GetComponent<TextMeshProUGUI>().text = storageInteraction;

        craftSlingshotTask = Instantiate(tasksPrefab, transform);
        craftSlingshotTask.GetComponent<TextMeshProUGUI>().text = craftSlingshot;

        boatTravelTask = Instantiate(tasksPrefab, transform);
        boatTravelTask.GetComponent<TextMeshProUGUI>().text = boatTravel;

        killMoaiTask = Instantiate(tasksPrefab, transform);
        killMoaiTask.GetComponent<TextMeshProUGUI>().text = killMoai;
    }

    public void UpdateCollectTasks(MaterialSO material)
    {
        if (part1Complete) return;
        Debug.Log(material.ToString());

        if (material == stoneSO && !stoneTaskComplete)
        {
            stoneCount++;
            stoneTask.GetComponent<TextMeshProUGUI>().text = stoneCollectTask + $" {stoneCount}/{stoneQuantity}";
            if (stoneCount >= stoneQuantity)
            {
                stoneTaskComplete = true;
                stoneTask.GetComponent<Animator>().SetBool("TaskComplete", true);
                audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.Play();
            }
        }

        if (material == stickSO && !stickTaskComplete)
        {
            Debug.Log("stick pickedup");
            stickCount++;
            stickTask.GetComponent<TextMeshProUGUI>().text = stickCollectTask + $" {stickCount}/{stickQuantity}";
            if (stickCount >= stickQuantity)
            {
                stickTaskComplete = true;
                stickTask.GetComponent<Animator>().SetBool("TaskComplete", true);
                audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.Play();
            }
        }

        if (material == leafSO && !leafTaskComplete)
        {
            leafCount++;
            leafTask.GetComponent<TextMeshProUGUI>().text = leafCollectTask + $" {leafCount}/{leafQuantity}";
            if (leafCount >= leafQuantity)
            {
                leafTaskComplete = true;
                leafTask.GetComponent<Animator>().SetBool("TaskComplete", true);
                audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.Play();
            }
        }

        if (stoneTaskComplete && stickTaskComplete && leafTaskComplete && !part1Complete)
        {
            ActivatePart2Tasks();
            part1Complete = true;
        }
    }

    public void StorageInteracted()
    {
        if (!part1Complete) return;
        if (storageInteractionComplete) return;
        storageInteractionComplete = true;
        storageTask.GetComponent<Animator>().SetBool("TaskComplete", true);
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();

        CheckPart2Tasks();
    }

    public void CraftedSlingshot(WeaponSO weapon)
    {
        if (!part1Complete) return;
        if(craftSlingshotComplete) return;
        if (weapon != slingShotSO) return;
        craftSlingshotComplete = true;
        craftSlingshotTask.GetComponent<Animator>().SetBool("TaskComplete", true);
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();

        CheckPart2Tasks();
    }

    public void CompleteBoatTask()
    {
        if (!part1Complete) return;
        if(boatTravelComplete) return;
        boatTravelComplete = true;
        boatTravelTask.GetComponent<Animator>().SetBool("TaskComplete", true);
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();

        CheckPart2Tasks();
    }

    public void CompleteMoai()
    {
        if (!part1Complete) return;
        if(killMoaiComplete) return;
        killMoaiComplete = true;
        killMoaiTask.GetComponent<Animator>().SetBool("TaskComplete", true);
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();

        CheckPart2Tasks();
    }

    private void CheckPart2Tasks()
    {
        if(!storageInteractionComplete) return;
        if(!craftSlingshotComplete) return;
        if (!boatTravelComplete) return;
        if (!killMoaiComplete) return;

        fullBoardAnimator.enabled = true;
    }

    [System.Serializable]
    public struct SaveData
    {
        public bool stickTaskComplete;
        public int stickCount;
        public bool stoneTaskComplete;
        public int stoneCount;
        public bool leafTaskComplete;
        public int leafCount;
        public bool part1Complete;
        public bool storageInteractionComplete;
        public bool craftSlingshotComplete;
        public bool boatTravelComplete;
        public bool killMoaiComplete;
    }

    public SaveData GetSaveData()
    {
        SaveData saveData;

        saveData.stickTaskComplete = stickTaskComplete;
        saveData.stickCount = stickCount;
        saveData.stoneTaskComplete = stoneTaskComplete;
        saveData.stoneCount = stoneCount;
        saveData.leafTaskComplete = leafTaskComplete;
        saveData.leafCount = leafCount;
        saveData.part1Complete = part1Complete;
        saveData.storageInteractionComplete = storageInteractionComplete;
        saveData.craftSlingshotComplete = craftSlingshotComplete;
        saveData.boatTravelComplete = boatTravelComplete;
        saveData.killMoaiComplete = killMoaiComplete;

        return saveData;
    }

    public void LoadSaveData(SaveData saveData)
    {
        stickTaskComplete = saveData.stickTaskComplete;
        stickCount = saveData.stickCount;
        stoneTaskComplete = saveData.stoneTaskComplete;
        stoneCount = saveData.stoneCount;
        leafTaskComplete = saveData.leafTaskComplete;
        leafCount = saveData.leafCount;
        part1Complete = saveData.part1Complete;
        storageInteractionComplete = saveData.storageInteractionComplete;
        craftSlingshotComplete = saveData.craftSlingshotComplete;
        boatTravelComplete = saveData.boatTravelComplete;
        killMoaiComplete = saveData.killMoaiComplete;

        if (stickTaskComplete)
        {
            stickTask.GetComponent<TextMeshProUGUI>().text = stickCollectTask + $" {stickCount}/{stickQuantity}";
            stickTask.GetComponent<Animator>().SetBool("TaskComplete", true);
        }

        if (stoneTaskComplete)
        {
            stoneTask.GetComponent<TextMeshProUGUI>().text = stoneCollectTask + $" {stoneCount}/{stoneQuantity}";
            stoneTask.GetComponent<Animator>().SetBool("TaskComplete", true);

            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }

        if (leafTaskComplete)
        {
            leafTask.GetComponent<TextMeshProUGUI>().text = leafCollectTask + $" {leafCount}/{leafQuantity}";
            leafTask.GetComponent<Animator>().SetBool("TaskComplete", true);
        }

        if (part1Complete)
        {
            ActivatePart2Tasks();
        }

        if (storageInteractionComplete)
        {
            storageTask.GetComponent<Animator>().SetBool("TaskComplete", true);
        }

        if (craftSlingshotComplete)
        {
            craftSlingshotTask.GetComponent<Animator>().SetBool("TaskComplete", true);
        }

        if (boatTravelComplete)
        {
            boatTravelTask.GetComponent<Animator>().SetBool("TaskComplete", true);
        }

        if (killMoaiComplete)
        {
            killMoaiTask.GetComponent<Animator>().SetBool("TaskComplete", true);
        }

        CheckPart2Tasks();
    }
}