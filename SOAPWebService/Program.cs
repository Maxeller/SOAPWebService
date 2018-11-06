using System;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace SOAPWebService {
    class Program
    {
        public static PolicyVersion PolicyVersion { get; private set; }

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
                    MetadataExporter = { PolicyVersion = PolicyVersion.Policy15 }
                };
                host.Description.Behaviors.Add(metadataBehavior);
            }
            
            Binding mexBinding = MetadataExchangeBindings.CreateMexHttpBinding();
            host.AddServiceEndpoint(typeof(IMetadataExchange), mexBinding, "mex");
            Binding httpBinding = new BasicHttpBinding(); //WSHttpBinding doesn't work with SOAPUI
            httpBinding.Namespace = "4it475.vse.cz";
            host.AddServiceEndpoint(typeof(IMyService), httpBinding, "");
            host.Open();

            Console.WriteLine("Service on. Press Enter key to close..."); 
            Console.ReadLine();
            host.Close();
        }
    }
}
