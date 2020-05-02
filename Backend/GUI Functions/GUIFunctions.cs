using Backend.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Backend.APIFunctions;

namespace Backend.GUIFunctions
{

    public class GUIFunctions
    {
        Stopwatch cooldown = new Stopwatch();
        bool firstTime = true;
        public async Task GoButtonFunction(string poeSessid, string league, string accountName, TabControl tabControl, Style style)
        {
            try
            {
                //TODO function parametrs null arg check
                if (cooldown.ElapsedMilliseconds > 60000 || firstTime)
                {
                    firstTime = false;
                    cooldown.Restart();
                    int numberOfStashTabs = await PathOfExileApiFunctions.GetStashTabNumbersAsync(poeSessid, league, accountName);

                    List<Task<List<TrimmedItemModel>>> tasks = new List<Task<List<TrimmedItemModel>>>();
                    for (int i = 0; i < 40; i++)
                    {
                        //didnt work without another variable
                        int tmp = i;
                        tasks.Add(Task.Run(() => PathOfExileApiFunctions.GetItemsInAStashTabAsync(poeSessid, league, accountName, tmp, ItemVariant.itemsWithNameAndPrice)));
                    }
                    await Task.WhenAll(tasks);
                    //tasks[stashNumber].Result[itemNumber] 
                    //create and fill structure with tab items
                    for (int i = 0; i < 40; i++)
                    {
                        ItemsForDataGrid itemsForDataGrid = new ItemsForDataGrid();
                        for (int i2 = 0; i2 < tasks[i].Result.Count; i2++)
                        {
                            //if there are items in a tab add them
                            if (tasks[i].Result.Count != 0)
                            {
                                itemsForDataGrid.items.Add(tasks[i].Result[i2]);
                            }
                        }
                        //if there are items in a tab => create dataGrid
                        if (tasks[i].Result.Count != 0)
                        {
                            DataGrid dataGrid = new DataGrid() { ItemsSource = itemsForDataGrid.items };
                            dataGrid.Style = style;
                            /*DockPanel dock = new DockPanel();
                            DockPanel.SetDock(dataGrid, Dock.Top);
                            dock.Children.Add(dataGrid);*/

                            TabItem item = new TabItem() { Header = itemsForDataGrid.items[0].TabName, Content = dataGrid };
                            //columns
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
                MessageBox.Show(ex.Message);
            }
        }
    }
}
