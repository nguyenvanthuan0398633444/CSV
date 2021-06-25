using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace m_processing_month
{
    [Table("m_processing_month", Schema = "public")]
    public class ProcessingMonth
    {
        [Key]
        [Required]
        [StringLength(6)]
        [Display(Name = "�������x")]
        [Column("processing_month")]
        public string Processing_month { get; set; }

    }
}