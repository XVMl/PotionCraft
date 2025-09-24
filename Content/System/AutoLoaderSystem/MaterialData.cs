namespace PotionCraft.Content.System.AutoLoaderSystem;
public struct MaterialData
{
    public string Name;
    public string Tooltips;
    public Base BaseType;
    public string NameColor;

    public MaterialData(string name, string tooltips, Base baseType)
    {
        Name = name;
        Tooltips = tooltips;
        BaseType = baseType;
    }

    public MaterialData(string name, string tooltips, Base baseType, string nameColor)
    {
        Name = name;
        Tooltips = tooltips;
        BaseType = baseType;
        NameColor = nameColor;
    }
}