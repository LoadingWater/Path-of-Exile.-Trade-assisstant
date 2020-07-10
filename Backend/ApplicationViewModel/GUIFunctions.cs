using Backend.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using Backend.Database;
using System.Linq;
using System.Windows.Media;
using System.Windows;
using System;

namespace Backend.ApplicationViewModel
{
    public class GuiFunctions
    {
        public void CreateDataGrid(DatabaseContext database, TabControl mainTabControl, string league)
        {
            try
            {
                //serach items with given leagie
                mainTabControl.Items.Clear();
                int numberOfTabs = database.Tabs.Count((x) => x.TabLeague == league);

                //Iterate every stash tab 
                for (int tabNumber = 0; tabNumber < numberOfTabs; tabNumber++)
                {
                    //Create DataGrid items source
                    List<DataGridItemModel> dataGridItems = new List<DataGridItemModel>();
                    Tab tab = database.Tabs.Where((x) => x.TabIndex == tabNumber && x.TabLeague == league).First();
                    List<Item> itemsInATab = database.Items.Where((x) => x.TabId == tab.TabId).ToList();
                    //Init all items in a stash tab
                    foreach (var item in itemsInATab)
                    {
                        dataGridItems.Add(new DataGridItemModel()
                        {
                            ItemFrameType = item.ItemFrameType,
                            ItemIconAddress = item.ItemIconAddress,
                            ItemId = item.ItemId,
                            ItemName = item.ItemName,
                            ItemNote = item.ItemNote,
                            TabId = tab.TabId,
                            ItemAffixes = item.ItemAffixes,
                            ElapsedTime = FormatDateTime(DateTime.Now.Subtract(DateTime.Parse(item.CreationTime))),
                            ElapsedTimeFromTheLastPriceChange = FormatDateTime(DateTime.Now.Subtract(DateTime.Parse(item.PriceChangedTime)))
                        });
                    }
                    //Create DataGrid for every tab if any items
                    if (dataGridItems.Count != 0)
                    {
                        DataGrid dataGrid = new DataGrid() { ItemsSource = dataGridItems };
                        dataGrid.BorderBrush = Brushes.Blue;
                        TabItem tabItem = new TabItem() { Header = tab.TabName, Content = dataGrid, Background = GetTabItemColour(tab) };

                        dataGrid.Columns.Add(CreateDataGridTextColumn("Name", "ItemName"));
                        dataGrid.Columns.Add(CreateDataGridTextColumn("Note", "ItemNote"));
                        dataGrid.Columns.Add(CreateDataGridComboBoxColumn("Description"));
                        dataGrid.Columns.Add(CreateDataGridTextColumn("Elapsed time", "ElapsedTime"));
                        dataGrid.Columns.Add(CreateDataGridTextColumn("Price Changed", "ElapsedTimeFromTheLastPriceChange"));

                        mainTabControl.Items.Add(tabItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private DataGridTextColumn CreateDataGridTextColumn(string columnName, string propertyToBindHeaderTo)
        {
            DataGridTextColumn column = new DataGridTextColumn();
            column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            column.Header = columnName;
            column.Binding = new Binding(propertyToBindHeaderTo);
            return column;
        }

        private DataGridComboBoxColumn CreateDataGridComboBoxColumn(string columnName)
        {
            DataGridComboBoxColumn column = new DataGridComboBoxColumn();
            column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            column.Header = columnName;
            return column;
        }

        private SolidColorBrush GetTabItemColour(Tab tab)
        {
            return new SolidColorBrush(Color.FromRgb((byte)tab.TabColourRed, (byte)tab.TabColourGreen, (byte)tab.TabColourBlue));
        }
        private string FormatDateTime(TimeSpan time)
        {
            return $"{time.Days} d/{time.Hours} h/{time.Minutes}m";
        }
    }
}

