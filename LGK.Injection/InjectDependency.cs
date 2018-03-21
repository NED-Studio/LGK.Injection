// See LICENSE file in the root directory
//

using System;

namespace LGK.Injection
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectDependency : Attribute
    {
        public readonly string Name;

        public InjectDependency()
        {
            Name = null;
        }

        public InjectDependency(string name)
        {
            Name = name;
        }
    }
}

