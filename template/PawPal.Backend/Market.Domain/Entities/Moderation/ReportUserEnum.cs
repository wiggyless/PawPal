using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Moderation
{
    public enum ReportUserEnum
    {
        [Description("Tung tung sahur")]
        AIGen,
        [Description("Tung tung sahur")]
        Misinformation,
        [Description("Tung tung sahur")]
        HateSpeech,
        [Description("Tung tung sahur")]
        AnimalAbuse,
        [Description("Tung tung sahur")]
        Graphic,
    }
}
