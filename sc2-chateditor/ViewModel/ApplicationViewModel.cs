// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationViewModel.cs" company="Ascend">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Defines the ApplicationViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starcraft2.ChatEditor.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Starcraft2.ChatEditor.MVVM;

    public class ApplicationViewModel : ObservableObject
    {
        private ICommand loadReplayCommand;
        public ICommand LoadReplayCommand
        {
            get
            {
                return this.loadReplayCommand ?? (this.loadReplayCommand = new RelayCommand(this.LoadReplay));
            }
        }

        private ObservableCollection<ReplayEditorViewModel> openReplays = new ObservableCollection<ReplayEditorViewModel>();

        public ObservableCollection<ReplayEditorViewModel> OpenReplays
        {
            get
            {
                return this.openReplays;
            }

            set
            {
                this.openReplays = value;
                RaisePropertyChanged("OpenReplays");
            }
        }

        private ReplayEditorViewModel selectedReplay;

        public ReplayEditorViewModel SelectedReplay
        {
            get
            {
                return this.selectedReplay;
            }

            set
            {
                this.selectedReplay = value;
                RaisePropertyChanged("SelectedReplay");
            }
        }

        private bool collectHasItems;

        public bool CollectionHasItems
        {
            get
            {
                return this.collectHasItems;
            }

            set
            {
                this.collectHasItems = value;
                RaisePropertyChanged("CollectionHasItems");
            }
        }
        

        private void LoadReplay()
        {
            var ofd = new Microsoft.Win32.OpenFileDialog
                { Filter = "SC2Replay Files (*.sc2replay)|*.sc2replay|All Files (*.*)|*.*" };

            if (ofd.ShowDialog() == true)
            {
                var replay = new ReplayEditorViewModel(ofd.FileName);
                replay.CloseRequested += (sender, e) =>
                    {
                        this.OpenReplays.Remove(replay);
                        if (this.OpenReplays.Count == 0)
                        {
                            this.CollectionHasItems = false;
                        }
                    };
                this.openReplays.Add(replay);
                this.SelectedReplay = replay;
                this.CollectionHasItems = true;
            }
        }

    }
}
