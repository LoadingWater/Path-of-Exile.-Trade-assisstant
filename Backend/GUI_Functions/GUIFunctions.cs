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

    public class GUIFunctions
    {
        Stopwatch cooldown = new Stopwatch();
        bool firstTime = true;
        public async Task GoButtonFunction(CustomClient client, string league, string accountName, TabControl tabControl, Style style)
        {
            try
            {
                //TODO function parametrs null arg check
                //TODO no cooldown if forbidden
                if (cooldown.ElapsedMilliseconds > 60000 || firstTime)
                {
                    firstTime = false;
                    cooldown.Restart();

                    int numberOfStashTabs = await PathOfExileApiFunctions.GetNumberOfStashTabs(client, league, accountName);
                    List<Task<List<TrimmedItemModel>>> tasks = new List<Task<List<TrimmedItemModel>>>();

                    //TODO: 40 prosto stoit tut. ne znaju predela zaprosow na 60 sec
                    for (int i = 0; i < 40; i++)
                    {
                        //didnt work without another variable
                        int tmp = i;
                        tasks.Add(Task.Run(() => PathOfExileApiFunctions.GetItemsInAStashTabAsync(client, league, accountName, tmp, ItemVariant.itemsWithNameAndPrice)));
                    }
                    await Task.WhenAll(tasks);
                    //create and fill structure with tab items
                    for (int stashNumber = 0; stashNumber < 40; stashNumber++)
                    {
                        ItemsForDataGrid itemsForDataGrid = new ItemsForDataGrid();
                        //tasks[stashNumber].Result[itemNumber] 
                        int numberOfItemsInATab = tasks[stashNumber].Result.Count;
                        for (int itemNumber = 0; itemNumber < numberOfItemsInATab; itemNumber++)
                        {
                            //if there are items in a tab add them
                            if (numberOfItemsInATab > 0)
                            {
                                itemsForDataGrid.items.Add(tasks[stashNumber].Result[itemNumber]);
                            }
                        }
                        //if any items in a tab/ if not dont create DataGrid
                        if (tasks[stashNumber].Result.Count > 0)
                        {
                            //TODO:create a control template to elluminate manual creation of the grid
                            DataGrid dataGrid = new DataGrid() { ItemsSource = itemsForDataGrid.items };
                            dataGrid.Style = style;
                            //item[0] null if items == 0
                            TabItem item = new TabItem() { Header = itemsForDataGrid.items[0].TabName, Content = dataGrid };
                            //Creation of DataGrid columns
                            DataGridTextColumn nameColumn = new DataGridTextColumn();
                            nameColumn.Header = "Name";
                            nameColumn.Binding = new Binding("Name");
                            dataGrid.Columns.Add(nameColumn);
                            DataGridTextColumn priceColumn = new DataGridTextColumn();
                            priceColumn.Header = "Price";
                            priceColumn.Binding = new Binding("Price");
                            dataGrid.Columns.Add(priceColumn);

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
    }
}
