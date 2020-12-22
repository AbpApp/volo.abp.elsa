using Elsa.Models;

namespace Volo.Abp.WorkFlowManagement
{
    public class ConnectionDto
    {
        public ConnectionDto()
        {
        }
        
        

        public string? SourceActivityId { get; set; }
        public string? DestinationActivityId { get; set; }
        public string? Outcome { get; set; }
    }
}