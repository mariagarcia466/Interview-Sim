using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class JankasaurusRex : MonoBehaviour
{
    // IT IS 4:30 AM I AM NO LONGER WRITING GOOD CODE
    // AWFUL CODE IS MY NEW BEST FRIEND

    public GameObject webSocketDriver;
    public GameObject textDisplay;
    public GameObject boxButton;
    public GameObject timerDisplay;
    public GameObject witHandler;

    private Connection socketConnection;

    private TextMeshProUGUI textMesh;
    private TextMeshProUGUI timerMesh;
    private WitHandler witHandlerScript;

    private BoxButtonScript boxButtonScript;

    private int gameState = 0;

    // Start is called before the first frame update
    void Start()
    {
        socketConnection = webSocketDriver.GetComponent<Connection>();
        textMesh = textDisplay.GetComponent<TextMeshProUGUI>();
        timerMesh = timerDisplay.GetComponent<TextMeshProUGUI>();
        boxButtonScript = boxButton.GetComponent<BoxButtonScript>();
        witHandlerScript = witHandler.GetComponent<WitHandler>();
    }

    // Update is called once per frame
    private float lastEvent = 0F;
    private float targetTimeDelta = 0F;

    private float questionStart = 0F;
    
    void Update()
    {
        float currentTime = Time.time;
        float timeDelta = currentTime - lastEvent;

        // This is how we handle sleep :sunglasses:
        if (timeDelta >= targetTimeDelta)
        {
            // THIS IS TERRIBLE CODE
            // DO NOT READ ANY FURTHER
            // THIS IS FOR YOUR OWN SANITY

            lastEvent = currentTime;
            targetTimeDelta = 0F;

            // Game start
            if (gameState == 0)
            {
                if (boxButtonScript.hit)
                {
                    gameState += 1;
                    boxButton.SetActive(false);
                    boxButtonScript.hit = false;
                }
            }
            // Handshake prompt
            else if (gameState == 1)
            {
                textMesh.SetText("Shake my hand!");
                targetTimeDelta = 5F;
                gameState += 1;
            }
            else if (gameState == 2)
            {
                String result = socketConnection.lastValue;
                if (result.Equals("0"))
                {
                    textMesh.SetText("Hmm... Not a very strong handshake. Not elegant.");
                }
                else if (result.Equals("1"))
                {
                    textMesh.SetText("Very nice handshake. Very elegant.");
                }
                else if (result.Equals("2"))
                {
                    textMesh.SetText("That's my hand, not a maraca!");
                }

                targetTimeDelta = 3F;
                gameState += 1;
            }
            else if (gameState == 3)
            {
                targetTimeDelta = 0.5F;
                textMesh.SetText("Now, tell me about yourself. You have 30 seconds.");
                questionStart = currentTime + 5;
                targetTimeDelta = 5F;
                gameState += 1;
            } else if (gameState == 4)
            {
                gameState += 1;
                witHandler.SetActive(true);
            }
            else if (gameState == 5)
            {
                if (currentTime - questionStart >= 30)
                {
                    gameState += 1;
                }
                else
                {
                    timerMesh.SetText((30 - (currentTime - questionStart)).ToString(CultureInfo.CurrentCulture));
                    textMesh.SetText(witHandlerScript.fullTranscript + " " + witHandlerScript.tempTranscript);
                    
                }
            } else if (gameState == 6)
            {
                targetTimeDelta = 0.5F;
                witHandler.SetActive(false);
                gameState += 1;
            } 
            else
            {
                textMesh.SetText("no game state :(\nWe haven't made this part yet...");
            }
        }
        else
        {
            timerMesh.SetText((targetTimeDelta - timeDelta).ToString(CultureInfo.CurrentCulture));
        }
    }
}