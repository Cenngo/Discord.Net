using System;

namespace Discord.WebSocket
{
    public class SocketInteractionParameter
    {
        public string Name { get; }
        public object Value { get; }
        public Type Type { get; }

        internal SocketInteractionParameter ( string name, object value, Type type )
        {
            Name = name;
            Value = value;
            Type = type;
        }

        internal static SocketInteractionParameter Create<T> (string name, object value) =>
            new SocketInteractionParameter(name, value, typeof(T));
    }
}
