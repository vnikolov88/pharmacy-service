using System;
using System.Collections.Generic;

namespace PharmacyService.Models
{
    public class GeographicalPoint
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class PharmacyLocation
    {
        public GeographicalPoint GeographicalPoint { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
    }

    public class PharmacyOpeningTimeDate
    {
        public DateTime Date { get; set; }
        public string TimeZone { get; set; }
    }

    public class PharmacyOpeningTime
    {
        public PharmacyOpeningTimeDate Start { get; set; }
        public PharmacyOpeningTimeDate End { get; set; }
    }

    public class PharmacyElectronicAddress
    {
        public bool Approved { get; set; }
        public string Identifier { get; set; }
        public string Type { get; set; }
        public string Usage { get; set; }
    }

    public class Pharmacy
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PharmacyLocation Location { get; set; }
        public string Phone { get; set; }
        public IDictionary<string, PharmacyOpeningTime> OpeningTimes { get; set; }
        public double DistanceMetric { get; set; }
        public IDictionary<string, PharmacyElectronicAddress> ElectronicAddresses { get; set; }
        public PharmacyElectronicAddress PrimaryEmail { get; set; }
    }
}
