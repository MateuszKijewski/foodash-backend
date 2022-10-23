namespace FooDash.Persistence.Common.Exceptions
{
    internal class RecordNotFoundException : Exception
    {
        public RecordNotFoundException() : base()
        {
        }

        public RecordNotFoundException(string? message) : base(message)
        {
        }

        public RecordNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public RecordNotFoundException(Type entityType, Guid id)
            : base($"Record of type: {entityType} with Id: {id} was not found")
        {
        }

        public RecordNotFoundException(Type entityType, Guid id, Exception ex)
            : base($"Record of type: {entityType} with Id: {id} was not found", ex)
        {
        }

        public RecordNotFoundException(Type entityType, object obj, Exception ex)
            : base($"Record of type: {entityType} was not found, see ExceptionData for further details", ex)
        {
            base.Data["MissingObject"] = obj;
        }
    }
}