using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace SneakersBase.Client.Components
{
    public class ModelDialogBase : ComponentBase
    {
        protected bool DeleteDialogOpen { get; set; } = false;
        [Parameter] public string Title { get; set; }
        [Parameter] public string Text { get; set; }
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public ModalDialogType DialogType { get; set; }

     
      

        protected Task ModalCancel()
        {
            Close();
            return OnClose.InvokeAsync(false);
        }

        protected Task ModalOk()
        {
            Close();
            return OnClose.InvokeAsync(true);
        }

        public void Close()
        {
            DeleteDialogOpen = false;
            StateHasChanged();
        }

        public void Show()
        {
            DeleteDialogOpen = true;
            StateHasChanged();
        }

        public enum ModalDialogType
        {
            Ok,
            OkCancel,
            DeleteCancel
        }
    }
}

