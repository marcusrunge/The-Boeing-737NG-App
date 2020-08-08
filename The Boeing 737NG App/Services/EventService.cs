using System;

namespace The_Boeing_737NG_App.Services
{
    public interface IEventService
    {
        event EventHandler ProgressChangedEvent;
        event EventHandler DatabaseUpdatedEvent;
        event EventHandler ForceFragmentBackStackEvent;
        void OnProgressChangedEvent(ProgressChangedEventArgs eventArgs);
        void OnDatabaseUpdatedEvent(EventArgs eventArgs);
        void OnForceFragmentBackStackEvent(EventArgs eventArgs);
    }
    public class EventService : IEventService
    {
        public event EventHandler ProgressChangedEvent;
        public event EventHandler DatabaseUpdatedEvent;
        public event EventHandler ForceFragmentBackStackEvent;

        public static EventService Instance() => new EventService();
        public void OnProgressChangedEvent(ProgressChangedEventArgs eventArgs) => ProgressChangedEvent?.Invoke(this, eventArgs);
        public void OnDatabaseUpdatedEvent(EventArgs eventArgs) => DatabaseUpdatedEvent?.Invoke(this, eventArgs);
        public void OnForceFragmentBackStackEvent(EventArgs eventArgs) => ForceFragmentBackStackEvent?.Invoke(this, eventArgs);
    }

    public class ProgressChangedEventArgs : EventArgs
    {
        public ProgressChangedEventArgs(int progress) => Progress = progress;
        public int Progress { get; }
    }

    public class DatabaseUpdatedEventArgs<T> : EventArgs
    {
        public T Item { get; }
        public UpdateType Update { get; }
        public DatabaseUpdatedEventArgs(T item, UpdateType update)
        {
            Item = item;
            Update = update;
        }
    }

    public enum UpdateType { Add, Update, Delete };
}