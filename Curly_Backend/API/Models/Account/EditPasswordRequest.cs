namespace API.Models.Account;

public class EditPasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}