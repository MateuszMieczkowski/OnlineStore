using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OnlineStore.Client.Services;
using OnlineStore.Shared.Models;

namespace OnlineStore.Client.Pages;

public class SizeBase : ComponentBase
{
    protected override async Task OnInitializedAsync()
    {
        sizes = await SizeService.GetAllAsync();
        CreateSize = new CreateSizeDto();
    }

    protected async void UpdateSize(SizeDto sizeDto)
    {
        try
        {
            ConfirmSize = sizeDto;
            EditSize = new UpdateSizeDto
            {
                Name = sizeDto.Name
            };
            await JsRuntime.InvokeVoidAsync("ScrollTop");
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Toast.ShowToast(ex.Message, ToastLevel.Error);
        }
    }

    protected async void SaveEdit(UpdateSizeDto updateSizeDto)
    {
        try
        {
            var updatedSize = await SizeService.Update(ConfirmSize.Id, updateSizeDto);

            ConfirmSize.Name = updatedSize.Name;
            Toast.ShowToast("Size updated!", ToastLevel.Success);
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Toast.ShowToast(ex.Message, ToastLevel.Error);
        }
    }

    protected async void RemoveSize(SizeDto sizeDto)
    {
        try
        {
            await SizeService.Remove(sizeDto.Id);

            sizes?.Remove(sizeDto);
            Toast.ShowToast("Size removed!", ToastLevel.Success);
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Toast.ShowToast("Something went wrong: " + ex.Message, ToastLevel.Error);
        }
    }


    protected async void AddSize(CreateSizeDto sizeDto)
    {
        try
        {
            var newSize = await SizeService.Create(sizeDto);
            sizes?.Add(newSize);
            Toast.ShowToast("Size added!", ToastLevel.Success);

            sizeDto.Name = string.Empty;
            ;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Toast.ShowToast("Something went wrong: " + ex.Message, ToastLevel.Error);
        }
    }

    #region Injections

    [Inject] protected ToastService Toast { get; set; }

    [Inject] protected ISizeService SizeService { get; set; }

    [Inject] protected IJSRuntime JsRuntime { get; set; }

    #endregion

    #region Properties

    protected List<SizeDto>? sizes;
    protected UpdateSizeDto? EditSize { get; set; }
    protected SizeDto ConfirmSize { get; set; }
    protected CreateSizeDto CreateSize { get; set; }

    #endregion
}