using Microsoft.Extensions.Logging;

namespace AchiesUtilities.Logging;

//public interface ILogFactory : ILoggerFactory
//{
//    /// <summary>
//    /// WARN: currently not recommended with multiple properties. (Creates wrapper on each property)
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    /// <param name="logger"></param>
//    /// <param name="name"></param>
//    /// <param name="property"></param>
//    /// <returns></returns>
//    public ILogger<T> WithProperty<T>(ILogger<T> logger, string name, object? property);

//    /// <summary>
//    /// WARN: currently not recommended with multiple properties. (Creates wrapper on each property)
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    /// <param name="logger"></param>
//    /// <param name="name"></param>
//    /// <param name="property"></param>
//    /// <returns></returns>
//    public ILogger WithProperty<T>(ILogger logger, string name, T property);
//    public ILogger<T> WithProperties<T>(ILogger<T> logger, IEnumerable<KeyValuePair<string, object?>> properties);
//}