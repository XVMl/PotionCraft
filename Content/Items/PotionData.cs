
namespace PotionCraft.Content.Items;

public class PotionData(int buffId, int itemId, int counts, int buffTime)
{
    public int BuffId = buffId;
    public int ItemId = itemId;
    public int Counts = counts;
    public int BuffTime = buffTime;
}