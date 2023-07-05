using Shared_Resources.Enums;
using System;

namespace Assets.SSE
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventMapAttribute : Attribute
    {
        public readonly SSEEventType EventType;
        public EventMapAttribute(SSEEventType eventType)
        {
            EventType = eventType;
        }
    }
}
