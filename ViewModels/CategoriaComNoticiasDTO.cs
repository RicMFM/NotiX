namespace NotiX.ViewModels
{
    public class CategoriaComNoticiasDTO
    {
        public int Id { get; set; }
        public string Categoria { get; set; } = "";
        public List<NoticiaDTO> Noticias { get; set; } = [];
    }
}
