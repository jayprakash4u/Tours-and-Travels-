using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;
using TravelAgency.Domain.Entities;
using TravelAgency.Domain.Enums;
using TravelAgency.Infrastructure.Repositories;

namespace TravelAgency.Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<TicketRequestDto>> GetAllTicketsAsync()
    {
        var tickets = await _ticketRepository.GetAllAsync();
        return tickets.Select(MapToDto);
    }

    public async Task<TicketRequestDto?> GetTicketByIdAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        return ticket == null ? null : MapToDto(ticket);
    }

    public async Task<IEnumerable<TicketRequestDto>> GetTicketsByUserIdAsync(int userId)
    {
        var tickets = await _ticketRepository.GetByUserIdAsync(userId);
        return tickets.Select(MapToDto);
    }

    public async Task<TicketRequestDto> CreateTicketAsync(int userId, CreateTicketRequestDto createDto)
    {
        // Verify user exists
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException($"User with ID {userId} not found");

        var ticket = new TicketRequest
        {
            UserId = userId,
            FromLocation = createDto.FromLocation,
            ToLocation = createDto.ToLocation,
            TravelDate = createDto.TravelDate,
            TicketType = createDto.TicketType,
            NumberOfPassengers = createDto.NumberOfPassengers,
            Notes = createDto.Notes,
            Status = BookingStatus.Pending,
            CreatedDate = DateTime.UtcNow
        };

        var createdTicket = await _ticketRepository.AddAsync(ticket);
        await _ticketRepository.SaveChangesAsync();

        return MapToDto(createdTicket);
    }

    public async Task<TicketRequestDto> UpdateTicketAsync(int id, UpdateTicketRequestDto updateDto)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
            throw new InvalidOperationException($"Ticket with ID {id} not found");

        ticket.FromLocation = updateDto.FromLocation;
        ticket.ToLocation = updateDto.ToLocation;
        ticket.TravelDate = updateDto.TravelDate;
        ticket.TicketType = updateDto.TicketType;
        ticket.NumberOfPassengers = updateDto.NumberOfPassengers;
        ticket.Notes = updateDto.Notes;

        var updatedTicket = await _ticketRepository.UpdateAsync(ticket);
        await _ticketRepository.SaveChangesAsync();

        return MapToDto(updatedTicket);
    }

    public async Task<TicketRequestDto> UpdateTicketStatusAsync(int id, UpdateTicketStatusDto updateStatusDto)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
            throw new InvalidOperationException($"Ticket with ID {id} not found");

        ticket.Status = updateStatusDto.Status;
        if (updateStatusDto.Price.HasValue)
            ticket.EstimatedPrice = updateStatusDto.Price.Value;

        if (updateStatusDto.Status == BookingStatus.Completed)
            ticket.CompletedDate = DateTime.UtcNow;

        var updatedTicket = await _ticketRepository.UpdateAsync(ticket);
        await _ticketRepository.SaveChangesAsync();

        return MapToDto(updatedTicket);
    }

    public async Task<bool> DeleteTicketAsync(int id)
    {
        var deleted = await _ticketRepository.DeleteAsync(id);
        if (deleted)
            await _ticketRepository.SaveChangesAsync();
        return deleted;
    }

    public async Task<IEnumerable<TicketRequestDto>> GetTicketsByStatusAsync(string status)
    {
        if (!Enum.TryParse<BookingStatus>(status, true, out var bookingStatus))
            throw new InvalidOperationException($"Invalid status: {status}");

        var tickets = await _ticketRepository.GetByStatusAsync(bookingStatus);
        return tickets.Select(MapToDto);
    }

    private static TicketRequestDto MapToDto(TicketRequest ticket)
    {
        return new TicketRequestDto
        {
            Id = ticket.Id,
            UserId = ticket.UserId,
            FromLocation = ticket.FromLocation,
            ToLocation = ticket.ToLocation,
            TravelDate = ticket.TravelDate,
            TicketType = ticket.TicketType,
            Status = ticket.Status,
            NumberOfPassengers = ticket.NumberOfPassengers,
            EstimatedPrice = ticket.EstimatedPrice,
            Notes = ticket.Notes,
            CreatedDate = ticket.CreatedDate
        };
    }
}
