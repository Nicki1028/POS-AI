using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF.AItool
{
    public class AIResponseModel
    {

        public Candidate[] candidates { get; set; }
        public Usagemetadata usageMetadata { get; set; }
        public string modelVersion { get; set; }
        public string responseId { get; set; }


        public class Usagemetadata
        {
            public int promptTokenCount { get; set; }
            public int candidatesTokenCount { get; set; }
            public int totalTokenCount { get; set; }
            public Prompttokensdetail[] promptTokensDetails { get; set; }
            public int thoughtsTokenCount { get; set; }
        }

        public class Prompttokensdetail
        {
            public string modality { get; set; }
            public int tokenCount { get; set; }
        }

        public class Candidate
        {
            public Content content { get; set; }
            public string finishReason { get; set; }
            public int index { get; set; }
            public string finishMessage { get; set; }
        }

        public class Content
        {
            public Part[] parts { get; set; }
            public string role { get; set; }
        }

        public class Part
        {
            public string text { get; set; }
            public Functioncall functionCall { get; set; }
            public string thoughtSignature { get; set; }
        }

        public class Functioncall
        {
            public string name { get; set; }
            public object args { get; set; }
            public string id { get; set; }
        }

    }
}
