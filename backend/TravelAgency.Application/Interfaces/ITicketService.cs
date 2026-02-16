using TravelAgency.Application.DTOs;

namespace TravelAgency.Application.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<TicketRequestDto>> GetAllTicketsAsync();
    Task<TicketRequestDto?> GetTicketByIdAsync(int id);
    Task<IEnumerable<TicketRequestDto>> GetTicketsByUserIdAsync(int userId);
    Task<TicketRequestDto> CreateTicketAsync(int userId, CreateTicketRequestDto createDto);
    Task<TicketRequestDto> UpdateTicketAsync(int id, UpdateTicketRequestDto updateDto);
    Task<TicketRequestDto> UpdateTicketStatusAsync(int id, UpdateTicketStatusDto updateStatusDto);
    Task<bool> DeleteTicketAsync(int id);
    Task<IEnumerable<TicketRequestDto>> GetTicketsByStatusAsync(string status);
}
