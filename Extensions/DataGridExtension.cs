
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DocumentHelper.Extensions
{
    public static class DataGridExtension
    {
        public static DataGridCell GetCurrentCell(this DataGrid grid)
        {
            if (grid.CurrentItem == null) return null;

            int rowIndex = grid.Items.IndexOf(grid.CurrentItem);
            var row = grid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;

            if (row != null)
            {
                int columnIndex = grid.CurrentCell.Column.DisplayIndex;
                var presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter != null)
                {
                    return presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
                }
            }

            return null;
        }

        private static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i) as Visual;
                if (child is T result)
                    return result;

                var descendant = GetVisualChild<T>(child);
                if (descendant != null)
                    return descendant;
            }

            return null;
        }
    }
}
