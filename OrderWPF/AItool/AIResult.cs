using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF.AItool
{
    public class AIResult
    {
        AIResponseModel response { get; set; }

        public bool CanExcuteTool;

        public string ResponseText;
        public AIResult(AIResponseModel aIResponse)
        {
            this.response = aIResponse;
            var part = this.response.candidates[0].content.parts[0];
            if (part.functionCall != null)
            {

                CanExcuteTool = true;
            }
            else
            {
                ResponseText = part.text;
                CanExcuteTool = false;
            }
        }

        public object Runtool()
        {
            var function_call = response.candidates[0].content.parts[0].functionCall;

            if (function_call != null)
            {
                string toolName = function_call.name.Replace("_", ".");

                string toolAgs = JsonConvert.SerializeObject(function_call.args);
                MethodInfo methodInfo = typeof(JsonConvert).GetMethods().First(x => x.Name == "DeserializeObject" && x.IsGenericMethod
                && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(string));

                //MethodInfo methodInfo = typeof(JsonConvert).GetMethod("DeserializeObject",new Type[] {typeof(string)});
                Type type = Type.GetType($"{toolName}Model");
                MethodInfo genericMethod = methodInfo.MakeGenericMethod(type);
                var result = genericMethod.Invoke(null, new object[] { toolAgs });

                MethodInfo functionCall = Type.GetType(toolName).GetMethod("FunctionCall", new Type[] { typeof(object) });
                var obj = Activator.CreateInstance(Type.GetType(toolName));
               return functionCall.Invoke(obj, new object[] { result });
            }

            return null;

        }
    }
}

