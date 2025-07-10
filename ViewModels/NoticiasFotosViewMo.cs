using System.ComponentModel.DataAnnotations;
using NotiX.Models;

namespace NotiX.ViewModels
{
    public class NoticiasFotosViewMo
    {
        /// <summary>
        /// Nome do ficheiro que contém a Foto
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
		public string Nome { get; set; }

        public Noticias Noticias { get; set; }


	}
}
