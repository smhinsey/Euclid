namespace CompositeInspector.Models
{
    public class AgentModel : InspectorNavigationModel
    {
        public AgentPartModel Commands { get; set; }

        public string DescriptiveName { get; set; }

        public AgentPartModel Queries { get; set; }

        public AgentPartModel ReadModels { get; set; }

        public string SystemName { get; set; }
    }
}