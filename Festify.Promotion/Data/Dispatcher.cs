using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Festify.Promotion.Data
{
    public class Dispatcher
    {
        public readonly IServiceProvider serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task DispatchAll(IEnumerable<object> entitiesAdded)
        {
            foreach (var entityAdded in entitiesAdded)
            {
                await Dispatch(entityAdded);
            }
        }

        private async Task Dispatch(object entityAdded)
        {
            var notifierType = typeof(INotifier<>).MakeGenericType(entityAdded.GetType());
            var notifyDispatcherType = typeof(NotifyDispatcher<>).MakeGenericType(entityAdded.GetType());
            var collectionType = typeof(IEnumerable<>).MakeGenericType(notifierType);
            var notifiers = (IEnumerable)serviceProvider.GetService(collectionType);

            foreach (var notifier in notifiers)
            {
                var notifyDispatcher = (INotifyDispatcher)Activator.CreateInstance(notifyDispatcherType, notifier);
                await notifyDispatcher.Dispatch(entityAdded);
            }
        }

        interface INotifyDispatcher
        {
            Task Dispatch(object entityAdded);
        }

        class NotifyDispatcher<T> : INotifyDispatcher
        {
            private readonly INotifier<T> notifier;

            public NotifyDispatcher(INotifier<T> notifier)
            {
                this.notifier = notifier;
            }

            public async Task Dispatch(object entityAdded)
            {
                await notifier.Notify((T)entityAdded);
            }
        }
    }
}
