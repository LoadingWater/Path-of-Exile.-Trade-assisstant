using Backend.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using Backend.Database;
using System.Linq;
using System.Windows.Media;
using System.Windows;

namespace Backend.ApplicationViewModel
{
    public class GuiFunctions
    {
        public void CreateDataGrid(DatabaseContext database, TabControl mainTabControl)
        {
            try
            {
                int numberOfTabs = database.Tabs.Count((x) => x.TabId != null);

                for (int tabNumber = 0; tabNumber < numberOfTabs; tabNumber++)
                {
                    //Create DataGrid items source
                    List<DataGridItemModel> dataGridItems = new List<DataGridItemModel>();
                    var tab = database.Tabs.Where((x) => x.TabIndex == tabNumber).First();
                    var itemsInATab = database.Items.Where((x) => x.TabId == tab.TabId).ToList();
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
                        });
                    }
                    //Create DataGrid for every tab
                    if (dataGridItems.Count != 0)
                    {
                        DataGrid dataGrid = new DataGrid() { ItemsSource = dataGridItems };
                        TabItem tabItem = new TabItem() { Header = tab.TabName, Content = dataGrid, Background = GetTabItemColour(tab) };

                        dataGrid.Columns.Add(CreateDataGridColumn("Name", "ItemName"));
                        dataGrid.Columns.Add(CreateDataGridColumn("Note", "ItemNote"));

                        mainTabControl.Items.Add(tabItem);
                    }

                    /*Style cellStyle = new Style();
                    Setter s = new Setter();
                    s.Property = DataGridCell.BackgroundProperty;
                    s.Value = Brushes.Red;
                    cellStyle.Setters.Add(s);
                    nameColumn.CellStyle = cellStyle;*/
                    /*dataGrid.SelectAllCells();
                    foreach (var cell in dataGrid.SelectedCells)
                    {
                        DataGridItemModel item = (DataGridItemModel)cell.Item;
                        if (item.ItemFrameType == 0)
                        {
                            Style cellStyle = new Style();
                            Setter s = new Setter();
                            s.Property = DataGridCell.BackgroundProperty;
                            s.Value = Brushes.Red;
                            cellStyle.Setters.Add(s);
                            cell.Column.CellStyle = cellStyle;
                        }
                    }*/
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            
        }

        private DataGridTextColumn CreateDataGridColumn(string columnName, string propertyToBindHeaderTo)
        {
            DataGridTextColumn column = new DataGridTextColumn();
            column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            column.Header = columnName;
            column.Binding = new Binding(propertyToBindHeaderTo);
            return column;
        }

        private SolidColorBrush GetTabItemColour(Tab tab)
        {
            return new SolidColorBrush(Color.FromRgb((byte)tab.TabColourRed, (byte)tab.TabColourGreen, (byte)tab.TabColourBlue));
        }
    }
}

