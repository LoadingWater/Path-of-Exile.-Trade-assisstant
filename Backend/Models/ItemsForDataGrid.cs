using System.Collections.Generic;


namespace Backend.Models
{
    public class ItemsForDataGrid
    {
        public ItemsForDataGrid()
        {
            items = new List<Backend.Models.TrimmedItemModel>();
        }
        public List<TrimmedItemModel> items;
    }
}