using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat_Controller : MonoBehaviour
{

    [Header("Chat Objects")]
    public GameObject text_test;
    public InputField player_input;
    public GameObject command_panel;
    public Text current_viewers;

    private int count = 0;
    [Header("Game Manager")]
    public GameManager gm;

    [Header("MinMaxSettings")]
    public float minWaitAtLowScore = 20.0f;
    public float maxWaitAtLowScore = 30.0f;
    public float minWaitAtHighScore = 0.5f;
    public float maxWaitAtHighScore = 1.0f;
    public float highScore = 10000.0f;
    public int minViewers = 0;
    public int maxViewers = 0;
    public int randomChanceOfCommand = 25;

    //This is a test max to be the cutoff on the top
    private GameObject[] text_list = new GameObject[18];
    //private Stack<KeyValuePair<GameObject, int>> text_stack;


    private string[] names = new string[]{
        "jetsontwo", "ArcticLazer31", "MoonrakerOxide", "Cedart32endril", "mediancrafty9", "oraxiom", "Jaialaifly", "coughConsent12", "LIONECLAIR", "dimwittedWand", "Accretionstuff", "MerlomMutter",
    "SpinnerVenereal65", "Figet123", "WhomFad", "Faropens231", "GoldMEDAL12", "Stunsailarid", "MotionlessNepture", "beerRecondite32", "Featherboots", "LegBonelli", "YieldingEuler5", "BeneficentPaperclip",
        "StarSeparately", "PungentForget52", "YAML_The_Camel", "HealBeginner", "ConicalMug", "TINTJOLT", "Festival_SSteady", "Universe_of_Brocolli", "cried_bookmark", "Envelope_Clink", "Fax_Busstop",
    "Musteringchalk6234", "LocationDuffer__21", "ElandProcessing1", "Toyotateachers", "Bint_Provide", "stockingsresource", "ROFL", "Nappies_called_son", "Secure_Favorites", "Lustrouspeppery", "sidlaws",
    "For_eleverate", "MasculineSalinity23", "ShoppingHoop", "EarthyDeclare", "SortDiorite213", "Mincraftr5123", "CliveHicky11", "PowderResigned", "Integers_12_seat", "vapidadult1337", "pizzblazerod"};


    //The datatype for a command, has access to the string command that will show up in chat as well as the function it will activate
    private struct CommandData
    {
        public CommandData(string mes, string function)
        {
            message = mes;
            function_name = function;
        }
        public string message;
        public string function_name;
    }

    //This will be the list of commands and the functions they call for both the player and the AI agents (AI Agents can only access 1 - 3)
    /*
      1  Spawn brawler & Healing
      2  player moves faster, Enemies do more damage
      3  Stream quality & spawn shooters
      4  Player double damage, player moves slower, enemies move slower
      5  Lag, Bullet upgrades and traps
      6  Temp invincibility
      7  Boss spawning
     */
    private CommandData[] commands = new CommandData[] { new CommandData("!SpawnRaptors", "spawnraptor"), new CommandData("!LagPlayer", "lag") };

    private string[] useless_messages = new string[] { "You Suck!", "Best Streamer Ever!", "I Love you player1", "uu n00b l0l", "I wish people respected you more", "I hate this game play something else", "I could play this so much better than you can", "reeeeeeeeeeeeee",
    "<message deleted>"};
    private string[] colors = new string[] { "aqua", "blue", "brown", "red", "yellow", "navy", "orange", "purple", "lime", "green", "magenta", "maroon" };

    // Use this for initialization
    void Start()
    {
        if (maxViewers == 0)
            maxViewers = Random.Range(1000000, 9999999);
        player_input.Select();
        StartCoroutine(SpamChat());
    }

    private IEnumerator SpamChat()
    {
        while (true)
        {
            //float ratio = Mathf.Clamp(gm.score / highScore, 0.0f, 1.0f);

            //float minWait = Mathf.Lerp(minWaitAtLowScore, minWaitAtHighScore, ratio);
            //float maxWait = Mathf.Lerp(maxWaitAtLowScore, maxWaitAtHighScore, ratio);

            //Should change the wait depending on the score of the game, higher score = less wait between messages
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            //yield return new WaitForSeconds(Random.Range(minWait, maxWait));


            int rand = Random.Range(0, 100);
            if (rand < randomChanceOfCommand)
            {
                CommandData com = commands[Random.Range(0, commands.Length)];
                Add_Message("<color=" + colors[Random.Range(0, colors.Length)] + ">" + names[Random.Range(0, names.Length)] + "</color>" + ": " + com.message, false);
                gm.SendCommand(com.function_name);
                //Call the function in the Game Manager using call_func(commands.function_name)***********************************************************************
            }
            else
                //Sends the random messages, also makes them somtimes upper case to mimic the real internet
                Add_Message("<color=" + colors[Random.Range(0, colors.Length)] + ">" + names[Random.Range(0, names.Length)] + "</color>" + ": " + (rand > 80 ? useless_messages[Random.Range(0, useless_messages.Length)].ToUpper() : useless_messages[Random.Range(0, useless_messages.Length)]), false);
        }
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Tab))
            command_panel.SetActive(true);
        else
            command_panel.SetActive(false);

        current_viewers.text = Mathf.Lerp(minViewers, maxViewers, gm.score / highScore).ToString();

        for (int i = 0; i < text_list.Length; ++i)
        {
            if (text_list[i])
                text_list[i].transform.localPosition = new Vector3(0, (i * 20), 0);
        }
    }

    private void Add_Message(string message, bool bold)
    {
        for (int i = text_list.Length - 1; i > 0; --i)
        {
            if (text_list[i - 1])
            {
                //Means its the last one
                if (i == text_list.Length - 1)
                {
                    Destroy(text_list[i]);
                    text_list[i] = text_list[i - 1];
                }
                text_list[i] = text_list[i - 1];
            }

        }
        text_list[0] = Instantiate(text_test, transform);

        //Heres where you can set the string
        Text text_obj = text_list[0].GetComponent<Text>();
        text_obj.text = message;
        if (bold)
            text_obj.fontStyle = FontStyle.Bold;

        //This is for handling multi-lines with the username being a different color
        //Canvas.ForceUpdateCanvases();
        //if(text_obj.cachedTextGenerator.lines.Count > 1)
        //{
        //    int next_line_index = text_obj.cachedTextGenerator.lines[1].startCharIdx;
        //    Add_Message(text_obj.text.Substring(next_line_index, text_obj.text.Length - next_line_index), bold, caps);
        //    text_obj.text = text_obj.text.Substring(0, next_line_index);
        //}

        //Dont want to have to do this but doing it for now
        Canvas.ForceUpdateCanvases();
        int empty_line_num = text_obj.cachedTextGenerator.lineCount;
        //This adds the extra breaks in the lines
        for (int i = 1; i < empty_line_num; ++i)
        {
            Add_Message("", false);
        }
    }

    public void check_for_enter()
    {
        if (player_input.text.Length > 0)
        {
            if (player_input.text[player_input.text.Length - 1] == '\n')
            {
                if (player_input.text.Length - 1 > 0)
                    Add_Message("<color=white>" + "Player2" + ": " + player_input.text.Substring(0, player_input.text.Length - 1) + "</color>", true);
                player_input.text = "";
            }
            else if (player_input.text[player_input.text.Length - 1] == '\t')
            {
                player_input.text = player_input.text.Substring(0, player_input.text.Length - 1);
            }
        }

    }
}
