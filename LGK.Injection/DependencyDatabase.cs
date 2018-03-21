// See LICENSE file in the root directory
//
using System;
using System.Collections.Generic;

namespace LGK.Injection
{
    public class DependencyDatabase
    {
        readonly IDictionary<string, object> m_NamedDependencies;
        readonly IDictionary<Type, object> m_TypedDependencies;

        public DependencyDatabase()
        {
            m_NamedDependencies = new Dictionary<string, object>();
            m_TypedDependencies = new Dictionary<Type, object>();
        }

        public void Create(string name, object target)
        {
            m_NamedDependencies.Add(name, target);
        }

        public void Create(Type type, object target)
        {
            m_TypedDependencies.Add(type, target);
        }

        public void Remove(string name)
        {
            m_NamedDependencies.Remove(name);
        }

        public void Remove(Type type)
        {
            m_TypedDependencies.Remove(type);
        }

        public bool TryGet(Type type, out object result)
        {
            return m_TypedDependencies.TryGetValue(type, out result);
        }

        public bool TryGet(string name, out object result)
        {
            return m_NamedDependencies.TryGetValue(name, out result);
        }
    }
}

