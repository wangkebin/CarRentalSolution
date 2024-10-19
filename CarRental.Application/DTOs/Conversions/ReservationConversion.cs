using CarRental.Domain.Entities;

namespace CarRental.Application.DTOs.Conversions;

public static class ReservationConversion
{
    public static Reservation ToReservation(this ReservationDTO reservationDto) => new Reservation
    {
        Id = reservationDto.Id,
        CarId = reservationDto.CarId,
        CustomerId = reservationDto.CustomerId,
        ReservationStartDateTime = reservationDto.ReservationStartDateTime,
        ReservationEndDateTime = reservationDto.ReservationEndDateTime,
        Note = reservationDto.Note
    };

    public static ReservationDTO? FromReservation(this Reservation? reservation)
    {
        if (reservation == null) return null;
        return new ReservationDTO(
            Id: reservation!.Id,
            CarId: reservation!.CarId,
            CustomerId: reservation!.CustomerId,
            ReservationStartDateTime: reservation!.ReservationStartDateTime,
            ReservationEndDateTime: reservation!.ReservationEndDateTime,
            Note: reservation!.Note
        );
    }

    public static IEnumerable<ReservationDTO>? FromReservation(this IEnumerable<Reservation>? reservations)
    {
        if (reservations == null) return null;
        return reservations.Select(r => r.FromReservation()!);
    }
}