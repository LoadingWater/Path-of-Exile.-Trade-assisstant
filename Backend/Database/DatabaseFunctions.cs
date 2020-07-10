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

            //Need to create index and increase it when we add a new tab. "For loop index" didn't work
            int tabIndex = 0;
            //Selected league. Iterate until get a league from an item
            string league = "No info";
            for (int i = 0; i < stashTabs.Count; i++)
            {
                if (stashTabs[i].items.Count != 0)
                {
                    league = stashTabs[i].items[0].league;
                    break;
                }
            }
            //Iterate through stash tabs
            for (int tabNumber = 0; tabNumber < stashTabs.Count; tabNumber++)
            {
                var tab = stashTabs[tabNumber].tabs[tabNumber];

                //If any items in a tab
                if (stashTabs[tabNumber].items.Count != 0)
                {
                    UpdateTabs(database, tab, league, tabIndex);
                    ++tabIndex;
                }

                //Iterate through tab items
                for (int itemNumber = 0; itemNumber < stashTabs[tabNumber].items.Count; itemNumber++)
                {
                    UpdateItems(database, stashTabs[tabNumber].items[itemNumber], tab);
                    idsOfItemsFromRequest.Add(stashTabs[tabNumber].items[itemNumber].id);
                }
            }

            RemoveMismatchedItemsFromDB(idsOfItemsFromRequest, database, league);
            database.SaveChanges();
        }

        private void UpdateTabs(DatabaseContext database, ItemModel.Tab tabFromRequest, string league, int tabIndex)
        {
            var tabFromDb = database.Tabs.Find(tabFromRequest.id);
            if (tabFromDb == null)
            {
                //Add new if not found
                database.Tabs.Add(new Tab() 
                { 
                    TabName = tabFromRequest.n, 
                    TabIndex = tabIndex, 
                    TabId = tabFromRequest.id, 
                    TabColourBlue = tabFromRequest.colour.b, 
                    TabColourGreen = tabFromRequest.colour.g, 
                    TabColourRed = tabFromRequest.colour.r,
                    TabLeague = league
                });
            }
            else
            {
                //Update if found
                tabFromDb.TabId = tabFromRequest.id;
                tabFromDb.TabIndex = tabIndex;
                tabFromDb.TabName  = tabFromRequest.n;
                tabFromDb.TabColourRed  = tabFromRequest.colour.r;
                tabFromDb.TabColourGreen  = tabFromRequest.colour.g;
                tabFromDb.TabColourBlue  = tabFromRequest.colour.b;
                tabFromDb.TabLeague = league;
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
                        ItemLeague = itemFromRequest.league,
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
                    itemFromDb.ItemFrameType = itemFromRequest.frameType;
                    itemFromDb.ItemIconAddress = itemFromRequest.icon;
                    itemFromDb.ItemId = itemFromRequest.id;
                    itemFromDb.ItemName = RetriveItemName(itemFromRequest);
                    itemFromDb.ItemNote = itemFromRequest.note;
                    itemFromDb.TabId = tabFromRequest.id;
                    itemFromDb.ItemAffixes = RetriveItemAffixes(itemFromRequest);
                    itemFromDb.ItemLeague = itemFromRequest.league;
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
        private void RemoveMismatchedItemsFromDB(List<string> itemsIdsFromRequest, DatabaseContext database, string league)
        {
            //Get number of item within a given league
            int dbItemsWithinLeagueCount = database.Items.Where((x) => x.ItemLeague == league).Count();
            //If there are different amount of items in DB and Request under the same league
            if (itemsIdsFromRequest.Count != dbItemsWithinLeagueCount)
            {
                //Get items within a league
                var dbItems = database.Items.Where((x) => x.ItemLeague != league).ToList();
                Item itemToDelete = new Item();

                //Compare every Item ID in DB to every Item ID from a request
                for (int firstIndex = 0; firstIndex < dbItemsWithinLeagueCount; firstIndex++)
                {
                    for (int secondIndex = 0; secondIndex < itemsIdsFromRequest.Count; secondIndex++)
                    {
                        //If Item was found by ID
                        if (dbItems[firstIndex].ItemId == itemsIdsFromRequest[secondIndex])
                        {
                            //Reset an itemToDelete if we found a match after a missmatch. Missmatch -> Match -> Missmatch -> Item got deleted
                            itemToDelete = null;
                            //Break if item was found
                            break;
                        }
                        else
                        {
                            itemToDelete = dbItems[firstIndex];
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