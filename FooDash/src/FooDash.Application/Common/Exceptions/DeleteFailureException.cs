using System.Runtime.Serialization;
using System.Security.Permissions;

namespace FooDash.Application.Common.Exceptions
{
    [Serializable]
    public class DeleteFailureException : Exception
    {
        public string Name { get; }
        public object Key { get; }

        public DeleteFailureException(string name, object key, string message)
            : base($"Deletion of entity \"{name}\" ({key}) failed. {message}")
        {
            Name = name;
            Key = key;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected DeleteFailureException(SerializationInfo info, StreamingContext context)
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