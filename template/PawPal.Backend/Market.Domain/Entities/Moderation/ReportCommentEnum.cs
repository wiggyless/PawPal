using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Moderation
{
    public enum ReportCommentEnum
    {
        [Description("This comment was generated using AI")]
        AIGen,
        [Description("This comment contains misinformation")]
        Misinformation,
        [Description("This comment promotes hate speech")]
        HateSpeech,
        [Description("This comment shows signs of animal abuse or neglect")]
        AnimalAbuse,
        [Description("This comment contains inappropriate or graphic content")]
        Graphic,
    }
}
