using Backend.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Backend.APIFunctions;
using Backend.Classes;

namespace Backend.GUIFunctions
{
    using ItemsInOneTab = Task<List<TrimmedItemModel>>;
    using EveryTabWithItems = List<Task<List<TrimmedItemModel>>>;
    public class GUIFunctions
    {
        Stopwatch cooldown = new Stopwatch();
        bool firstTime = true;
        public async Task GoButtonFunction(CustomClient client, PlayerInfo playerInfo, TabControl tabControl, Style style)
        {
            try
            {
                //TODO function parametrs null arg check
                //TODO no cooldown if forbidden
                if (cooldown.ElapsedMilliseconds > 60000 || firstTime)
                {
                    firstTime = false;
                    cooldown.Restart();

                    EveryTabWithItems tabs = new EveryTabWithItems();
                    tabs = await PathOfExileApiFunctions.GetItemsInAllStashTabsAsync(client, playerInfo);
                    //create and fill structure with tab items

                    for (int stashNumber = 0; stashNumber < tabs.Count; stashNumber++)
                    {
                        ItemsForDataGrid itemsForDataGrid = new ItemsForDataGrid();
                        int numberOfItemsInATab = tabs[stashNumber].Result.Count;

                        for (int itemNumber = 0; itemNumber < numberOfItemsInATab; itemNumber++)
                        {
                            if (numberOfItemsInATab > 0)
                            {
                                itemsForDataGrid.items.Add(tabs[stashNumber].Result[itemNumber]);
                            }
                        }
                        //if any items in a tab/ if not dont create DataGrid
                        if (tabs[stashNumber].Result.Count > 0)
                        {
                            //TODO:create a control template to elluminate manual creation of the grid
                            DataGrid dataGrid = new DataGrid();
                            dataGrid = CreateDataGridColumns(itemsForDataGrid, style);
                            TabItem item = new TabItem() { Header = itemsForDataGrid.items[0].TabName, Content = dataGrid };
                            tabControl.Items.Add(item);
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Cooldown - {(60000 - cooldown.ElapsedMilliseconds) * 0.001}");
                }

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    MessageBox.Show("Wrong POESESSID");
                    //NOTE:Easy to trick
                    firstTime = true;
                }
                else
                {
                    MessageBox.Show($"{ex.Message}\n{ex.Source}");
                }

            }
        }

        #region Private functions
        private void AddItemsIfThereAreAny(EveryTabWithItems tabs, ItemsForDataGrid itemsForDataGrid, int stashNumber, int numberOfItemsInATab)
        {
            for (int itemNumber = 0; itemNumber < numberOfItemsInATab; itemNumber++)
            {
                if (numberOfItemsInATab > 0)
                {
                    itemsForDataGrid.items.Add(tabs[stashNumber].Result[itemNumber]);
                }
            }
        }
        private DataGrid CreateDataGridColumns(ItemsForDataGrid itemsForDataGrid, Style style)
        {
            DataGrid dataGrid = new DataGrid() { ItemsSource = itemsForDataGrid.items, Style = style };
            DataGridTextColumn nameColumn = new DataGridTextColumn() { Header = "Name", Binding = new Binding("Name") };
            dataGrid.Columns.Add(nameColumn);
            DataGridTextColumn priceColumn = new DataGridTextColumn() { Header = "Price", Binding = new Binding("Price") };
            dataGrid.Columns.Add(priceColumn);
            DataGridTextColumn itemIdColumn = new DataGridTextColumn() { Header = "Item Id", Binding = new Binding("Id") };
            dataGrid.Columns.Add(itemIdColumn);
            DataGridTextColumn itemDescriptionColumn = new DataGridTextColumn() { Header = "Description Text", Binding = new Binding("DescText") };
            dataGrid.Columns.Add(itemDescriptionColumn);
            return dataGrid;
        }
        #endregion
    }
}
