using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProcessVueAlarmZones.Domain;

namespace ProcessVueAlarmZones.Application.Interface
{
    public interface IEemuaZoneClassifier
    {
            Zone Classify(double averageAlarmRate, double percentOutsideTarget);
        
    }
}