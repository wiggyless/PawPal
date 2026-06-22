using System.ComponentModel;

namespace PawPal.Domain.Entities.Moderation
{
    public enum ReportPostEnum
    {
        [Description("This post was created using Generative AI")]
        AIGen,
        [Description("This post contains misinformation")]
        Misinformation,
        [Description("This post promotes hate speech")]
        HateSpeech,
        [Description("This post shows signs of animal abuse or neglect")]
        AnimalAbuse,
        [Description("This post contains inappropriate or graphic images")]
        Graphic,
        [Description("This post is rehoming an animal that may be stolen")]
        Stolen,
        [Description("Other")]
        Other
    }
}
