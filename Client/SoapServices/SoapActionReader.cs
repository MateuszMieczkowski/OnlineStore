using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace OnlineStore.Client.SoapServices;

public class SoapActionReader
{
    public static string? GetSoapAction<T>(Expression<Func<T, Task>> methodExpression)
    {
        if (methodExpression.Body is not MethodCallExpression methodCall)
            throw new ArgumentException("Wyrażenie musi być wywołaniem metody.");

        var methodInfo = methodCall.Method;

        var attr = methodInfo
            .GetCustomAttributes(typeof(OperationContractAttribute), true)
            .FirstOrDefault() as OperationContractAttribute;

        return attr?.Action;
    }
    
    public static string GetDataContractNamespace(Type type)
    {
        var attr = (DataContractAttribute?)Attribute.GetCustomAttribute(type, typeof(DataContractAttribute));
        return attr?.Namespace ?? $"http://schemas.datacontract.org/2004/07/{type.Namespace}";
    }
}
