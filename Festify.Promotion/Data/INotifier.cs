namespace Festify.Promotion.Data;

public interface INotifier<T>
{
    Task Notify(T entityAdded);
}