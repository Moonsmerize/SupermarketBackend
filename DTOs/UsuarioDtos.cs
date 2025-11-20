namespace SupermercadoBackend.DTOs
{
	public class RegistroDto
	{
		public string Nombre { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
	}

	public class LoginDto
	{
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}