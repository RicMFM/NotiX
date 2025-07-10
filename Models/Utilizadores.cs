﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotiX.Models
{
	/// <summary>
	/// Classe para descrever os Utilizadores existentes na redação
	/// </summary>

	public class Utilizadores
    {
        public Utilizadores()
        {
            this.DataInicio = DateTime.Now;
        }

        /// <summary>
        /// Identificador do utilizador
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do utilizador
        /// </summary>
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [StringLength(60)]
        public string Nome { get; set; }

        /// <summary>
        /// Contacto de telemovel do utilizador
        /// </summary>
        [Display(Name = "Telemóvel")]
        [StringLength(9)]
        [RegularExpression("9[1236][0-9]{7}", ErrorMessage = "o {0} só aceita 9 digitos")]
        public string? Contacto { get; set; }
        /// <summary>
        /// Idade do utilizador
        /// </summary>
        [DataType(DataType.Date)] // informa a View de como deve tratar este atributo
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]


		///<summary>
		///Data de Nascimento do utilizador
		/// </summary>
		[Display(Name = "Data de nascimento")]
		public DateOnly? DataNascimento { get; set; }

        /// <summary>
        /// Data de registo do utilizador
        /// </summary>
        [Display(Name = "Data de criação")]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// atributo para funcionar como FK
        /// no relacionamento entre a 
        /// base de dados do 'negócio' 
        /// e a base de dados da 'autenticação'
        /// </summary>
        [StringLength(40)]
        public string UserName { get; set; } = string.Empty;
    }
}

