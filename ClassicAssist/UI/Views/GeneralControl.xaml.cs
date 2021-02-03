using System.Windows.Controls;

namespace ClassicAssist.UI.Views
{
    /// <summary>
    ///     Interaction logic for GeneralControl.xaml
    /// </summary>
    public partial class GeneralControl : UserControl
    {
        public GeneralControl()
        {
            InitializeComponent();
        }

        private void GridSplitter_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {

        }
    }

    public class CustomTabControl : TabControl
    {
        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo)
        {
            foreach (TabItem item in this.Items)
            {
                double newW = (this.ActualWidth / Items.Count) - 1;
                if (newW < 0) newW = 0;

                item.Width = newW;
            }
        }
    }
}