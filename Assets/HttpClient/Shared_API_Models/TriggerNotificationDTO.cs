using Assets.Enums;
using System;

namespace WebAPI.GameState_Management
{
    public class TriggerNotificationDTO
    {
        public Guid Id { get; set; }
        public Guid GameActionId { get; set; }
        public Guid PlayerId { get; set; }
        public bool IsReceived { get; set; }
        public DateTime Created { get; set; }
        public NotificationType NotificationType { get; set; }
        public object? ExtraProperties { get; set; }
    }
}