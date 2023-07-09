using Shared_Resources.Enums;
using System;

namespace Assets.SSE
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventMethodMappingAttribute : Attribute
    {
        public readonly SSEType EventType;
        public EventMethodMappingAttribute(SSEType eventType)
        {
            EventType = eventType;
        }
    }
}
