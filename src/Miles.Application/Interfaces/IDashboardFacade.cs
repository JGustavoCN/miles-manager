using Miles.Application.DTOs;

namespace Miles.Application.Interfaces;

public interface IDashboardFacade
{
    Task<DashboardDetailsDTO> ObterDetalhesAsync(int userId);
}
