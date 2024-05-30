namespace PruebaTecnica.Core.Dtos
{
    public class UserAuthDto
	{
		public int usuarioID { get; set; } // (int, not null)
        public string? usuario { get; set; } // (nvarchar(25), null)
        public string? nombre { get; set; } // (nvarchar(25), null)
        public string? PassCorrect { get; set; } // (varchar(1), not null)
	}
}
