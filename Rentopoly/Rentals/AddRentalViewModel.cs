using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MaterialDesignThemes.Wpf;

using Rentopoly.Data;

namespace Rentopoly.Rentals;

public partial class AddRentalViewModel(BoardGameContext dbContext, ISnackbarMessageQueue messageQueue)
    : ObservableObject
{
    private BoardGameContext DbContext { get; } = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    public ISnackbarMessageQueue MessageQueue { get; } = messageQueue ?? throw new ArgumentNullException(nameof(messageQueue));

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    private string? _loanedTo;

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private async Task OnSubmit()
    {
        if (string.IsNullOrWhiteSpace(LoanedTo))
        {
            return;
        }
        Rental newRental = new()
        {
            LoanedOn = DateTime.Today,
            LoanedTo = LoanedTo
        };
        DbContext.Rentals.Add(newRental);
        await DbContext.SaveChangesAsync();
        LoanedTo = string.Empty;
        MessageQueue.Enqueue("Saved!");
    }

    private bool CanSubmit() => !string.IsNullOrWhiteSpace(LoanedTo);
}
