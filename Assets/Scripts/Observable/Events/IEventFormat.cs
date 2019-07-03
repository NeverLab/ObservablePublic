using System;

namespace NeverLab.Observable.Events
{
    public interface IEventFormat
    {
        int EventTypeBirth { get; }
        int EventTypeDeath { get; }

        Enum getEnumById(int id);
        int TotalEventTypesCount { get; }
    }
}