// // See LICENSE file in the root directory
// //

namespace LGK.Injection
{
    public interface IInjectionManager
    {
        void Register<T>(string name, T target);

        void Register<T>(T target);

        void Unregister(string name);

        void Unregister<T>();

        bool TryResolve<T>(out T result);

        bool TryResolve<T>(string name, out T result);

        void Inject<T>(T dependentObject);

    }
}

