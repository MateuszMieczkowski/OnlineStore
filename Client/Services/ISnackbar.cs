// using MudBlazor;
// using System.Timers;
// using Timer = System.Timers.Timer;
//
// namespace OnlineStore.Client.Services;
//
// // public enum Severity
// // {
// //     Info,
// //     Success,
// //     Warning,
// //     Error
// // }
//
// public class ISnackbar
// {
//     private readonly MudBlazor.ISnackbar _snackbar;
//     private Timer Countdown;
//     public event Action<string, Severity> OnShow;
//     public event Action OnHide;
//
//     public ISnackbar(MudBlazor.ISnackbar snackbar)
//     {
//         _snackbar = snackbar ?? throw new ArgumentNullException(nameof(snackbar));
//
//     }
//     
//     public void Add(string message, MudBlazor.Severity severity)
//     {
//         
//     }
// }