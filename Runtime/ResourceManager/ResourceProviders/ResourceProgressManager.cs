using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.Util;

// ReSharper disable once CheckNamespace //TODO: try to use rootNamespace in .asmdef in 2020.2
namespace UnityEngine.ResourceManagement.ResourceProviders
{
    public class ResourceProgressManager : ComponentSingleton<ResourceProgressManager>
    {
        //public static ResourceProgressManager Instance = new ResourceProgressManager();

        public enum ErrorActionErrand
        {
            Fail = 0,
            Repeat
        }

        public interface IHandler //TODO: share with fs remote driver's one
        {
            Task<ErrorActionErrand> OnError();
        }

        public IHandler Handler { get; set; }

        private int _errorActionCount;
        private readonly ConcurrentQueue<Action<ErrorActionErrand>> _errorActions = new ConcurrentQueue<Action<ErrorActionErrand>>();

        //TODO: log processing
        public void ProcessError(Action<ErrorActionErrand> errorAction)
        {
            var handler = Handler;
            if (handler == null)
            {
                errorAction(ErrorActionErrand.Fail);
                return;
            }

            _errorActions.Enqueue(errorAction);
            if (Interlocked.Increment(ref _errorActionCount) == 0)
            {
                throw new NotImplementedException(); //XXXXXXXX
                // handler.(errand =>
                // {
                // });
            }
        }
    }
}
