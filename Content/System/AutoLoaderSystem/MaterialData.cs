namespace PotionCraft.Content.System.AutoLoaderSystem;
public struct MaterialData
{
    public string Name;
    public string Tooltips;
    public Base BaseType;
    public string NameColor;
    public float ScaleSize = 1f;
    public int UseStyle = 5;
    public PotionUseSound PotionUseSound = PotionUseSound.Item2;
    

    public MaterialData(string name, string tooltips, Base baseType)
    {
        Name = name;
        Tooltips = tooltips;
        BaseType = baseType;
    }

    public MaterialData(string name, string tooltips, Base baseType, float scaleSize) : this(name, tooltips, baseType)
    {
        ScaleSize = scaleSize;
    }
    
    public MaterialData(string name, string tooltips, Base baseType, string nameColor,float scalesize)
    {
        Name = name;
        Tooltips = tooltips;
        BaseType = baseType;
        NameColor = nameColor;
        ScaleSize = scalesize;
    }
}