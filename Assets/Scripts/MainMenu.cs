using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
        //js headers  

        /* MTURK DOESN'T USE GENERATED KEYS
    [DllImport("__Internal")]
    private static extern string ReadData (string tableName,
                                           string token);
    
    [DllImport("__Internal")]
    private static extern string UpdateToken (string tableName,
                                              string token);
                                              */
                                              
    [DllImport("__Internal")]
    private static extern string getScreenWidth();
    [DllImport("__Internal")]
    private static extern string getScreenHeight();
    [DllImport("__Internal")]
    private static extern string getPixelRatio();
    [DllImport("__Internal")]
    private static extern string getBrowserVersion();
    [DllImport("__Internal")]
    private static extern string fullscreenMenuListener();

    public string URL;
    public string WID;
    public InputField TokenField;
    public InputField AgeField;
    public int ageYears;
    public Text errMessage;
    public GameObject consentForm;
    public DateTime consentTime;
    public DateTime startTime;
    public bool genderTicked;
    public ToggleGroup genderToggle;
    public bool handTicked;
    public ToggleGroup handToggle;
    public bool pointerTicked;
    public bool laptopOnetime;
    public ToggleGroup pointerToggle;
    public CameraController cameraController;
    public GameObject Buttons;
    public GameObject Form;
    public GameObject LaptopWarning;

    [System.Serializable]
    public class tokenClass
    {
        public string tokenId;
        public bool available;
        public string gameProgress;
        public int trial;
        public int SSD;
        public int score;
    }

         private List<string> adjectives = new List<string>()
     {
        "Active",
        "Adventurous",
        "Agile",
        "Alert",
        "Amusing",
        "Angry",
        "Annoyed",
        "Athletic",
        "Aware",
        "Bashful",
        "Beaming",
        "Beautiful",
        "Big",
        "Bitter",
        "Blissful",
        "Brave",
        "Brilliant",
        "Busy",
        "Calm",
        "Capable",
        "Cautious",
        "Challenging",
        "Charming",
        "Cheerful",
        "Chilly",
        "Chocolatey",
        "Clever",
        "Cloudy",
        "Compassionate",
        "Considerate",
        "Cozy",
        "Cranky",
        "Creative",
        "Crispy",
        "Crunchy",
        "Dangerous",
        "Daring",
        "Dark",
        "Delicate",
        "Delightful",
        "Ecstatic",
        "Elated",
        "Empty",
        "Endless",
        "Enormous",
        "Entertaining",
        "Equal",
        "Exhausted",
        "Fantastic",
        "Flexible",
        "Fluffy",
        "Freezing",
        "Frenetic",
        "Funny",
        "Furious",
        "Fussy",
        "Generous",
        "Gentle",
        "Gigantic",
        "Glad",
        "Gleeful",
        "Gorgeous",
        "Graceful",
        "Harmonious",
        "Icky",
        "Icy",
        "Infinite",
        "Intelligent",
        "Jaded",
        "Jolly",
        "Jovial",
        "Joyful",
        "Joyous",
        "Jumpy",
        "Kind",
        "Kindly",
        "Knowledgeable",
        "Large",
        "Lazy",
        "Left",
        "Light",
        "Likely",
        "Lousy",
        "Loyal",
        "Lucky",
        "Lumpy",
        "Marvellous",
        "Mean",
        "Minty",
        "Mysterious",
        "Naive",
        "Nervous",
        "New",
        "Nice",
        "Nimble",
        "Optimistic",
        "Oval",
        "Peaceful",
        "Petite",
        "Pleasant",
        "Pleased",
        "Polite",
        "Precise",
        "Pretty",
        "Proud",
        "Quick",
        "Quiet",
        "Rainy",
        "Relaxing",
        "Restful",
        "Right",
        "Serene",
        "Shocking",
        "Short",
        "Simple",
        "Skilful",
        "Slow",
        "Small",
        "Soothing",
        "Sour",
        "Sparkling",
        "Speedy",
        "Spiky",
        "Still",
        "Straight",
        "Strong",
        "Stubborn",
        "Stunning",
        "Sunny",
        "Swift",
        "Tall",
        "Terrified",
        "Thrilled",
        "Timid",
        "Tiny",
        "Tranquil",
        "Tricky",
        "Truthful",
        "Whimsical",
        "Wise",
        "Young"

     };
    private List<string> colours = new List<string>()
    {
        "Black",
        "Blue",
        "Bronze",
        "Brown",
        "Burgundy",
        "Copper",
        "Coral",
        "Crimson",
        "Cyan",
        "Emerald",
        "Fuchsia",
        "Gold",
        "Gray",
        "Green",
        "Indigo",
        "Ivory",
        "Khaki",
        "Lavendar",
        "Lilac",
        "Lime",
        "Magenta",
        "Maroon",
        "Navy",
        "Orange",
        "Peach",
        "Red",
        "Silver",
        "Teal",
        "Turquoise",
        "Violet",
        "White",
        "Yellow"

    };
    private List<string> animals = new List<string>()
    {
        "Alligator",
        "Alpaca",
        "Antelope",
        "Ape",
        "Armadillo",
        "Baboon",
        "Badger",
        "Bat",
        "Bear",
        "Bee",
        "Bison",
        "Boar",
        "Buffalo",
        "Butterfly",
        "Cat",
        "Cattle",
        "Cheetah",
        "Chicken",
        "Cod",
        "Coyote",
        "Crow",
        "Deer",
        "Dinosaur",
        "Dog",
        "Dolphin",
        "Donkey",
        "Dove",
        "Duck",
        "Eagle",
        "Eel",
        "Elephant",
        "Elk",
        "Emu",
        "Falcon",
        "Ferret",
        "Finch",
        "Fish",
        "Flamingo",
        "Fly",
        "Fox",
        "Frog",
        "Gerbil",
        "Giraffe",
        "Goat",
        "Goose",
        "Gorilla",
        "Grasshopper",
        "Grouse",
        "Guineapig",
        "Gull",
        "Hamster",
        "Hawk",
        "Hedgehog",
        "Heron",
        "Hippopotamus",
        "Hog",
        "Hornet",
        "Horse",
        "Hound",
        "Hummingbird",
        "Hyena",
        "Jellyfish",
        "Kangaroo",
        "Koala",
        "Lark",
        "Leopard",
        "Lion",
        "Llama",
        "Magpie",
        "Mallard",
        "Mole",
        "Monkey",
        "Moose",
        "Mosquito",
        "Mouse",
        "Mule",
        "Nightingale",
        "Ostrich",
        "Otter",
        "Owl",
        "Ox",
        "Oyster",
        "Panda",
        "Parrot",
        "Penguin",
        "Pheasant",
        "Pig",
        "Pigeon",
        "Platypus",
        "Porpoise",
        "Possum",
        "Rabbit",
        "Raccoon",
        "Rat",
        "Raven",
        "Reindeer",
        "Rhinoceros",
        "Seal",
        "Shark",
        "Sheep",
        "Snake",
        "Sparrow",
        "Spider",
        "Squid",
        "Squirrel",
        "Swan",
        "Tiger",
        "Toad",
        "Trout",
        "Turkey",
        "Turtle",
        "Walrus",
        "Wasp",
        "Weasel",
        "Whale",
        "Wolf",
        "Wombat",
        "Woodpecker",
        "Wren",
        "Yak",
        "Zebra"
    };

    public void Awake()
    {

    }

    public void Start()
    {
        if(UserInfo.Instance.GameMode != "debug")
        {
            URL = Application.absoluteURL;
            WID = URL.Substring(URL.LastIndexOf('=') + 1);

            UserInfo.Instance.tokenId = WID.ToString();
            GenerateUsername();
        }
        else
        {
            TokenField.text = "debug";
        }

        UserInfo.Instance.consent = false;
        genderTicked = false;
        handTicked = false;
        pointerTicked = false;
        if(UserInfo.Instance.GameMode != "debug")
        {
            UserInfo.Instance.widthPx = getScreenWidth();
            UserInfo.Instance.heightPx = getScreenHeight();
            UserInfo.Instance.pxRatio = getPixelRatio();
            UserInfo.Instance.browserVersion = getBrowserVersion();
            fullscreenMenuListener();
        }
    }

    public void PlayGame() 
    { 
        if((UserInfo.Instance.GameMode != "debug" && UserInfo.Instance.tokenId != "notryan") && (UserInfo.Instance.consent == false || !handTicked || !pointerTicked || !genderTicked || ageYears < 18))
        {
            if(ageYears < 18)
            {
                errMessage.text = "You must be 18 years or older to participate";
            }
            if(!genderTicked)
            {
                errMessage.text = "You must indicate your gender before participating in the experiment";
            }
            if(!handTicked)
            {
                errMessage.text = "You must select your preferred hand before participating in the experiment";
            }
            else if(!pointerTicked)
            {
                errMessage.text = "You must select your pointer device before participating in the experiment";
            }
            else if(UserInfo.Instance.consent == false)
            {
                errMessage.text = "You must provide your consent before participating in the experiment";
            }
        }
        else
        {
            if(UserInfo.Instance.GameMode != "debug" && UserInfo.Instance.tokenId != "notryan")
            {
                //ReadData(UserInfo.Instance.TokenTable, UserInfo.Instance.tokenId.ToString());
                SceneManager.LoadScene("SettingsMenu"); //MTURK ONLY - REMOVE FOR OTHER BUILDS
            }
            else
            {
                SceneManager.LoadScene("SettingsMenu");
            }
        }
 
    }

    public void ViewConsent()
    {
        Screen.fullScreen = true;
        cameraController.ScaleFullScreenCamera();
        consentForm.SetActive(true);
        Buttons.SetActive(false);
        Form.SetActive(false);
    }
    public void GiveConsent()
    {
        UserInfo.Instance.consent = true;
        UserInfo.Instance.consentTime = DateTime.Now;
        errMessage.text = "";
        consentForm.SetActive(false);
        Buttons.SetActive(true);
        Form.SetActive(true);
    }

    public void RefuseConsent()
    {
        UserInfo.Instance.consent = false;
        consentForm.SetActive(false);
        Buttons.SetActive(true);
        Form.SetActive(true);
    }
    
    /* MTURK GENERATED AUTOMATICALLY

    public void GetToken()
    {
        UserInfo.Instance.tokenId = TokenField.text.ToString();
    }
    */

    public void GenerateUsername()
    {
        // generate a random number between 1 and the max number of username combos
        System.Random rnd = new System.Random();
        int usernameNum = rnd.Next(1, adjectives.Count * colours.Count * animals.Count);
        
        string name = GetNthUsername(usernameNum, adjectives, colours, animals);
        // get the username for this number and assign it to the username textbox
        TokenField.text = name;

        UserInfo.Instance.username = name;
    }

    /// Get the n-th unique user name constructed using one element from each of the given lists in order
  public static string GetNthUsername(int n, List<string> pt1, List<string> pt2, List<string> pt3)
  {
    // The maximum number of unique names possible
    int maxNames = pt1.Count * pt2.Count * pt3.Count;
    // If n is bigger than the maximum number of names (minus 1), add a counter to the end of the generated name
    int overflowLoops = n / maxNames;
    string overflowId = overflowLoops > 0 ? "-"+overflowLoops.ToString() : "";

    // Get the correct index for each list, given the current ID number n
    n = n % maxNames;
    int index1 = n / (pt2.Count * pt3.Count);
    int index2 = (n - index1 * pt2.Count * pt3.Count) / pt3.Count;
    int index3 = (n - (index1 * pt2.Count * pt3.Count) - (index2 * pt3.Count));

    return pt1[index1] + "-" + pt2[index2] + "-" + pt3[index3] + overflowId;
  } 

    public void GetAge()
    {
        ageYears = Convert.ToInt32(AgeField.text);
        UserInfo.Instance.age = AgeField.text.ToString();
    }

    public void GenderFemale()
    {
        UserInfo.Instance.gender = "female";
        genderTicked = true;
        genderToggle.allowSwitchOff = false;
    }
    public void GenderMale()
    {
        UserInfo.Instance.gender = "male";
        genderTicked = true;
        genderToggle.allowSwitchOff = false;
    }

    public void HandLeft()
    {
        UserInfo.Instance.handedness = "left";
        handTicked = true;
        handToggle.allowSwitchOff = false;
    }

    public void HandRight()
    {
        UserInfo.Instance.handedness = "right";
        handTicked = true;
        handToggle.allowSwitchOff = false;
    }

    public void PointerMouse()
    {
        UserInfo.Instance.pointer = "mouse";
        pointerTicked = true;
        pointerToggle.allowSwitchOff = false;
    }

    public void PointerTrackpad()
    {
        UserInfo.Instance.pointer = "trackpad";
        pointerTicked = true;
        pointerToggle.allowSwitchOff = false;
        if(!laptopOnetime)
        {
            LaptopWarning.SetActive(true);
            laptopOnetime = true;
        }
    }

    public void ConfirmLaptop()
    {
        LaptopWarning.SetActive(false);
    }

        // This is called in the ReadData() .js function, supplying the requested data as its argument. Checks token table for existence and completion
/* NOT USED IN MTURK BUILD
    public void StringCallback (string reqData)
    {
        tokenClass tokenObject = JsonUtility.FromJson<tokenClass>(reqData);
        //if available or incomplete progess start the experiment 
        if(tokenObject.available == true || tokenObject.gameProgress != "complete")
        {
            UpdateToken(UserInfo.Instance.TokenTable, 
                        UserInfo.Instance.tokenId.ToString());
            //cursor was locked here
            UserInfo.Instance.gameProgress = tokenObject.gameProgress;
            UserInfo.Instance.trialProgress = tokenObject.trial;
            UserInfo.Instance.SSD = tokenObject.SSD;
            UserInfo.Instance.score = tokenObject.score;
            SceneManager.LoadScene("SettingsMenu");
        }
        else if(tokenObject.available == false)
        {
            errMessage.text = "The token you have entered has already been used";
        }
    }
    */

    private void ToggleFullscreen(string stateReq)
    {
        if(stateReq == "fullscreen")
        {
            UserInfo.Instance.fullscreen = true;
        }
        else if(stateReq == "exitFullscreen")
        {
            UserInfo.Instance.fullscreen = false;
        }
    }

    public void ErrorCallback(string error)
        {
            errMessage.text = error;
        }
 
}
