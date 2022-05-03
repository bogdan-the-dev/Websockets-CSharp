using System.Net;
using System.Net.Sockets;

namespace WebSockets;

public class SenderNode
{
    private IPEndPoint _nextEndpoint;
    private IPEndPoint _D2;
    private IPEndPoint _D3;
    private UdpClient _udpClient;

    public SenderNode(IPEndPoint ownEndpoint, IPEndPoint nextEndpoint, IPEndPoint d2, IPEndPoint d3)
    {
        _nextEndpoint = nextEndpoint;
        _D2 = d2;
        _D3 = d3;
        _udpClient = new UdpClient(ownEndpoint);
        try{
            _udpClient.Connect(_nextEndpoint.Address, ownEndpoint.Port);
        }
        catch (Exception e ) {
            Console.WriteLine(e.ToString());
        }
    }


    public void SendAll()
    {
        Random r = new Random();
        int i;
        for (i = 1; i <= 100; i++)
        {
            IPEndPoint destination;
            int randomDestination = r.Next(1, 4);
            switch (randomDestination)
            {
                case 2 :
                   destination = new IPEndPoint(_D2.Address, _D2.Port - 1);
                   break;
               case 3 :
                   destination = new IPEndPoint(_D3.Address, _D3.Port - 1);
                   break;
               default:
                   destination = new IPEndPoint(_nextEndpoint.Address, _nextEndpoint.Port - 1);
                   break;
            }
            
             Package package = new Package(destination.Address.ToString(), i); 
            Console.WriteLine("Target: " + package.TargetIp);
            byte[] data = package.ObjectToByteArray();
            _udpClient.BeginSend(data, data.Length, OnSend, null);
        }
    }

    private void OnSend(IAsyncResult result)
    {
        _udpClient.EndSend(result);
    }
}