using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Rentopoly.Data;

namespace Rentopoly.Rentals;

public partial class RentalItemViewModel(Rental rental, BoardGameContext boardGameContext, IMessenger messenger)
{
    private int RentalId { get; } = rental.Id;
    public string LoanedTo { get; } = rental.LoanedTo;
    public DateTime LoanedOn { get; } = rental.LoanedOn;
    public DateTime? ReturnedOn { get; } = rental.ReturnedOn;
    private BoardGameContext BoardGameContext { get; } = boardGameContext;
    private IMessenger Messenger { get; } = messenger;

    [RelayCommand]
    public async Task OnReturned()
    {
        if (await BoardGameContext.Rentals.FindAsync(RentalId) is { } foundRental)
        {
            foundRental.ReturnedOn = DateTime.Now;
            await BoardGameContext.SaveChangesAsync();
            Messenger.Send(new RentalUpdates(foundRental));
        }
    }
}

public record class RentalUpdates(Rental Rental);
