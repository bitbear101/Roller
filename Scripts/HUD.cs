using Godot;
using System;

public class HUD : CanvasLayer
{
    [Signal]
    public delegate void ShowMenu();
    [Signal]
    public delegate void ShowHUD();
    
    Label messageLabel;
    Label lapTime;
    Button startBtn;
    Button exitBtn;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Grab a reffrence to all the nodes in the HUD scene 
        messageLabel = GetNode<Label>("MessageLabel");
        lapTime = GetNode<Label>("LapTime");
        startBtn = GetNode<Button>("Start");
        exitBtn = GetNode<Button>("Exit");
        //Hide all the HUD controls for the first start up of the game
        HideUI();
        //Show the UI nodes needed for the start menu
        LoadStartMenu();
    }
    //Show the exit button for the main n=menu when it shows
    public void LoadMenu()
    {
        HideUI();
        EmitSignal(nameof(ShowMenu));
        exitBtn.Show();
    }
    private void LoadHUD()
    {
        HideUI();
        EmitSignal(nameof(ShowHUD));
        lapTime.Show();
        exitBtn.Show();
    }
    public void LoadStartMenu()
    {
        LoadMenu();
        startBtn.Show();
    }
    public void ShowMessage(string text)
    {
        messageLabel.Text = text;
        messageLabel.Show();
    }

    public void UpdateLapTime(String text)
    {
        lapTime.Text = text;
    }
    public void HideMessage()
    {
        messageLabel.Text = "";
        messageLabel.Hide();
    }
    //Hides all the UI nodes, use to quickly clear out hte screen of UI elements
    public void HideUI()
    {
        messageLabel.Hide();
        lapTime.Hide();
        startBtn.Hide();
        exitBtn.Hide();
    }
    //This function is called when the user clicks the start buttob in the menu
    private void StartGamePressed()
    {
        LoadHUD();
    }
    private void QuitGame()
    {
        GetTree().Quit();
    }

}
