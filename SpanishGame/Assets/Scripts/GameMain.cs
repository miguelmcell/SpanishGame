using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMain : MonoBehaviour {
    int p1Score;
    int p2Score;
    public ButtonHandler bh;
    public Text p1;
    public Text p2;//should probably show a tutorial of the game
    public Text timer;
    public GameObject instructor;
    public GameObject leaderboard;
    int phase;//0 = startup everything on the screen, 1 = show up the question, 2 = start counting to 0 to pop up the answers, 3 = results, wait till a response from player, if nothing for 10 seconds, go on to the next quesiton
    public bool runningPhase;
    public bool waitingInput;
    int currentButton;
    public List<Question> questions;
    public int curQuestion;
    
    //tool pouch
    //StartCoroutine("CountDown");
    //


    // Use this for initialization
    void Start () {//888888888888888888888888888888888888888888888888888888Next step is to hide the example and options before the timer finishes
        curQuestion = 0;
        currentButton = -1;
        timer.text = "3";
        p1Score = 0;
        p2Score = 0;
        waitingInput = false;
        //RoundWinner(2);
        phase = 0;
        runningPhase = false;
        questions.Add(new Question("Fill in the blank in the sentence", new List<string> { "llamas" , "idk", "loss"}, 0,1, "Hola como te") );
        questions.Add(new Question("this is new,lol, society?", new List<string> { "soap", "paper", "yep" }, 1,1));
    }
	
	// Update is called once per frame
	void Update () {
        //print("phase value: " + phase + ", runningPhase value: " + runningPhase);
        if (runningPhase == false)
        {
            //print("herro with phase: " + phase);
            if (phase == 0)
            {
                loadQuestion();
                StartCoroutine("ShowQuestion");
            }
            else if (phase == 1)
            {
                StartCoroutine("CountDown");
            }
            else if (phase == 2)
            {
                StartCoroutine("Play");
            }
            else if (phase == 3)
            {
                StartCoroutine("Results");
            }
            else if (phase == 4)
            {
                //print("starting next question");
                StartCoroutine("NextQuestion");
            }
            else if(phase == 5)
            {
                StartCoroutine("End");
                print("sup");
            }
            runningPhase = true;
        }
    }
    void loadQuestion()
    {
        instructor.GetComponentInChildren<Text>().text = questions[curQuestion].question;

        bh.p1B1.gameObject.GetComponentInChildren<Text>().text = questions[curQuestion].choices[0];
        bh.p1B2.gameObject.GetComponentInChildren<Text>().text = questions[curQuestion].choices[1];
        bh.p1B3.gameObject.GetComponentInChildren<Text>().text = questions[curQuestion].choices[2];

        Transform options = instructor.transform.parent.GetChild(7);

        options.GetChild(0).GetComponentInChildren<Text>().text = questions[curQuestion].choices[0];
        options.GetChild(1).GetComponentInChildren<Text>().text = questions[curQuestion].choices[1];
        options.GetChild(2).GetComponentInChildren<Text>().text = questions[curQuestion].choices[2];

        bh.p2B1.gameObject.GetComponentInChildren<Text>().text = questions[curQuestion].choices[0];
        bh.p2B2.gameObject.GetComponentInChildren<Text>().text = questions[curQuestion].choices[1];
        bh.p2B3.gameObject.GetComponentInChildren<Text>().text = questions[curQuestion].choices[2];
    }
    IEnumerator End()
    {
        bool done = false;
        string stat = "";
        if (p1Score > p2Score)
            stat = "Player 1 wins";
        else if (p2Score > p1Score)
        {
            stat = "Player 2 wins";
        }
        else
        {
            stat = "Its a draw";
        }
        instructor.transform.parent.GetChild(5).GetComponentInChildren<Text>().text = stat;
        //float timer = 0;
        while (!done)//while question is not in the middle youre going to keep going towards it
        {
            //go towards 0 from 285, then from 285 to -285
            //yield return null;
            if (instructor.transform.parent.GetChild(5).GetComponent<RectTransform>().anchoredPosition.y > 0)//470****************-0.05373612
                instructor.transform.parent.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.Lerp(instructor.transform.parent.GetChild(5).GetComponent<RectTransform>().anchoredPosition.y, -5, 0.05f));
            else if (instructor.GetComponent<RectTransform>().anchoredPosition.y <= 0)
            {
                //print("now counting see - " + timer);
                done = !done;
                //timer += Time.deltaTime;
            }
            //if (instructor.GetComponent<RectTransform>().anchoredPosition.y <= -280)
            //{
                //print("Cat is in the bag!");
                
            //}
            //if (timer > 2)
            //{
             //   instructor.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.Lerp(instructor.GetComponent<RectTransform>().anchoredPosition.y, -300, 0.03f));
              //  //print("timer is above 5");
            //}
            yield return new WaitForSeconds(0.01f);

        }
        //----------------------experimental
        //phase = 1;
        //runningPhase = false;
        //-----------------------
        yield return null;
    }
    IEnumerator NextQuestion()
    {
        //reset values
        currentButton = -1;
        timer.text = "3";
        timer.color = Color.green;
        instructor.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 300);

        bool done = false;
        while (!done)
        {//do while timer is not 0 then keep 
            curQuestion++;//will have to check if theres still more before incrementing
            yield return new WaitForSeconds(0.005f);
            //yield return new WaitForSeconds(0.01f);
            done = !done;
        }
        phase = 0;
        //print("exiting play");
        runningPhase = false;
        yield return null;
    }
    IEnumerator Results()
    {
        //print("starting play");
        bool done = false;
        while (!done)
        {//do while timer is not 0 then keep 
            if((currentButton % 3)  == questions[curQuestion].correntAns)
            {
                //so its correct, now which player got it correct?
                if (currentButton < 3)
                    RoundWinner(1);
                else
                    RoundWinner(2);
            }
            else
            {
                if (currentButton < 3)
                    Wrong(1);
                //  player 1 got it wrong!  
                else
                    // player 2 got it wrong!
                    Wrong(2);
            }
            yield return new WaitForSeconds(2f);
            done = !done;
            //yield return new WaitForSeconds(0.01f);
        }

        if (curQuestion >= questions.Count-1)//its because it hasnt been added 1 to curQuestion
        {
            phase = 5;
        }
        else
        {
            phase = 4;
        }
        //print("exiting results");
        runningPhase = false;
        yield return null;
    }
    public void getInput(int buttonID)
    {
        if (waitingInput)
        {
            print("Succesfully got button " + buttonID);
            currentButton = buttonID;
            waitingInput = false;
        }
        else
        {
            print("Input did not register");
        }
        
    }
    IEnumerator Play()
    {
        //print("starting play");
        bool done = false;
        waitingInput = true;
        while (!done)
        {//do while timer is not 0 then keep 
            //print("sup");
            
            yield return new WaitForSeconds(0.005f);
            if (waitingInput == false)//means we got our input
            {
                //print("Button " + currentButton + " was first to pick!");
                done = !done;
            }
        }
        phase = 3;
        //print("exiting play");
        runningPhase = false;
        yield return null;
    }
    IEnumerator ShowQuestion()
    {
        bool done = false;
        float timer = 0;
        while (!done)//while question is not in the middle youre going to keep going towards it
        {
            //go towards 0 from 285, then from 285 to -285
            //yield return null;
            if(instructor.GetComponent<RectTransform>().anchoredPosition.y > 5)
                instructor.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.Lerp(instructor.GetComponent<RectTransform>().anchoredPosition.y, 0, 0.03f));
            else if (instructor.GetComponent<RectTransform>().anchoredPosition.y <= 5)
            {
                //print("now counting see - " + timer);
                
                timer += Time.deltaTime;
            }
            if (instructor.GetComponent<RectTransform>().anchoredPosition.y <= -280)
            {
                //print("Cat is in the bag!");
                done = !done;
            }
            if (timer > 2)
            {
                instructor.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.Lerp(instructor.GetComponent<RectTransform>().anchoredPosition.y, -300, 0.03f));
                //print("timer is above 5");
            }
            yield return new WaitForSeconds(0.01f);
            
        }
        //----------------------experimental
        phase = 1;
        runningPhase = false;
        //-----------------------
        yield return null;
    }
    IEnumerator CountDown()
    {
        bool done = false;
        timer.gameObject.GetComponent<AudioSource>().Play();
        while (!done) {//do while timer is not 0 then keep 
            //yield return null;
            yield return new WaitForSeconds(1f);
            timer.text = (int.Parse(timer.text) - 1).ToString();
            if(timer.text == "0")
            {
                timer.text = "Go";
                done = !done;
            }
        }
        phase = 2;
        runningPhase = false;
        yield return null;
    }

    public void RoundWinner(int winner)
    {
        timer.color = Color.green;
        if (winner == 1)
        {
            p1Score++;
            timer.text = "Player 1: Correct";
        }
        else if(winner == 2)
        {
            p2Score++;
            timer.text = "Player 2: Correct";
        }
        SetScore();
        leaderboard.GetComponents<AudioSource>()[0].Play();
    }
    public void Wrong(int looser)
    {
        timer.color = Color.red;
        if (looser == 1)
        {
            p1Score--;
            timer.text = "Player 1: Wrong";
        }
        else if (looser == 2)
        {
            p2Score--;
            timer.text = "Player 2: Wrong";
        }
        SetScore();
        leaderboard.GetComponents<AudioSource>()[1].Play();
    }
    void SetScore()
    {
        p1.text = p1Score.ToString();
        p2.text = p2Score.ToString();
    }

}
