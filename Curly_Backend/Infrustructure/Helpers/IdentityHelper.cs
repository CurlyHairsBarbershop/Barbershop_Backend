namespace Infrustructure.Helpers;

public static class IdentityHelper
{
    public static string GeneratePassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";

        var random = new Random();
        
        // Generate at least one character of each type
        var password = new string(new[]
        {
            chars[random.Next(chars.Length / 2)],         // Uppercase letter
            chars[random.Next(chars.Length / 2, chars.Length)],   // Lowercase letter
            chars[random.Next(chars.Length / 2, chars.Length)],   // Digit
            chars[random.Next(chars.Length / 2, chars.Length)]    // Special character
        });

        // Generate the rest of the password
        for (var i = password.Length; i < 8; i++)
        {
            password += chars[random.Next(chars.Length)];
        }

        // Shuffle the password characters
        password = new string(password.ToCharArray().OrderBy(c => random.Next()).ToArray());

        return password;
    }
}