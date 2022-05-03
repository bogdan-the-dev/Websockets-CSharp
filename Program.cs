// See https://aka.ms/new-console-template for more information

using System.Net;
using WebSockets;

IPEndPoint senderEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.15"), 6060);
IPEndPoint d1Endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6061);
IPEndPoint d2Endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.2"), 6062);
IPEndPoint d3Endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.3"), 6063);

DestinationNode d1 = new DestinationNode(d1Endpoint, d2Endpoint, senderEndpoint);
DestinationNode d2 = new DestinationNode(d2Endpoint, d3Endpoint, d1Endpoint);
DestinationNode d3 = new DestinationNode(d3Endpoint, null, d2Endpoint);
SenderNode sender = new SenderNode(senderEndpoint, d1Endpoint, d2Endpoint, d3Endpoint);

sender.SendAll();

Console.ReadLine();
