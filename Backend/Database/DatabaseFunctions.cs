using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Backend.Models;


namespace Backend.Database
{
    public class DatabaseFunctions
    {
        public void UpdateDatabase(List<ItemModel.RootObject> stashTabs, DatabaseContext database)
        {
            List<string> idsOfItemsFromRequest = new List<string>();

            for (int tabNumber = 0; tabNumber < stashTabs.Count; tabNumber++)
            {
                var tab = stashTabs[tabNumber].tabs[tabNumber];
                UpdateTabs(database, tab);

                for (int itemNumber = 0; itemNumber < stashTabs[tabNumber].items.Count; itemNumber++)
                {
                    UpdateItems(database, stashTabs[tabNumber].items[itemNumber], tab);
                    idsOfItemsFromRequest.Add(stashTabs[tabNumber].items[itemNumber].id);
                }
            }
            RemoveMismatchedItemsFromDB(idsOfItemsFromRequest, database);
            database.SaveChanges();
        }

        private void UpdateTabs(DatabaseContext database, ItemModel.Tab tabFromRequest)
        {
            var tabFromDb = database.Tabs.Find(tabFromRequest.id);
            if (tabFromDb == null)
            {
                database.Tabs.Add(new Tab() { TabName = tabFromRequest.n, TabIndex = tabFromRequest.i, TabId = tabFromRequest.id, TabColourBlue = tabFromRequest.colour.b, TabColourGreen = tabFromRequest.colour.g, TabColourRed = tabFromRequest.colour.r });
            }
            else
            {
                tabFromDb.TabId = tabFromRequest.id;
                tabFromDb.TabIndex = tabFromRequest.i;
                tabFromDb.TabName  = tabFromRequest.n;
                tabFromDb.TabColourRed  = tabFromRequest.colour.r;
                tabFromDb.TabColourGreen  = tabFromRequest.colour.g;
                tabFromDb.TabColourBlue  = tabFromRequest.colour.b;
            }
        }
        private void UpdateItems(DatabaseContext database, ItemModel.Item itemFromRequest, ItemModel.Tab tabFromRequest)
        {
            var itemFromDb = database.Items.Find(itemFromRequest.id);
            if (itemFromDb == null)
            {
                database.Items.Add(new Item() { ItemFrameType = itemFromRequest.frameType, ItemIconAddress = itemFromRequest.icon, ItemId = itemFromRequest.id, ItemName = RetriveItemName(itemFromRequest), ItemNote = itemFromRequest.note, TabId = tabFromRequest.id });
            }
            else
            {
                itemFromDb.ItemId = itemFromRequest.id;
                itemFromDb.ItemName = RetriveItemName(itemFromRequest);
                itemFromDb.ItemNote = itemFromRequest.note;
                itemFromDb.ItemFrameType = itemFromRequest.frameType;
                itemFromDb.ItemIconAddress = itemFromRequest.icon;
                itemFromDb.TabId = tabFromRequest.id;
            }
        }
        private string RetriveItemName(ItemModel.Item item)
        {
            if (item.name != "")
            {
                return item.name;
            }
            else
            {
                return item.typeLine;
            }
        }
        private void RemoveMismatchedItemsFromDB(List<string> idsOfItemsFromRequest, DatabaseContext database)
        {
            //NOTE: do this with sql
            int currentNumberOfItemsInDb = database.Items.Where((x) => x.ItemId != "").Count();
            if (idsOfItemsFromRequest.Count != currentNumberOfItemsInDb)
            {
                var currentItemsInDb = database.Items.Where((x) => x.ItemId != "").ToList();
                Item itemToDelete = new Item();
                for (int i = 0; i < currentNumberOfItemsInDb; i++)
                {
                    for (int c = 0; c < idsOfItemsFromRequest.Count; c++)
                    {
                        if (currentItemsInDb[i].ItemId == idsOfItemsFromRequest[c])
                        {
                            //reset item if we find match after mismatch
                            itemToDelete = null;
                            break;
                        }
                        else
                        {
                            itemToDelete = currentItemsInDb[i];
                        }
                    }
                    if (itemToDelete != null)
                    {
                        database.Items.Remove(itemToDelete);
                    }
                }
            }
        }
    }
}