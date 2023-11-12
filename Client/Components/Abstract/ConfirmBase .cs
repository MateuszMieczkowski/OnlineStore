using Microsoft.AspNetCore.Components;

namespace OnlineStore.Client.Components;

public class ConfirmBase : ComponentBase
{
    protected bool ShowConfirmation { get; set; }

    [Parameter] public string ConfirmationTitle { get; set; } = "Confirm Delete";

    [Parameter] public string ConfirmationMessage { get; set; } = "Are you sure you want to delete";

    [Parameter] public EventCallback<bool> ConfirmationChanged { get; set; }

    public void Show()
    {
        ShowConfirmation = true;
        StateHasChanged();
    }

    protected async Task OnConfirmationChange(bool value)
    {
        ShowConfirmation = false;
        await ConfirmationChanged.InvokeAsync(value);
        }
}