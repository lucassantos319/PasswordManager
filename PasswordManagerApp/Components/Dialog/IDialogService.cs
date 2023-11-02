namespace PasswordManagerApp.Components.Dialog
{
    public interface IDialogService
    {
        Task<bool> DisplayConfirm(string title, string message, string accept, string cancel);
    }
}