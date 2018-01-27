using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat_Spawning_Test : MonoBehaviour {

    public GameObject text_test;
    public InputField player_input;
    private int count = 0;

    //This is a test max to be the cutoff on the top
    private GameObject[] text_list = new GameObject[18];
    //private Stack<KeyValuePair<GameObject, int>> text_stack;


    private string[] names = new string[]{
        "James", "John", "Robert","Michael","William","David","Richard","Joseph","Thomas","Charles","Christopher","Daniel","Matthew","Anthony","Donald","Mark","Paul","Steven","Andrew","Kenneth","George","Joshua","Kevin","Brian","Edward","Ronald","Timothy","Jason",
        "Jeffrey","Ryan","Gary","Jacob","Nicholas","Eric","Stephen","Jonathan","Larry","Justin","Scott","Frank","Brandon","Raymond","Gregory","Benjamin","Samuel","Patrick","Alexander","Jack","Dennis","Jerry","Tyler","Aaron","Henry","Douglas","Jose","Peter","Adam",
        "Zachary","Nathan","Walter","Harold","Kyle","Carl","Arthur","Gerald","Roger","Keith","Jeremy","Terry","Lawrence","Sean","Christian","Albert","Joe","Ethan","Austin","Jesse","Willie","Billy","Bryan","Bruce","Jordan","Ralph","Roy","Noah","Dylan","Eugene","Wayne",
        "Alan","Juan","Louis","Russell","Gabriel","Randy","Philip","Harry","Vincent","Bobby","Johnny","Logan","Mary","Patricia","Jennifer","Elizabeth","Linda","Barbara","Susan","Jessica","Margaret","Sarah","Karen","Nancy","Betty","Lisa","Dorothy","Sandra","Ashley",
        "Kimberly","Donna","Carol","Michelle","Emily","Amanda","Helen","Melissa","Deborah","Stephanie","Laura","Rebecca","Sharon","Cynthia","Kathleen","Amy","Shirley","Anna","Angela","Ruth","Brenda","Pamela","Nicole","Katherine","Virginia","Catherine","Christine",
        "Samantha","Debra","Janet","Rachel","Carolyn","Emma","Maria","Heather","Diane","Julie","Joyce","Evelyn","Frances","Joan","Christina","Kelly","Victoria","Lauren","Martha","Judith","Cheryl","Megan","Andrea","Ann","Alice","Jean","Doris","Jacqueline","Kathryn",
        "Hannah","Olivia","Gloria","Marie","Teresa","Sara","Janice","Julia","Grace","Judy","Theresa","Rose","Beverly","Denise","Marilyn","Amber","Madison","Danielle","Brittany","Diana","Abigail","Jane","Natalie","Lori","Tiffany","Alexis","Kayla", "Ittai", "Mayan",
        "Claudia"};

    private string[] commands = new string[] { "!SpawnEnemies", "!SlowDownPlayer" };
    private string[] crap = new string[] { "You Suck!", "Best Streamer Ever!", "I Love you player 1", "FUKK UU N00B L0l", "I wish people respected you more", "I hate this game play something else", "I could play this so much better than you can", "reeeeeeeeeeeeeeeeeee" };

    // Use this for initialization
    void Start () {
        StartCoroutine(SpamChat());
    }

    private IEnumerator SpamChat()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));


            int rand = Random.Range(0, 100);
            if (rand < 50)
                Add_Message(names[Random.Range(0, 200)] + ": " + commands[Random.Range(0, commands.Length)], false, false);
            else
                Add_Message(names[Random.Range(0, 200)] + ": " + crap[Random.Range(0, crap.Length)], false, false);
        }
    }

    void Update()
    {
        for(int i = 0; i < text_list.Length; ++i)
        {
            if (text_list[i])
                text_list[i].transform.localPosition = new Vector3(0, (i * 20), 0);
        }
    }

    private void Add_Message(string message, bool bold, bool caps)
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
            Add_Message("", false, false);
        }
    }

    public void check_for_enter()
    {
        if(player_input.text.Length > 0)
            if (player_input.text[player_input.text.Length - 1] == '\n')
            {
                Add_Message("Player2: " + player_input.text.Substring(0, player_input.text.Length-1), true, false);
                player_input.text = "";
            }
    }
}
