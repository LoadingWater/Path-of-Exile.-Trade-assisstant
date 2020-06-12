using System;
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

            //Iterate through stash tabs
            for (int tabNumber = 0; tabNumber < stashTabs.Count; tabNumber++)
            {
                var tab = stashTabs[tabNumber].tabs[tabNumber];
                UpdateTabs(database, tab);

                //Iterate through tab items
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
                //Add new if not found
                database.Tabs.Add(new Tab() 
                { 
                    TabName = tabFromRequest.n, 
                    TabIndex = tabFromRequest.i, 
                    TabId = tabFromRequest.id, 
                    TabColourBlue = tabFromRequest.colour.b, 
                    TabColourGreen = tabFromRequest.colour.g, 
                    TabColourRed = tabFromRequest.colour.r 
                });
            }
            else
            {
                //Update if found
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
            try
            {
                var itemFromDb = database.Items.Find(itemFromRequest.id);
                if (itemFromDb == null)
                {
                    //Add new if not found
                    database.Items.Add(new Item()
                    {
                        ItemFrameType = itemFromRequest.frameType,
                        ItemIconAddress = itemFromRequest.icon,
                        ItemId = itemFromRequest.id,
                        ItemName = RetriveItemName(itemFromRequest),
                        ItemNote = itemFromRequest.note,
                        TabId = tabFromRequest.id,
                        ItemAffixes = RetriveItemAffixes(itemFromRequest),
                        CreationTime = DateTime.Now.ToString(),
                        PriceChangedTime = DateTime.Now.ToString()
                    });
                }
                else
                {
                    //Update if found
                    if (itemFromDb.ItemNote != itemFromRequest.note)
                    {
                        itemFromDb.PriceChangedTime = DateTime.Now.ToString();
                    }
                    itemFromDb.ItemId = itemFromRequest.id;
                    itemFromDb.ItemName = RetriveItemName(itemFromRequest);
                    itemFromDb.ItemNote = itemFromRequest.note;
                    itemFromDb.ItemFrameType = itemFromRequest.frameType;
                    itemFromDb.ItemIconAddress = itemFromRequest.icon;
                    itemFromDb.TabId = tabFromRequest.id;
                    itemFromDb.ItemAffixes = RetriveItemAffixes(itemFromRequest);
                }
            }
            catch (System.Exception w)
            {
                MessageBox.Show(w.Message);
                throw;
            }
        }
        private string RetriveItemName(ItemModel.Item itemFromRequest)
        {
            if (itemFromRequest.name != "")
            {
                return itemFromRequest.name;
            }
            else
            {
                return itemFromRequest.typeLine;
            }
        }
        //TODO: Rewrite
        private string RetriveItemAffixes(ItemModel.Item itemFromRequest)
        {
            string itemAffixes = "";
            //if its a prophecy. no mods. unidentified
            if (itemFromRequest.explicitMods == null && itemFromRequest.properties == null)
            {
                //the void has no explicit mods and properties
                if (itemFromRequest.typeLine == "The Void")
                {
                    itemAffixes = itemFromRequest.flavourText[0]; 
                }
                else if (itemFromRequest.identified == true)
                {
                    if (itemFromRequest.flavourText != null)
                    {
                        itemAffixes = itemFromRequest.flavourText[0];
                    }
                    else
                    {
                        itemAffixes = "No mods";
                    }
                }
                else
                { 
                    itemAffixes = "Unidentified";
                }
            }
            else if (itemFromRequest.explicitMods != null)
            {
                //if divinition card
                
                if (itemFromRequest.frameType == 6)
                {
                    if (itemFromRequest.flavourText != null)
                    {
                        foreach (var line in itemFromRequest.flavourText)
                        {
                            itemAffixes += line;
                        }
                        itemAffixes = ExtractDivinitionCardDescription(itemAffixes);
                    }
                    else
                    {
                        Debug.WriteLine(itemFromRequest.id);
                    }
                }
                else
                {
                    //card can be without a description
                    if (itemFromRequest.explicitMods.Count > 0)
                    {
                        for (int i = 0; i < itemFromRequest.explicitMods.Count; i++)
                        {
                            //create one string from mods
                            itemAffixes += $"/{itemFromRequest.explicitMods[i]}/\n";
                        }
                    }
                }
            }
            else if (itemFromRequest.properties != null)
            {
                foreach (var property in itemFromRequest.properties)
                {
                    if (property.values.Count == 1)
                    {
                        if (property.name.Contains("%0"))
                        {
                            property.name = property.name.Replace("%0", property.values[0][0].ToString());
                        }
                        else
                        {
                            property.name = $"{property.name} {property.values[0][0]}";
                        }
                    }
                    else if (property.values.Count == 2)
                    {
                        property.name = property.name.Replace("%0", property.values[0][0].ToString());
                        property.name = property.name.Replace("%1", property.values[1][0].ToString());
                    }
                    //create string of mods
                    itemAffixes += $"|{property.name}|\n";
                }
            }
            return itemAffixes;
        }
        private string ExtractDivinitionCardDescription(string cardDescription)
        {
            string cleanDescription = "";
            for (int i1 = 0; i1 < cardDescription.Length; i1++)
            {
                if (cardDescription[i1] == '{')
                {
                    for (int i2 = i1 + 1; i2 < cardDescription.Length; i2++)
                    {
                        if (cardDescription[i2] != '}')
                        {
                            cleanDescription += cardDescription[i2];
                        }
                        else
                        {
                            return cleanDescription;
                        }
                    }
                }
            }
            return cardDescription;
        }
        private void RemoveMismatchedItemsFromDB(List<string> idsOfItemsFromRequest, DatabaseContext database)
        {
            //NOTE: do this with sql?
            //Get items count
            int currentNumberOfItemsInDb = database.Items.Where((x) => x.ItemId != "").Count();
            //If db items count != items count from request
            if (idsOfItemsFromRequest.Count != currentNumberOfItemsInDb)
            {
                //Get all items in db
                var currentItemsInDb = database.Items.Where((x) => x.ItemId != "").ToList();
                Item itemToDelete = new Item();

                //Compare every item id in db to every item id from request
                for (int i = 0; i < currentNumberOfItemsInDb; i++)
                {
                    for (int c = 0; c < idsOfItemsFromRequest.Count; c++)
                    {
                        //If there is an item in db with ID from request
                        if (currentItemsInDb[i].ItemId == idsOfItemsFromRequest[c])
                        {
                            //Reset an itemToDelete if we found a match after a missmatch. Missmatch -> Match -> Missmatch -> Item got deleted
                            itemToDelete = null;
                            //Break if item was found
                            break;
                        }
                        else
                        {
                            itemToDelete = currentItemsInDb[i];
                        }
                    }
                    //Delete item from db
                    if (itemToDelete != null)
                    {
                        database.Items.Remove(itemToDelete);
                    }
                }
            }
        }
    }
}