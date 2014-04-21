using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sider.IO.Client.DataProvider.Net;
using Sider.IO.Client.DataProvider.Net.Commands;
using Sider.IO.Api.Dto.Message;
using Sider.IO.Api.Utils.Conversion;

namespace BasicExample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Connection connection = new Connection("127.0.0.1", 11000))
            {
                connection.Open(); 
                SiderResponse response;
                
                Console.WriteLine("========== Set ==============================================================");
                
                Set setCommand = new Set("users/1/name", "Fay");
                response = connection.Execute(setCommand);

                Console.WriteLine(response.ReturnCode); //Expected response: OK 
                Console.WriteLine(String.Format("Response as expected: {0}", response.ReturnCode == Sider.IO.Api.Dto.Enums.ReturnCode.OK)); //Expected response: OK 


                Console.WriteLine("========== Set with Delay ===================================================");

                Set delayedSetCommand = new Set("users/2/name", "Dane", 3000); //Delay for 3000 milliseconds                                
                response = connection.Execute(delayedSetCommand);

                Console.WriteLine(response.ReturnCode); //Expected response: Delay_OK 
                Console.WriteLine(String.Format("Response as expected: {0}", response.ReturnCode == Sider.IO.Api.Dto.Enums.ReturnCode.Delay_OK)); //Expected response: OK 


                Console.WriteLine("========== Get ==============================================================");

                Get getCommand = new Get("users/1/name");
                string returnValue = connection.Execute<string>(getCommand);

                Console.WriteLine(returnValue);
                Console.WriteLine(String.Format("Response as expected: {0}", returnValue == "Fay")); //Expected response: "Fay"

                
                Console.WriteLine("========== Get ==============================================================");
                                
                Get getCommandForSetDelay = new Get("users/2/name");
                string returnValueForSetDelay = connection.Execute<string>(getCommandForSetDelay);

                Console.WriteLine(returnValueForSetDelay);
                Console.WriteLine(String.Format("Response as expected: {0}", returnValueForSetDelay == null)); //Expected response: null because the Set command with the delay specified has not been executed yet.


                Console.WriteLine("========== Delete ===========================================================");

                Delete deleteCommand = new Delete("users/1/name");
                response = connection.Execute(deleteCommand);

                Console.WriteLine(response.ReturnCode);
                Console.WriteLine(String.Format("Response as expected: {0}", response.ReturnCode == Sider.IO.Api.Dto.Enums.ReturnCode.OK)); //Expected response: "OK"


                Console.WriteLine("========== Subscribe to non existent channel ===========================================================");

                Subscribe subscribeCommand = new Subscribe("groupchats/1234", "dane@somedomain.com");
                response = connection.Execute(subscribeCommand);

                Console.WriteLine(response.ReturnCode);
                Console.WriteLine(String.Format("Response as expected: {0}", response.ReturnCode == Sider.IO.Api.Dto.Enums.ReturnCode.Resource_Not_Found)); //Expected response: "OK"

                Console.WriteLine("========== Publish to non existent channel ===========================================================");

                Publish publishCommand = new Publish("groupchats/1234", "dane@somedomain.com", "Create channel if it does not exist, otherwise this text will be published to the existing channel and all current subscribers will receive this message.");
                response = connection.Execute(publishCommand);

                Console.WriteLine(response.ReturnCode);
                Console.WriteLine(String.Format("Response as expected: {0}", response.ReturnCode == Sider.IO.Api.Dto.Enums.ReturnCode.OK)); //Expected response: "OK"

                Console.WriteLine("========== Subscribe to existing channel ===========================================================");
          
                Subscribe subscribeToExistingChannelCommand = new Subscribe("groupchats/1234", "dane@somedomain.com");
                response = connection.Execute(subscribeToExistingChannelCommand);

                Console.WriteLine(response.ReturnCode);
                Console.WriteLine(String.Format("Response as expected: {0}", response.ReturnCode == Sider.IO.Api.Dto.Enums.ReturnCode.OK)); //Expected response: "OK"

                Console.WriteLine("========== Unsubscribe ===========================================================");

                Unsubscribe unsubscribeCommand = new Unsubscribe("groupchats/1234", "dane@somedomain.com");
                response = connection.Execute(unsubscribeCommand);

                Console.WriteLine(response.ReturnCode);
                Console.WriteLine(String.Format("Response as expected: {0}", response.ReturnCode == Sider.IO.Api.Dto.Enums.ReturnCode.OK)); //Expected response: "OK"


                Console.WriteLine("========== Unsubscribe ===========================================================");

                Unsubscribe unsubscribeToNonExistentCommand = new Unsubscribe("groupchats/5678", "dane@somedomain.com");
                response = connection.Execute(unsubscribeToNonExistentCommand);

                Console.WriteLine(response.ReturnCode);
                Console.WriteLine(String.Format("Response as expected: {0}", response.ReturnCode == Sider.IO.Api.Dto.Enums.ReturnCode.Resource_Not_Found)); //Expected response: "Resource_Not_Found"

                
                //Set again for truncate example
                response = connection.Execute(setCommand);
                Console.WriteLine(response.ReturnCode);
                Console.WriteLine(String.Format("Response as expected: {0}", response.ReturnCode == Sider.IO.Api.Dto.Enums.ReturnCode.OK)); //Expected response: "OK"

                //Get again for truncate example   
                returnValue = connection.Execute<string>(getCommand);

                Console.WriteLine(returnValue);
                Console.WriteLine(String.Format("Response as expected: {0}", returnValue == "Fay")); //Expected response: "Fay"
                               
                Console.WriteLine("========== Truncate ===========================================================");
                Truncate truncateCommand = new Truncate();
                response = connection.Execute(truncateCommand);                   

                Console.WriteLine(response.ReturnCode);
                Console.WriteLine(String.Format("Response as expected: {0}", response.ReturnCode == Sider.IO.Api.Dto.Enums.ReturnCode.OK)); //Expected response: "OK"

                //Get again for truncate example   
                returnValue = connection.Execute<string>(getCommand);

                Console.WriteLine(returnValue);
                Console.WriteLine(String.Format("Response as expected: {0}", returnValue == null)); //Expected response: null
            }

            switch (Console.ReadLine())
            {
                case "r":
                    Main(null);
                    break;
                default:
                    Environment.Exit(0);
                    break;
            } 
        }
    }
}
