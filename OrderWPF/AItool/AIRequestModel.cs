using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF.AItool
{
    public class AIRequestModel
    {
        public List<Content> contents { get; set; } = new List<AIRequestModel.Content>();
        public Tool[] tools { get; set; } = new AIRequestModel.Tool[]
        {
            new AIRequestModel.Tool()
        };

        public class Content
        {
            public string role { get; set; }
            public Part[] parts { get; set; }
        }

        public class Part
        {
            public string text { get; set; }
        }

        public class Tool
        {
            public List<AFunctionDeclaration> functionDeclarations { get; set; }

            public Tool()
            {

            }

        }

        public class Items
        {
            public string type { get; set; }
        }


    }
}
