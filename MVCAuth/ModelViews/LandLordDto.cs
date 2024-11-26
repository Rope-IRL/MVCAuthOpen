namespace MVCAuth.ModelViews;

public class LandLordDto
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;
}