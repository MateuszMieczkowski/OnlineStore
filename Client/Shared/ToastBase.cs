using Microsoft.AspNetCore.Components;
using OnlineStore.Client.Services;

namespace OnlineStore.Client.Shared;

public class ToastBase : ComponentBase, IDisposable
{
    [Inject] private ToastService ToastService { get; set; }

    protected string Heading { get; set; }
    protected string Message { get; set; }
    protected bool IsVisible { get; set; }
    protected string BackgroundCssClass { get; set; }
    protected string IconCssClass { get; set; }

    public void Dispose()
    {
        ToastService.OnShow -= ShowToast;
    }

    protected override void OnInitialized()
    {
        ToastService.OnShow += ShowToast;
        ToastService.OnHide += HideToast;
    }

    private void ShowToast(string message, ToastLevel level)
    {
        BuildToastSettings(level, message);
        IsVisible = true;
        StateHasChanged();
    }

    private void HideToast()
    {
        IsVisible = false;
        StateHasChanged();
    }

    private void BuildToastSettings(ToastLevel level, string message)
    {
        switch (level)
        {
            case ToastLevel.Info:
                BackgroundCssClass = "bg-info";
                IconCssClass = "info";
                Heading = "Info";
                break;
            case ToastLevel.Success:
                BackgroundCssClass = "bg-success";
                IconCssClass = "check";
                Heading = "Operacja zaakceptowana";
                break;
            case ToastLevel.Warning:
                BackgroundCssClass = "bg-warning";
                IconCssClass = "exclamation";
                Heading = "Warning";
                break;
            case ToastLevel.Error:
                BackgroundCssClass = "bg-danger";
                IconCssClass = "times";
                Heading = "Wystąpił błąd";
                break;
        }

        Message = message;
    }
}