using CityInfo.API.Interfaces;

namespace CityInfo.API.Models
{
    public class PointOfInterestUpdateDto : PointOfInterestBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
