// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReplayEditorViewModel.cs" company="Ascend">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   ViewModel for the main application window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starcraft2.ChatEditor.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Data;
    using System.Windows.Input;

    using Starcraft2.ChatEditor.Model;
    using Starcraft2.ChatEditor.MVVM;
    using Starcraft2.ReplayParser;

    /// <summary> ViewModel for the main application window. </summary>
    public class ReplayEditorViewModel : ObservableObject
    {
        /// <summary> Backing field for the CurrentFile property. </summary>
        private string currentFile;

        private string replayHeader;

        public event EventHandler CloseRequested;

        private ICommand closeCommand;
        public ICommand CloseCommand
        {
            get
			{
				return this.closeCommand ?? (this.closeCommand = new RelayCommand(this.OnRequestClose));
			}
        }

        private void OnRequestClose()
        {
            var handler = this.CloseRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public string ReplayHeader
        {
            get
            {
                return this.replayHeader;
            }

            set
            {
                this.replayHeader = value;
                RaisePropertyChanged("ReplayHeader");
            }
        }
        
        public ReplayEditorViewModel(string replayPath)
        {
            this.CurrentFile = replayPath;

            this.replay = Replay.Parse(this.CurrentFile);
            this.ReplayHeader = Path.GetFileName(this.CurrentFile);
            var messages = new ObservableCollection<PlayerChatMessage>();

            foreach (var message in this.replay.ChatMessages)
            {
                int id = message.PlayerId - 1;
                if (id < this.replay.Players.Length)
                {
                    var chat = new PlayerChatMessage { ChatMessage = message, Player = this.replay.Players[message.PlayerId - 1] };
                    messages.Add(chat);
                }
            }

            this.ChatMessageEditor = new ChatMessageEditViewModel { PlayerList = this.replay.Players };

            this.ChatMessages = messages;
        }

        /// <summary> Backing for the ClearAllCommand command. </summary>
        private ICommand clearAllCommand;

        /// <summary> Gets a command which </summary>
        public ICommand ClearAllCommand
        {
            get
            {
	            return this.clearAllCommand ?? (this.clearAllCommand = new RelayCommand(this.ClearAllMessages));
            }
        }

        private ICommand addNewCommand;
        public ICommand AddNewCommand
        {
            get
            {
                return this.addNewCommand ?? (this.addNewCommand = new RelayCommand(this.AddNew, () => this.replay != null));
            }
        }

        private void AddNew()
        {
            var message = new PlayerChatMessage 
                {
                    Player = this.chatMessageEditor.PlayerList[0],
                    ChatMessage = new ChatMessage { PlayerId = 1, MessageTarget = ChatMessageTarget.All, }
                };

            this.ChatMessages.Add(message);
            this.SelectedChatMessage = message;
        }

        private ICommand removeSelectedCommand;
        public ICommand RemoveSelectedCommand
        {
            get
            {
                return this.removeSelectedCommand ?? (this.removeSelectedCommand = new RelayCommand(this.RemoveSelected, () => this.SelectedChatMessage != null));
            }
        }

        private void RemoveSelected()
        {
            this.ChatMessages.Remove(this.SelectedChatMessage);
            this.SelectedChatMessage = null;
        }

        private void ClearAllMessages()
        {
            this.ChatMessages.Clear();
        }

        private ICommand saveAsCommand;
        public ICommand SaveAsCommand
        {
            get
			{
				return this.saveAsCommand ?? (this.saveAsCommand = new RelayCommand(this.SaveAs, () => this.replay != null));
			}
        }

        private void SaveAs()
        {
            var sfd = new Microsoft.Win32.SaveFileDialog
                { Filter = "Starcraft 2 Replay (*.sc2replay)|*.sc2replay|All Files (*.*)|*.*" };

            if (sfd.ShowDialog() == true)
            {
                var fileName = sfd.FileName;
                if (this.currentFile != fileName)
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    File.Copy(this.currentFile, fileName);                    
                }

                List<ChatMessage> messages = new List<ChatMessage>();
                
                foreach (var msg in this.ChatMessages)
                {
                    messages.Add(msg.ChatMessage);
                }

                ReplayMessageEvents.ClearChatLog(fileName);
                ReplayMessageEvents.AddChatMessageToReplay(fileName, messages);
            }
        }

        /// <summary> Gets or sets the current file being editted. </summary>
        public string CurrentFile
        {
            get
            {
                return this.currentFile;
            }

            set
            {
                this.currentFile = value;
                RaisePropertyChanged("CurrentFile");
            }
        }

        private Replay replay = null;

        private ObservableCollection<PlayerChatMessage> chatMessages;

        public ObservableCollection<PlayerChatMessage> ChatMessages
        {
            get
            {
                return this.chatMessages;
            }

            set
            {
                this.chatMessages = value;
                var collection = new CollectionViewSource()
                    {
                        Source = value
                    };

                collection.SortDescriptions.Add(
                    new SortDescription() { PropertyName = "ChatMessage.Timestamp" });


                RaisePropertyChanged("ChatMessages");

                this.ChatCollection = collection;
            }
        }

        private bool lockSelection = false;

        private PlayerChatMessage selectedChatMessage;

        public PlayerChatMessage SelectedChatMessage
        {
            get
            {
                return this.selectedChatMessage;
            }

            set
            {
                if (this.lockSelection == false)
                {
                    this.selectedChatMessage = value;

                    if (this.ChatMessageEditor != null)
                    {
                        this.ChatMessageEditor.Message = value;
                    }

                    if (this.ChatCollection != null)
                    {
                        // lock the selection, it likes to put it back to null for some reason
                        // and this prevents a stackoverflowexception
                        this.lockSelection = true;
                        this.ChatCollection.View.Refresh();
                        this.lockSelection = false;
                    }                                        
                }

                RaisePropertyChanged("SelectedChatMessage");                    
            }
        }

        private ChatMessageEditViewModel chatMessageEditor;

        public ChatMessageEditViewModel ChatMessageEditor
        {
            get
            {
                return this.chatMessageEditor;
            }

            set
            {
                this.chatMessageEditor = value;
                RaisePropertyChanged("ChatMessageEditor");
            }
        }

        private CollectionViewSource chatCollection;

        public CollectionViewSource ChatCollection
        {
            get
            {
                return this.chatCollection;
            }

            set
            {
                this.chatCollection = value;
                RaisePropertyChanged("ChatCollection");
            }
        }
    }
}
