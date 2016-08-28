﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Class for managing token and door operation
// One object is created and attached to the token canvas
public class TokenBoard : MonoBehaviour {

    public List<TokenControl> tc;
	// Use this for initialization
	void Awake () {
        tc = new List<TokenControl>();
	}

    // Add a door
    public void add(QuestData.Door d)
    {
        tc.Add(new TokenControl(d));
    }

    // Add a token
    public void add(QuestData.Token t)
    {
        tc.Add(new TokenControl(t));
    }

    // Class for tokens and doors that will get the onClick event
    public class TokenControl
    {
        QuestData.Event e;

        // Initialise from a door
        public TokenControl(QuestData.Door d)
        {
            UnityEngine.UI.Button button = d.gameObject.AddComponent<UnityEngine.UI.Button>();
            button.interactable = true;
            button.onClick.AddListener(delegate { startEvent(); });
            e = d;
        }

        // Initialise from a token
        public TokenControl(QuestData.Token t)
        {
            UnityEngine.UI.Button button = t.gameObject.AddComponent<UnityEngine.UI.Button>();
            button.interactable = true;
            button.onClick.AddListener(delegate { startEvent(); });
            e = t;
        }

        // On click the tokens start an event
        public void startEvent()
        {
            // If we aren't visible ignore the click
            if (!e.GetVisible())
                return;
            // If a dialog is open ignore
            if (GameObject.FindGameObjectWithTag("dialog") != null)
                return;
            // Spawn a window with the door/token info
            new DialogWindow(e);
        }

    }
     
    public void AddMonster(QuestData.Monster m)
    {
        Sprite tileSprite;
        Texture2D newTex = Resources.Load("sprites/tokens/villager-token-man") as Texture2D;
        // Check load worked
        if (newTex == null)
        {
            Debug.Log("Error: Cannot load monster image");
            Application.Quit();
        }

        // Create object
        GameObject gameObject = new GameObject("MonsterSpawn");
        gameObject.tag = "dialog";

        Game game = Game.Get();
        gameObject.transform.parent = game.tokenCanvas.transform;

        // Create the image
        UnityEngine.UI.Image image = gameObject.AddComponent<UnityEngine.UI.Image>();
        tileSprite = Sprite.Create(newTex, new Rect(0, 0, newTex.width, newTex.height), Vector2.zero, 1);
        // Set door colour
        image.color = Color.red;
        image.sprite = tileSprite;
        image.rectTransform.sizeDelta = new Vector2((int)((float)newTex.width * (float)0.8), (int)((float)newTex.height * (float)0.8));
        // Move to square (105 units per square)
        gameObject.transform.Translate(new Vector3(m.location.x, m.location.y, 0) * 105, Space.World);
    }

    public void AddHighlight(QuestData.Event e)
    {
        Sprite tileSprite;
        Texture2D newTex = Resources.Load("sprites/tokens/search-token-special") as Texture2D;
        // Check load worked
        if (newTex == null)
        {
            Debug.Log("Error: Cannot load monster image");
            Application.Quit();
        }

        // Create object
        GameObject gameObject = new GameObject("MonsterSpawn");
        gameObject.tag = "dialog";

        // Find the token canvas to add to
        Canvas[] canvii = GameObject.FindObjectsOfType<Canvas>();
        Canvas board = canvii[0];
        foreach (Canvas c in canvii)
        {
            if (c.name.Equals("TokenCanvas"))
            {
                board = c;
            }
        }
        Game game = Game.Get();
        gameObject.transform.parent = game.tokenCanvas.transform;

        // Create the image
        UnityEngine.UI.Image image = gameObject.AddComponent<UnityEngine.UI.Image>();
        tileSprite = Sprite.Create(newTex, new Rect(0, 0, newTex.width, newTex.height), Vector2.zero, 1);
        // Set door colour
        image.color = Color.cyan;
        image.sprite = tileSprite;
        image.rectTransform.sizeDelta = new Vector2((int)((float)newTex.width * (float)0.8), (int)((float)newTex.height * (float)0.8));
        // Move to square (105 units per square)
        gameObject.transform.Translate(new Vector3(e.location.x, e.location.y, 0) * 105, Space.World);
    }
}