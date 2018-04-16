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
    void Start () {
        curQuestion = 0;
        currentButton = -1;
        timer.text = "3";
        p1Score = 0;
        p2Score = 0;
        waitingInput = false;
        //RoundWinner(2);
        phase = 0;
        runningPhase = false;
        questions.Add(new Question("Fill in the blank lol?", new List<string> { "sup" , "lol", "ok"}, 1) );
    }
	
	// Update is called once per frame
	void Update () {
        if (!runningPhase)
        {
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
            runningPhase = true;
        }
    }
    void loadQuestion()
    {
        instructor.GetComponentInChildren<Text>().text = questions[curQuestion].question;

        bh.p1B1.gameObject.GetComponentInChildren<Text>().text = questions[curQuestion].answers[0];
        bh.p1B2.gameObject.GetComponentInChildren<Text>().text = questions[curQuestion].answers[1];
        bh.p1B3.gameObject.GetComponentInChildren<Text>().text = questions[curQuestion].answers[2];

        bh.p2B1.gameObject.GetComponentInChildren<Text>().text = questions[curQuestion].answers[0];
        bh.p2B2.gameObject.GetComponentInChildren<Text>().text = questions[curQuestion].answers[1];
        bh.p2B3.gameObject.GetComponentInChildren<Text>().text = questions[curQuestion].answers[2];
    }
    IEnumerator Results()
    {
        //print("starting play");
        bool done = false;
        while (!done)
        {//do while timer is not 0 then keep 
            //* NEXT OBJECTIVE: CALCULATE WICH WAS THE RIGHT ANSWER
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
            
            done = !done;
            //yield return new WaitForSeconds(0.01f);
        }

        //phase = 3;
        
        //print("exiting play");
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
    }
    void SetScore()
    {
        p1.text = p1Score.ToString();
        p2.text = p2Score.ToString();
    }

}
