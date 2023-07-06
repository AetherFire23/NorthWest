using Shared_Resources.Enums;
using System;

namespace Assets.SSE
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventMethodMappingAttribute : Attribute
    {
        public readonly SSEEventType EventType;
        public EventMethodMappingAttribute(SSEEventType eventType)
        {
            EventType = eventType;
        }
    }
}
