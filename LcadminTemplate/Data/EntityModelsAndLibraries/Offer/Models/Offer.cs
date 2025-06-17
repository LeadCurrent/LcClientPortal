using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data;

public partial class Offer
{
    public int Id { get; set; }

    public int Schoolid { get; set; }

    public int Clientid { get; set; }

    public string Url { get; set; }

    public bool Active { get; set; }

    public decimal Rpl { get; set; }

    public bool Dcap { get; set; }

    public int Dcapamt { get; set; }

    public bool Mcap { get; set; }

    public int Mcapamt { get; set; }

    public bool Wcap { get; set; }

    public int Wcapamt { get; set; }

    public string Type { get; set; }

    public bool Militaryonly { get; set; }

    public bool Nomilitary { get; set; }

    [NotMapped]
    public bool IsChecked { get; set; }
    [NotMapped]
    public string DayHtml { get; set; }
    [NotMapped]
    public string WeekHtml { get; set; }
    [NotMapped]
    public string MonthHtml { get; set; }

    public string Transferphone { get; set; }

    public string Lccampaignid { get; set; }

    public bool Archive { get; set; }

    public string EndClient { get; set; }

    public decimal CecRplA { get; set; }

    public decimal CecRplB { get; set; }

    public decimal CecRplC { get; set; }

    public decimal CecRplD { get; set; }

    public decimal CecRplE { get; set; }

    public decimal CecRplF { get; set; }

    public decimal CecRplG { get; set; }

    public string DeliveryIdentifier { get; set; }

    public string DeliveryName { get; set; }
    public int? CompanyId { get; set; }
    public Company Company { get; set; }

    public virtual ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();

    public virtual Client Client { get; set; }
    [NotMapped]
    public virtual Offertargeting Targeting { get; set; }

    public virtual ICollection<Offertargeting> Offertargetings { get; set; } = new List<Offertargeting>();

    public virtual Scholls School { get; set; }

    public int? oldId { get; set; }
}
