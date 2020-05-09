using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Database
{
    public class DatabaseFunctions
    {
        public void UpdateDatabase(List<ItemModel.RootObject> stashTabs, DatabaseContext database)
        {
            for (int tabNumber = 0; tabNumber < stashTabs.Count; tabNumber++)
            {
                //update tabs
                var tab = stashTabs[tabNumber].tabs[tabNumber];
                if (database.Tabs.Find(tab.id) == null)
                {
                    database.Tabs.Add(new Tab() { TabId = tab.id, TabName = tab.n });
                }
                else
                {
                    database.Tabs.SqlQuery($"update Tabs set TabId = {tab.id}, TabName = {tab.n} where TabId == '{tab.id}'");
                }
                for (int itemNumber = 0; itemNumber < stashTabs[tabNumber].items.Count; itemNumber++)
                {
                    //updata items
                    var item = stashTabs[tabNumber].items[itemNumber];
                    var t = database.Items.Find(item.id);
                    if (database.Items.Find(item.id) == null)
                    {
                        database.Items.Add(new Item() { ItemFrameType = item.frameType, ItemIconAddress = item.icon, ItemId = item.id, ItemName = item.name, ItemNote = item.note, TabId = tab.id });
                    }
                    else
                    {
                        database.Items.SqlQuery($"update Items set ItemId = {item.id}, ItemName = {item.name}, ItemNote = {item.note}, ItemFrameType = {item.frameType}, ItemIconAddress = {item.icon}, TabId = {tab.id} where ItemId == '{item.id}");
                    }
                }
            }
            database.SaveChanges();
        }
    }
}
