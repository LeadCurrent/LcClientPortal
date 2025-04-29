using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class NotificationHub : Hub
{
    public async Task NewCustomer()
    {
        await Clients.All.SendAsync("NewCustomer");
    }

    public async Task NewOrder(int OrderId, string TerminalId)
    {
        await Clients.All.SendAsync("NewOrder",OrderId,TerminalId);
    }



    public async Task AddProductToCurrentOrder(string Product, int Quantity, decimal Price, int CustomerId)
    {
        await Clients.All.SendAsync("AddProductToCurrentOrder", Product,Quantity,Price,CustomerId);
    }

    public async Task Test()
    {
        await Clients.All.SendAsync("Test");
    }
}