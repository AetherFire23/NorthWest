using Assets.Utils;
using System;
using WebAPI.Enums;

namespace WebAPI.Models
{
    public class Item: IDbKey
    {
        public Guid Key => Id;
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; } // pourrait etre un joueur ou une room
        public ItemType ItemType { get; set; } // Pour l'enum
    }
}
