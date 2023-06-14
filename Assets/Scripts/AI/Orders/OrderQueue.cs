using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public class OrderQueue
{
    private Queue<IOrder> orderQueue = new Queue<IOrder>();
    private ReactiveProperty<IOrder> currentOrder = new ReactiveProperty<IOrder>(null);

    public void Enqueue(IOrder order)
    {
        orderQueue.Enqueue(order);
    }

    public void Dequeue(IOrder order)
    {
        orderQueue = new Queue<IOrder>(orderQueue.Where(o => o != order));
    }

    public void ClearQueue()
    {
        orderQueue.Clear();
    }

    public void ExecuteNextOrder()
    {
        while (orderQueue.Count > 0)
        {
            IOrder order = orderQueue.Dequeue();

            order.ExecuteOrder();
        }
    }
}