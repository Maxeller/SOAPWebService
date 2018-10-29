using System;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace SOAPWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            if(Debugger.IsAttached)
            {
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US"); //Forces VS errors to be in english while debugging
            }

            Uri baseAddress = new Uri("http://localhost:8888/myservice/");
            ServiceHost host = new ServiceHost(typeof(MyService), baseAddress);
            
            ServiceMetadataBehavior metadataBehavior = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (metadataBehavior == null)
            {
                metadataBehavior = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    HttpGetUrl = baseAddress,
                    MetadataExporter = {PolicyVersion = PolicyVersion.Policy15}
                };
                host.Description.Behaviors.Add(metadataBehavior);
            }
            
            Binding mexBinding = MetadataExchangeBindings.CreateMexHttpBinding();
            host.AddServiceEndpoint(typeof(IMetadataExchange), mexBinding, "mex");
            Binding wsBinding = new WSHttpBinding();
            host.AddServiceEndpoint(typeof(IMyService), wsBinding, "");
            host.Open();

            Console.WriteLine("Service on. Press any key to close..."); 
            Console.ReadKey();
            host.Close();
        }
    }
}
