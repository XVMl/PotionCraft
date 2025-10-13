using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonoMod.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PotionCraft.Content.System.ColorfulText;
using Stubble.Core.Contexts;
using Terraria.ModLoader;
using static PotionCraft.Content.System.ColorfulText.PotionColorText;
using static PotionCraft.Content.System.AutoLoaderSystem.LoaderItemsSystem;
namespace PotionCraft.Content.System.AutoLoaderSystem;

public class JsonLoader:ModSystem
{
    public static Dictionary<string,Dictionary<string,string>> ColorfulTexts = new();
    
    private string FileStreamText(string filename)
    {
        using var reader = new StreamReader(Mod.GetFileStream(filename));
        return reader.ReadToEnd();
    }
    
    private void JsonToDictionary(string filetext,ref Dictionary<string, Dictionary<string, string>> dict)
    {
        try
        {
            using var js = new StreamReader(FileStreamText(filetext));
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(js.ReadToEnd());
            if (!jsonObject.TryGetValue("Type", out object value)) return;
            string[] operations = ["And", "MashUp", "Purified"];
            switch (value)
            {
                case "Operator":
                    foreach (var operation in operations)
                        dict.TryAdd(operation,Activator.CreateInstance(typeof(Dictionary<string,string>),
                            ArrayDictionary(jsonObject, operation) ) as Dictionary<string, string>);
                    break;
                case "ColorfulText":
                    dict.TryAdd("BuffName", jsonObject.ToDictionary(k => k.Key, v => v.Value.ToString()));
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
        var files =Mod.GetFileNames()
            .Where( f=> f.StartsWith("Assets/ColorfulText")&& f.EndsWith(".json"));
        foreach (var file in files)
        {
            JsonToDictionary(file,ref ColorfulTexts);
        }
    }
    
}