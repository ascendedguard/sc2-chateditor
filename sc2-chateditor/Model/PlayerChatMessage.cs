// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerChatMessage.cs" company="Ascend">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Defines the PlayerChatMessage type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starcraft2.ChatEditor.Model
{
    using Starcraft2.ChatEditor.MVVM;
    using Starcraft2.ReplayParser;

    /// <summary> Combines a ChatMessage with it's appropriate Player, based on PlayerId. </summary>
    public class PlayerChatMessage : ObservableObject
    {
        private PlayerDetails player;

        private ChatMessage chatMessage;

        public PlayerDetails Player
        {
            get
            {
                return this.player;
            }

            set
            {
                this.player = value;
                RaisePropertyChanged("Player");
            }
        }

        public ChatMessage ChatMessage
        {
            get
            {
                return this.chatMessage;
            }

            set
            {
                this.chatMessage = value;
                RaisePropertyChanged("ChatMessage");
            }
        }     
    }
}
