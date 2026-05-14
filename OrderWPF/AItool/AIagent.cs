using HttpUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace OrderWPF.AItool
{
    internal class AIagent
    {
        AIRequestModel requestModel;
        AIResponseModel response;
        HttpRequest httpRequest;
        public AIagent()
        {   
            httpRequest = new HttpRequest();
            Assembly assembly = Assembly.GetExecutingAssembly();
            var data = assembly.DefinedTypes.Where(x => x.BaseType == typeof(AFunctionDeclaration))
                                            .Select(x => (AFunctionDeclaration)Activator.CreateInstance(x)).ToList();
            requestModel = new AIRequestModel();
            requestModel.tools[0].functionDeclarations = data;
        }

        public void AddPrompt(string prompt, string role = "user")
        {
            requestModel.contents.Add(new AIRequestModel.Content()
            {
                role = role,
                parts = new AIRequestModel.Part[]
                {
           new AIRequestModel.Part()
           {
               text = prompt,
           }
                }
            });
        }

        public async Task<AIResult> GetResult()
        {
            response = await GetResponseAsync(requestModel);
            AIResult result = new AIResult(response);
            string message = result.CanExcuteTool ? "好的，已經按照您的指示完成執行，請問還有甚麼需要幫忙嗎?" : result.ResponseText;
            AddPrompt(message, "model");
            return result;
        }
        private async Task<AIResponseModel> GetResponseAsync(AIRequestModel aIRequestModel)
        {
            httpRequest.BaseUrl = "https://generativelanguage.googleapis.com/";

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
            {
                { "x-goog-api-key" , "AIzaSyCWcJEmsZwPRKRCH8U0woZ9S2JHirrTFik" }
            };
            httpRequest.AddHeaders(keyValuePairs);

            AIResponseModel response = await httpRequest.Post<AIResponseModel>("v1beta/models/gemini-3.1-flash-lite-preview:generateContent", aIRequestModel);
            return response;
        }
    }
}
