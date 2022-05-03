using System.Net;
using System.Net.Sockets;

namespace WebSockets;

public class DestinationNode
{
    private IPEndPoint _ownEndpoint;
    private IPEndPoint? _sendTo;
    private IPEndPoint? _receiveFrom;
    private UdpClient _udpClientReceive;
    private UdpClient _udpClientSend;
    
    public DestinationNode(IPEndPoint ownIp, IPEndPoint? sendTo, IPEndPoint receiveFrom)
    {
        _sendTo = sendTo;
        _ownEndpoint = ownIp;
        _receiveFrom = receiveFrom;
        _udpClientReceive = new UdpClient(new IPEndPoint(_ownEndpoint.Address, _receiveFrom.Port));

        if (_receiveFrom != null)
        {
            try{
                _udpClientReceive.Connect(_receiveFrom);
            }
            catch (Exception e ) {
                Console.WriteLine(e.ToString());
            }
        }
            
        if (_sendTo != null)
        {
            try{
                _udpClientSend = new UdpClient(new IPEndPoint(_ownEndpoint.Address, _ownEndpoint.Port));
                _udpClientSend.Connect(new IPEndPoint(_sendTo.Address, _ownEndpoint.Port));
            }
            catch (Exception e ) {
                Console.WriteLine(e.ToString());
            }
        }
        _udpClientReceive.BeginReceive(OnReceive, new IPEndPoint(_ownEndpoint.Address, _receiveFrom.Port));
    }

    private void SendToNexNode(Package response)
    {
        byte[] data = response.ObjectToByteArray();
        _udpClientSend.BeginSend(data, data.Length, OnSend, null);
    }

    private void OnReceive(IAsyncResult result)
    {
        Console.WriteLine("Own ip: " + _ownEndpoint.Address);
        var senderEndpoint = (IPEndPoint) result.AsyncState;
        
        Package response = new Package();
        response.ByteArrayToObject(_udpClientReceive.EndReceive(result, ref senderEndpoint));

        Console.WriteLine("Target: " + response.TargetIp + "  " + response.Value );
        
        if (response.TargetIp != _ownEndpoint.Address.ToString())
        {
            SendToNexNode(response);
        }
        _udpClientReceive.BeginReceive(OnReceive, new IPEndPoint(_ownEndpoint.Address, _receiveFrom.Port));
    }

    private void OnSend(IAsyncResult result)
    {
        _udpClientSend.EndSend(result);
    }

}