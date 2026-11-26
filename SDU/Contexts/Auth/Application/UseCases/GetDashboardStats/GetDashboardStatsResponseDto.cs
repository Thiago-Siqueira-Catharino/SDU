namespace SDU.Contexts.Auth.Application.UseCases.GetDashboardStats;

public record GetDashboardStatsResponseDto(
    int FileAmount, 
    double TotalUsedStorage, 
    string CNPJ, 
    string Name
    );

