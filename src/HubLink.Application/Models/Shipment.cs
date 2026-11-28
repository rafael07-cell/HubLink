namespace HubLink.Application.Models;

public sealed record Shipment(
    string TrackingNumber,
    string BranchCode,
    string Origin,
    string Destination,
    ShipmentStatus Status,
    DateTimeOffset LastUpdatedUtc);
