using System.Runtime.Serialization;
using System.Security.Permissions;

namespace FooDash.Application.Common.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public string Name { get; }
        public object Key { get; }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
            Name = name;
            Key = key;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Name = info.GetString(nameof(Name));
            Key = info.GetValue(nameof(Key), typeof(object));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info is null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(Name), Name);
            info.AddValue(nameof(Key), Key);
            base.GetObjectData(info, context);
        }
    }
}