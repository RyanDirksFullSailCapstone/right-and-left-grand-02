using System;
using Doozy.Engine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesList : MonoBehaviour
{
    public List<string> MyMovePartsQueue = new List<string>();
    private Floor thisFloor;
    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        //Start listening for game events
        Message.AddListener<GameEventMessage>(OnMessage);
        thisFloor = GameObject.Find("floor").GetComponent<Floor>();
    }

    private void OnDisable()
    {
        //Stop listening for game events
        Message.RemoveListener<GameEventMessage>(OnMessage);
    }
    private void OnMessage(GameEventMessage message)
    {
        if (message == null) return;
        if (message.EventName.Contains("Swiped - "))
        {
            AddSquareDanceMove(message.EventName.Replace("Swiped - ", ""));
        }
    }
    private void AddSquareDanceMove(string swipeDirection)
    {
        string theCall = "";
        switch (swipeDirection)
        {
            case "UP": 
                theCall = "MOVE UP";
                break;
            case "UP LEFT":
                theCall = "ALLEMANDE LEFT";
                break;
            case "UP RIGHT":
                theCall = "UP TO THE MIDDLE AND BACK";
                break;
            case "DOWN":
                theCall = "MOVE BACK";
                break;
            case "DOWN LEFT":
                theCall = "U TURN BACK";
                break;
            case "DOWN RIGHT":
                thisFloor.ResetSquare();
                break;
            case "LEFT":
                theCall = "FACE LEFT";
                break;
            case "RIGHT":
                theCall = "FACE RIGHT";
                break;
        }
        Debug.Log($"called [{theCall}]");
        MyMovePartsQueue.Add(theCall);
    }
}
