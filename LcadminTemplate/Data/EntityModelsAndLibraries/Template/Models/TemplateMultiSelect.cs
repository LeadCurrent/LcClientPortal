using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Data
{
    public class TemplateMultiSelect
    {
        public int Id { get; set; }
        public Template Template { get; set; }
        public int TemplateId { get; set; }
        public int SampleDropdown { get; set; }
    }
}