using RX.Nyss.Web.Features.Common.Dto;

namespace RX.Nyss.Web.Features.DataCollectors.Dto
{
    public class DataCollectorPerformanceFiltersRequestDto
    {
        public AreaDto Locations { get; set; }

        public string Name { get; set; }

        public int? SupervisorId { get; set; }

        public TrainingStatusDto TrainingStatus { get; set; }

        public int PageNumber { get; set; }
    }
}
