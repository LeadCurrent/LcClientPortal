using System;
using System.Collections.Generic;

namespace Data;

public partial class EduspotsEduapiLog
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Xml { get; set; }

    public string Searchreturnid { get; set; }

    public int Eduapiid { get; set; }
}
