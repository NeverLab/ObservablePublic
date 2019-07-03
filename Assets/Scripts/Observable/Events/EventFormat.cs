using System;

namespace NeverLab.Observable.Events
{
    public class EventFormat : IEventFormat
    {
        private static EventFormat _Instance;
        public static EventFormat Base
        {
            get
            {
                if (_Instance == null)
                    _Instance = new EventFormat();
                return _Instance;
            }
        }

        enum EventType
        {
            Birth,
            Death
        }

        public int TotalEventTypesCount
        {
            get
            {
                return Enum.GetNames(typeof(EventType)).Length;
            }
        }

        public int EventTypeBirth
        {
            get
            {
                return (int)EventType.Birth;
            }
        }
        public int EventTypeDeath
        {
            get
            {
                return (int)EventType.Death;
            }
        }

        public Enum getEnumById(int id)
        {
            return (EventType)id;
        }
    }
}