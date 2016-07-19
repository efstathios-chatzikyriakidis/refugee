using System.Collections.Generic;

namespace Refugee.Rest.Dto.Output
{
    public class GraphOutputDto
    {
        public IList<NodeOutputDto> Nodes { get; set; }

        public IList<LinkOutputDto> Links { get; set; }
    }
}