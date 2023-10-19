namespace Finance.Exceptions;

public class FinanceException : Exception
{
    public FinanceException() { }

    public FinanceException(string message) : base(message) { }

    public FinanceException(string message, Exception innerException) : base(message, innerException) { }
}

public class EntityNotFoundException : FinanceException
{
    public EntityNotFoundException() { }
    public EntityNotFoundException(string message) : base(message) { }
    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    public EntityNotFoundException(Type type) : base(GetMessage(type)) { }
    public EntityNotFoundException(Type type, int id) : base(GetMessage(type, id)) { }
    public EntityNotFoundException(Type type, int id, Exception innerException) : base(GetMessage(type, id), innerException) { }

    public static string GetMessage(Type type)
    {
        return $"The entity of type {type.Name} was not found.";
    }

    public static string GetMessage(Type type, int? id = null)
    {
        return $"The entity of type {type.Name} with id {id} was not found.";
    }
}

public class EntityNotNullException : FinanceException
{
    public EntityNotNullException() { }
    public EntityNotNullException(string message) : base(message) { }
    public EntityNotNullException(string message, Exception innerException) : base(message, innerException) { }
    public EntityNotNullException(Type type) : base(GetMessage(type)) { }
    public EntityNotNullException(Type type, Exception innerException) : base(GetMessage(type), innerException) { }

    public static string GetMessage(Type type)
    {
        return $"The entity of type {type.Name} cannot be null.";
    }
}
