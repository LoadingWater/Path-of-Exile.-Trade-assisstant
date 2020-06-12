namespace Backend.Models
{
    public class DataGridItemModel
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemNote { get; set; }
        public int ItemFrameType { get; set; }
        public string ItemIconAddress { get; set; }
        public string TabId { get; set; }
        public string ItemAffixes { get; set; }
        public string ElapsedTime { get; set; }
        public string ElapsedTimeFromTheLastPriceChange { get; set; }
        public override string ToString()
        {
            return $"ItemId: {ItemId}\n" +
                $"ItemName: {ItemName}\n" +
                $"ItemNote: {ItemNote}\n" +
                $"ItemFrameType: {ItemFrameType}\n" +
                $"ItemIconAddress: {ItemIconAddress}\n" +
                $"TabId: {TabId}\n" +
                $"ElapsedTime: {ElapsedTime}\n" +
                $"ElapsedTimeFromTheLastPriceChange: {ElapsedTimeFromTheLastPriceChange}\n";
        }
    }
}