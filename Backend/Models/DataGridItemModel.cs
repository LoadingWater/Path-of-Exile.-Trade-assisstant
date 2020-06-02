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
        public override string ToString()
        {
            return $"ItemId: {ItemId}\nItemName: {ItemName}\nItemNote: {ItemNote}\nItemFrameType: {ItemFrameType}\nItemIconAddress: {ItemIconAddress}\nTabId: {TabId}\n";
        }
    }
}