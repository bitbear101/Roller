using Godot;
using System;

public class SaveSystem : Node2D
{
    int value = 0;
    String savePath = "res://saveFile.cfg"; 
    ConfigFile config = new ConfigFile();
    Error LoadResponse;

    public override void _Ready()
    {
        LoadResponse = config.Load(savePath);
    }

   public void Save(String section, String key, int level, int val)
   {
       config.SetValue(section, key, val);
       config.Save(savePath);
   } 
   public int Load(string section, string key)
   {
       //Returns object type that has been cast to int
       return (int)config.GetValue(section, key, value);
   }
}
