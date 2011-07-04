// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationView.xaml.cs" company="Ascend">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Interaction logic for ApplicationView.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starcraft2.ChatEditor.View
{
    using Starcraft2.ChatEditor.ViewModel;

    /// <summary>
    /// Interaction logic for ApplicationView.xaml
    /// </summary>
    public partial class ApplicationView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationView"/> class.
        /// </summary>
        public ApplicationView()
        {
            InitializeComponent();
        }

        private void FileDrop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                var arr = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];

                if (arr != null)
                {
                    var viewModel = this.DataContext as ApplicationViewModel;

                    if (viewModel == null)
                    {
                        return;
                    }

                    foreach (var s in arr)
                    {
                        viewModel.LoadReplay(s);
                    }
                }
            }
        }
    }
}
