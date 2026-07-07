using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Domain.Entities.Moderation
{   public enum ReportUserEnum
    {
        [Description("This user is not responding after being confirmed for adoption")]
        Unresponsive,

        [Description("This user showed aggressive or abusive behavior")]
        AbusiveBehavior,

        [Description("This user is harassing me or another member")]
        Harassment,

        [Description("This user tried to sell an animal instead of adopting it out")]
        IllegalSelling,

        [Description("This user appears to be operating a scam or fake account")]
        ScamOrFakeAccount,

        [Description("This user has a history of neglecting or mistreating adopted animals")]
        AnimalMistreatmentHistory,

        [Description("This user is spamming or creating multiple fake listings")]
        SpamAccount,

        [Description("Other")]
        Other
    }
}

