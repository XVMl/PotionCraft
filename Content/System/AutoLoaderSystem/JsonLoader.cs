using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MonoMod.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PotionCraft.Content.Items;
using PotionCraft.Content.System.ColorfulText;
using ReLogic.OS;
using Stubble.Core.Contexts;
using Terraria;
using Terraria.ModLoader;
using static PotionCraft.Content.System.ColorfulText.PotionColorText;
namespace PotionCraft.Content.System.AutoLoaderSystem;

public class JsonLoader:ModSystem
{
    public static Dictionary<string,Dictionary<string,string>> ColorfulTexts = new();
    
    public static List<string> Materials = new ();

    private string FileStreamText(string filename)
    {
        using var reader = new StreamReader(Mod.GetFileStream(filename));
        return reader.ReadToEnd();
    }

    private void LoaderItems(Dictionary<string, object> jsonObject)
    {
        var array = jsonObject["Materials"] as JArray;
        foreach (var item in array)
        {
            var data = JsonConvert.DeserializeObject<MaterialData>(item.ToString());
            Materials.Add(data.Name);
            Mod.AddContent(new BaseCustomMaterials(data));
        }
    }
    
    private void JsonToDictionary(string filetext,ref Dictionary<string, Dictionary<string, string>> dict)
    {
        try
        {
            var js = FileStreamText(filetext);
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(js);
            if (!jsonObject.TryGetValue("Type", out object value)) return;
            string[] operations = ["And", "MashUp", "Purify"];
            switch (value)
            {
                case "Operation":
                    foreach (var operation in operations)
                        dict.TryAdd(operation,Activator.CreateInstance(typeof(Dictionary<string,string>),
                            ArrayDictionary(jsonObject, operation) ) as Dictionary<string, string>);
                    break;
                case "BuffName":
                    dict.TryAdd("BuffName", jsonObject.ToDictionary(k => k.Key, v => v.Value.ToString()));
                    break;
                case "Materials":
                    LoaderItems(jsonObject);
                    break;
                default:
                    return;
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void LoadCustumSetting()
    {
        try
        {
            var path = Main.SavePath;

        }
        catch (Exception e)
        {
            throw;
        }
    }
    private static Dictionary<string, string> ArrayDictionary(Dictionary<string, object> jsonObject,string key)
    {
        if (!jsonObject.TryGetValue(key, out _)) return null;
        var aArray = jsonObject[key] as JArray;
        var aDictionary = new Dictionary<string, string>();
        foreach (var item in aArray)
        {
            var itemDict = item as JObject;
            foreach (var property in itemDict.Properties())
            {
                aDictionary[property.Name] = property.Value.ToString();
            }
        }
        return aDictionary;
    }

    public override void Load()
    {
        var files = Mod.GetFileNames()
            .Where( f=> f.StartsWith("Assets")&& f.EndsWith(".json"));
        foreach (var file in files)
        {
            JsonToDictionary(file,ref ColorfulTexts);
        }
    }
    
}