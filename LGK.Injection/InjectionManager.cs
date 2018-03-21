// // See LICENSE file in the root directory
// //
using System;
using System.Reflection;

namespace LGK.Injection
{
    public class InjectionManager : IInjectionManager
    {
        readonly DependencyDatabase m_Database;
        readonly Type m_AttributeType;

        public InjectionManager()
        {
            m_Database = new DependencyDatabase();
            m_AttributeType = typeof(InjectDependency);
        }

        #region IInjectionManager implementation

        public void Register<T>(T target)
        {
            m_Database.Create(typeof(T), target);
        }

        public void Register<T>(string name, T target)
        {
            m_Database.Create(name, target);
        }

        public void Unregister<T>()
        {
            m_Database.Remove(typeof(T));
        }

        public void Unregister(string name)
        {
            m_Database.Remove(name);
        }

        public bool TryResolve<T>(out T result)
        {
            var type = typeof(T);
            object tempResult;

            if (!m_Database.TryGet(type, out tempResult))
            {
                result = default(T);
                return false;
            }

            if (tempResult is T)
            {
                result = (T)tempResult;
                return true;
            }

            result = default(T);
            return false;
        }

        public bool TryResolve<T>(string name, out T result)
        {
            object tempResult;

            if (!m_Database.TryGet(name, out tempResult))
            {
                result = default(T);
                return false;
            }

            if (tempResult is T)
            {
                result = (T)tempResult;
                return true;
            }

            result = default(T);
            return false;
        }

        public void Inject<T>(T dependentObject)
        {
            var dependentType = dependentObject.GetType();
            var dependentFields = dependentType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            object result;

            var count = dependentFields.Length;
            for (int i = 0; i < count; i++)
            {
                var dependentField = dependentFields[i];
                var dependentFieldAttributes = dependentField.GetCustomAttributes(m_AttributeType, false);
                if (dependentFieldAttributes.Length > 0)
                {
                    var attribute = dependentFieldAttributes[0] as InjectDependency;
                    if (string.IsNullOrEmpty(attribute.Name))
                    {
                        if (m_Database.TryGet(dependentField.FieldType, out result))
                        {
                            dependentField.SetValue(dependentObject, result);
                        }
                        else
                        {
                            throw new Exception(string.Format("Type of `{0}` not found, may be you not regiter it yet!", dependentField.FieldType.FullName));
                        }
                    }
                    else if (m_Database.TryGet(attribute.Name, out result))
                    {
                        dependentField.SetValue(dependentObject, result);
                    }
                    else
                    {
                        throw new Exception(string.Format("Type of `{0}` with name `{1}` not found, may be you not regiter it yet!", dependentField.FieldType.FullName, attribute.Name));
                    }
                }

            }
        }

        #endregion
    }
}

