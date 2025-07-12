namespace NotiX.ViewModels
{
    public class NoticiaComFotosDTO
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Texto { get; set; }
        public DateTime DataEscrita { get; set; }
        /// <summary>
        /// Data de Edição da noticia
        /// </summary>
        public DateTime? DataEdicao { get; set; }

        public List<FotoDTO> ListaFotos { get; set; } = [];
        
    }
}
