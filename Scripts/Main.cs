using Godot;
using System;
using System.Collections.Generic;

public class Main : Node2D
{
    //The level that the player is on
    int level = 1;
    //The start position for the ball when instantiated
    Vector2 startPosition;
    //The list of levels "Pre loaded" for use as the game preogresses
    List<PackedScene> levels = new List<PackedScene>();
    //The ball scene that will be instanciated at the start of the level
    PackedScene ballScene;
    //The node for keeping track of the ball and level scene after they have been instanciated
    Node currentLevel, ball;
    //A reference to the hud scene
    CanvasLayer hud;
    //The script reference attached to the hud scene, assinged to a reference as its used many times and I do not want to call getnode every time
    HUD hudScript;
    //A reference to the timer that starts when the level is started
    Timer lapTimer;
    //Stores the minutes for the lap time
    int lapTimeMinutes;
    //Stores the secons for the lap time
    int lapTimeSeconds;
    //The timer used to count down time befoer the level starts
    Timer levelStartTimer;
    //The amount for the count down timer
    int levelStartTime = 3;
    //Ball velocity when pausing the game
    Vector2 ballVelocity;
    //Stores the balls angular velocity when puasung the game
    float ballAngelVelocity;
    // Called when the node enters the scene tree for the first time.
    Timer levelResetTimer;
    public override void _Ready()
    {
        lapTimer = GetNode<Timer>("LapTimer");
        levelStartTimer = GetNode<Timer>("CountDownTimer");
        //Get the HUD node to enable the minipulation of its child nodes
        hud = GetNode<CanvasLayer>("HUD");
        //Get the HUD script
        hudScript = GetNode<HUD>("HUD");
        //Load the levels in the list of levels
        levels.Add(ResourceLoader.Load("res://Nodes/Level1.tscn") as PackedScene);
        levels.Add(ResourceLoader.Load("res://Nodes/Level2.tscn") as PackedScene);
        levels.Add(ResourceLoader.Load("res://Nodes/Level3.tscn") as PackedScene);

        //Grabs the ball scene and loads it in a container for quick reference
        ballScene = (PackedScene)ResourceLoader.Load("res://Nodes/Ball.tscn");
        //Connect to the HUDs signals for he show menu button functionality
        hud.Connect("ShowMenu", this, nameof(ShowMenu));
        //Connect to the HUDs signals for he show menu button functionality
        hud.Connect("ShowHUD", this, nameof(StartLevel));

        levelResetTimer = GetNode<Timer>("LevelResetTimer");
    }
    public void StartLevel()
    {
        if (level > levels.Count)
        {
            //Throws error if the amount of levels exceed the list size and returns out of thte method
            GD.PrintErr("Level exceeds the max amount of layers: level = " + level + " Levels.Count = " + levels.Count);
            return;
        }
        else
        {
            //Instanciate the level that level is equal to
            currentLevel = levels[(level - 1)].Instance();
            currentLevel.Name = "currentLevel";
        }
        //Instantiate the firts level
        AddChild(currentLevel);
        ball = ballScene.Instance();
        ball.Name = "ball";
        //Inctanciatte the ball
        AddChild(ball);
        //Get the balls start position from the levels SpawnPoint Node2D
        startPosition = GetNode<Node2D>("currentLevel/SpawnPoint").Position;
        //Set hte balls position to the tart points position in the level
        GetNode<RigidBody2D>("ball").Position = startPosition;
        //Removes the balls gravity
        GetNode<RigidBody2D>("ball").GravityScale = 0;
        //Connect to the levels levelcomplete node to connet to the level complete signal
        GetNode<Node2D>("currentLevel/GoalArea").Connect("GoalReached", this, nameof(LapDone));
        //Starts the laps count down timer
        levelStartTimer.Start();
    }
    public void ShowMenu()
    {
        /*
        //Removes the balls gravity
        GetNode<RigidBody2D>("ball").GravityScale = 0;
        //Set the velocities of the ball to zero after storing them so tat when the menu is called the ball does not keep moving
        ballVelocity = GetNode<RigidBody2D>("ball").LinearVelocity;
        GetNode<RigidBody2D>("ball").LinearVelocity = Vector2.Zero;
        ballAngelVelocity = GetNode<RigidBody2D>("ball").AngularVelocity;
        //GetNode<RigidBody2D>("ball").AngularVelocity = 0f;
        */
        //Stop the timer when the game is paused
        lapTimer.Stop();
    }

    private void LapDone()
    {
        SaveScore(level, (lapTimeSeconds + lapTimeMinutes * 60));
        lapTimer.Stop(); 
         hudScript.ShowMessage(("Lap time for level " + level + "\n" + lapTimeMinutes + ":" + lapTimeSeconds));

ResetLevel();
 hudScript.UpdateLapTime("");
hudScript.LoadMenu();

        if (level < levels.Count)
        {
            GD.Print("Going to new level");
            level++;
            lapTimeMinutes = 0;
            lapTimeSeconds = 0;
            levelResetTimer.Start();
            levelWait();     
        }
        else
        {
            int level1time = GetNode<SaveSystem>("/root/SaveSystem").Load("Level Times", "1");
            int level2time = GetNode<SaveSystem>("/root/SaveSystem").Load("Level Times", "2");
            int level3time = GetNode<SaveSystem>("/root/SaveSystem").Load("Level Times", "3");

            hudScript.ShowMessage("All levels are completed! \n" + "Level 1 time = " + level1time / 60 + ":" + level1time % 60 + "\n"+ "Level 2 time = " + level2time / 60 + ":" + level2time % 60 + "\n"+ "Level 3 time = " + level3time / 60 + ":" + level3time % 60 + "\n");
        }

    }

    private async void levelWait()
    {
        GD.Print("Waiting");
        await ToSignal(levelResetTimer, "timeout");  
         GD.Print("Finnished waiting");
        StartLevel();
        hudScript.hudParticles.Hide();
     
    }



    public void SaveScore(int level, int sec)
    {
        GetNode<SaveSystem>("/root/SaveSystem").Save("Level Times", level.ToString(), sec);
    }

    public void LoadScore()
    {
        GetNode<SaveSystem>("/root/SaveSystem").Load("Level Times", level.ToString());
    }
    private void LapStartTick()
    {
        hudScript.ShowMessage(levelStartTime.ToString());
        levelStartTime--;

        if (levelStartTime < 0)
        {
            levelStartTime = 3;
            lapTimer.Start();
            levelStartTimer.Stop();
            hudScript.HideMessage();
            GetNode<RigidBody2D>("ball").GravityScale = 5;
        }
    }
    private void LapTimerTick()
    {
        lapTimeSeconds++;
        if (lapTimeSeconds % 60 == 0)
        {
            lapTimeMinutes++;
            lapTimeSeconds = 0;
        }

        hudScript.UpdateLapTime(lapTimeMinutes.ToString() + ":" + lapTimeSeconds.ToString());
    }
    private void ResetLevel()
    {
        GetTree().QueueDelete(currentLevel);
        GetTree().QueueDelete(ball);
    }
}
