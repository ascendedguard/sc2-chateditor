// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChatMessageEditViewModel.cs" company="Ascend">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Defines the ChatMessageEditViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starcraft2.ChatEditor.ViewModel
{
    using Starcraft2.ChatEditor.Model;
    using Starcraft2.ChatEditor.MVVM;
    using Starcraft2.ReplayParser;

    public class ChatMessageEditViewModel : ObservableObject
    {
        private PlayerDetails[] playerList;

        public PlayerDetails[] PlayerList
        {
            get
            {
                return this.playerList;
            }

            set
            {
                this.playerList = value;
                RaisePropertyChanged("PlayerList");
            }
        }

        private PlayerDetails selectedPlayer;

        public PlayerDetails SelectedPlayer
        {
            get
            {
                return this.selectedPlayer;
            }

            set
            {
                this.selectedPlayer = value;

                this.message.Player = value;

                int index = 0;

                // Find the appropriate index.
                for (; index < this.PlayerList.Length; index++)
                {
                    if (this.PlayerList[index] == value)
                    {
                        break;
                    }
                }

                this.message.ChatMessage.PlayerId = index + 1;
                
                RaisePropertyChanged("SelectedPlayer");
            }
        }
        

        private PlayerChatMessage message;

        public PlayerChatMessage Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
                RaisePropertyChanged("Message");

                if (value == null)
                {
                    this.selectedPlayer = null;
                    RaisePropertyChanged("SelectedPlayer");

                    this.IsMessageAvailable = false;
                }
                else
                {
                    // Manually update the selected player to avoid unnecessary logic.
                    this.selectedPlayer = value.Player;
                    RaisePropertyChanged("SelectedPlayer");

                    this.IsMessageAvailable = true;
                }
            }
        }
        
        private bool isMessageAvailable;

        public bool IsMessageAvailable
        {
            get
            {
                return this.isMessageAvailable;
            }

            private set
            {
                this.isMessageAvailable = value;
                RaisePropertyChanged("IsMessageAvailable");
            }
        }
        
    }
}
