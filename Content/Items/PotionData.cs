
namespace PotionCraft.Content.Items;

public class PotionData(string buffName, int itemId, int counts, int buffTime,int buffid)
{
    public string BuffName = buffName;
    public int ItemId = itemId;
    public int Counts = counts;
    public int BuffTime = buffTime;
    public int  BuffId = buffid;
}