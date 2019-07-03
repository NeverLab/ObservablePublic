using System;
using System.Collections.Generic;
using NeverLab.Game.Data;
using NeverLab.Observable;

namespace NeverLab.Game.Entities
{
    public interface IObservableEntity : IObservableHandler<IIterationData>, IObserver<IIterationData>
    {
		IObservable<IIterationData> current { get; }
        IObservable<IIterationData> parent { get; set; }
        List<IObservable<IIterationData>> children { get; }
        void OnEvent(int eventType, IObservable<IIterationData> dispatcher);
    }
}