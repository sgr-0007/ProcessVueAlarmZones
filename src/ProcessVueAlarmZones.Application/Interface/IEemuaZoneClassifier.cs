using ProcessVueAlarmZones.Domain.Domain;

namespace ProcessVueAlarmZones.Application.Interface
{
    public interface IEemuaZoneClassifier
    {
            Zone Classify(double averageAlarmRate, double percentOutsideTarget);
        
    }
}